using DMnDBCS.UI.Models;
using DMnDBCS.UI.Services.UserProfiles;
using DMnDBCS.UI.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class AuthController(HttpClient httpClient, IConfiguration configuration, IUserProfilesService userProfilesService, IUsersService usersService) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserProfilesService _userProfilesService = userProfilesService;
        private readonly IUsersService _usersService = usersService;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var apiBaseUrl = _configuration["UriData:ApiUri"];
            var response = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                HttpContext.Session.SetString("JWTToken", authResponse!.Token);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(request);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (request.Password != request.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(request.ConfirmPassword), "Passwords do not match");
                return View(request);
            }

            if (request.DateOfBirth > DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
            {
                ModelState.AddModelError(nameof(request.DateOfBirth), "Person needs to be 18 y.o.");
                return View(request);
            }

            var apiBaseUrl = _configuration["UriData:ApiUri"];
            var registerRequest = new LoginRequest(request.Name, request.Email, request.Password);
            var response = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}auth/register", registerRequest);

            var userResponse = await _usersService.GetByEmailAsync(request.Email);

            if (!response.IsSuccessStatusCode || !userResponse.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                return View(request);
            }

            var profile = new UserProfile()
            {
                UserId = userResponse.Data!.Id,
                Phone = request.Phone,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                ProfilePicture = null
            };
            await _userProfilesService.CreateAsync(profile, request.ProfilePictureFile);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}auth/login",
                    new { request.Email, request.Password });

                if (loginResponse.IsSuccessStatusCode)
                {
                    var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
                    HttpContext.Session.SetString("JWTToken", authResponse!.Token);
                    Response.Cookies.Append("JWTToken", authResponse.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                }

                TempData["SuccessMessage"] = "Registration successful! Welcome!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                return View(request);
            }
        }

        [HttpGet]
        [Route("Auth/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
