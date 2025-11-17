using DMnDBCS.UI.Models;
using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.UserProfiles;
using DMnDBCS.UI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DMnDBCS.UI.Controllers
{
    public class UserProfilesController(HttpClient httpClient, IConfiguration configuration, IUserProfilesService userProfilesService, IUsersService usersService, IJwtService jwtService) : Controller
    {
        private readonly HttpClient _client = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserProfilesService _userProfilesService = userProfilesService;
        private readonly IUsersService _usersService = usersService;
        private readonly IJwtService _jwtService = jwtService;

        // GET: UserProfilesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var userResponse = await _usersService.GetByIdAsync(id);
            if (!userResponse.IsSuccessful || userResponse.Data == null)
            {
                return NotFound(userResponse.ErrorMessage);
            }

            var profileResponse = await _userProfilesService.GetByIdAsync(id);
            if (!profileResponse.IsSuccessful || profileResponse.Data == null)
            {
                return NotFound(profileResponse.ErrorMessage);
            }

            if (loggedUserId == id)
            {
                ViewBag.CanEdit = true;
            }

            var info = new UserInfoViewModel()
            {
                Id = id,
                Name = userResponse.Data.Name,
                Email = userResponse.Data.Email,
                Phone = profileResponse.Data.Phone,
                Address = profileResponse.Data.Address,
                DateOfBirth = profileResponse.Data.DateOfBirth,
                ProfilePicture = profileResponse.Data.ProfilePicture
            };

            return View(info);
        }

        // GET: UserProfilesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId) || loggedUserId != id)
            {
                return Unauthorized();
            }

            var userResponse = await _usersService.GetByIdAsync(id);
            if (!userResponse.IsSuccessful || userResponse.Data == null)
            {
                return NotFound(userResponse.ErrorMessage);
            }

            var profileResponse = await _userProfilesService.GetByIdAsync(id);
            if (!profileResponse.IsSuccessful || profileResponse.Data == null)
            {
                return NotFound(profileResponse.ErrorMessage);
            }

            var info = new UserInfoViewModel()
            {
                Id = id,
                Name = userResponse.Data.Name,
                Email = userResponse.Data.Email,
                Phone = profileResponse.Data.Phone,
                Address = profileResponse.Data.Address,
                DateOfBirth = profileResponse.Data.DateOfBirth,
                ProfilePicture = profileResponse.Data.ProfilePicture
            };

            return View(info);
        }

        // POST: UserProfilesController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(UserInfoViewModel userInfo)
        {
            try
            {
                var currentEmail = _jwtService.GetUserEmail();
                var apiBaseUrl = _configuration["UriData:ApiUri"];
                var loginResponse = await _client.PostAsJsonAsync($"{apiBaseUrl}auth/login",
                    new { Email = currentEmail, userInfo.Password });

                if (!loginResponse.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Invalid password");
                    return View(userInfo);
                }

                if (!ModelState.IsValid)
                {
                    return View(userInfo);
                }

                string cleanPhone = userInfo.Phone.Replace(" ", "").Replace("-", "");
                var regex = new Regex(@"^\+375(25|29|33|44|17)\d{7}$");

                if (!regex.IsMatch(cleanPhone))
                {
                    ModelState.AddModelError(nameof(userInfo.Phone), "Phone format is +375 XX XXXXXXX");
                    return View(userInfo);
                }

                if (currentEmail != userInfo.Email)
                {
                    var emailUser = await _usersService.GetByEmailAsync(userInfo.Email);
                    if (emailUser.IsSuccessful && emailUser.Data != null && emailUser.Data.Email == userInfo.Email)
                    {
                        ModelState.AddModelError(nameof(userInfo.Email), "User with that Email already exists");
                        return View(userInfo);
                    }
                }

                var phoneUser = await _userProfilesService.GetByPhoneAsync(cleanPhone);
                if (phoneUser.IsSuccessful && phoneUser.Data != null && phoneUser.Data.Phone == cleanPhone && phoneUser.Data.UserId != userInfo.Id)
                {
                    ModelState.AddModelError(nameof(userInfo.Phone), "User with that Phone already exists");
                    return View(userInfo);
                }

                await _usersService.UpdateAsync(new User()
                {
                    Id = userInfo.Id,
                    Email = userInfo.Email,
                    Name = userInfo.Name,
                    Password = userInfo.NewPassword ?? userInfo.Password
                });

                await _userProfilesService.UpdateAsync(new UserProfile()
                {
                    UserId = userInfo.Id,
                    Phone = cleanPhone,
                    Address = userInfo.Address,
                    DateOfBirth = userInfo.DateOfBirth,
                    ProfilePicture = userInfo.ProfilePicture,
                }, userInfo.ProfilePictureFile);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }
    }
}
