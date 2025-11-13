using DMnDBCS.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class AuthController(HttpClient httpClient, IConfiguration configuration) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;

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

            var apiBaseUrl = _configuration["UriData:ApiUri"];
            var registerRequest = new { request.Name, request.Email, request.Password };
            var response = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}auth/register", registerRequest);

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
