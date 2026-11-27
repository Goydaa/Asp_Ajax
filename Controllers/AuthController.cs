using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyProject.Controllers
{
    public class AuthController : Controller
    {
        // 1. Страница входа (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Обработка формы входа (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Простейшая проверка: логин "admin", пароль "123"
            if (username == "admin" && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin") // Даем роль админа
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Создаем куки и сохраняем в браузере
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Profile"); // Перекидываем в профиль
            }

            ViewBag.Error = "Неверный логин или пароль";
            return View();
        }

        // 3. Защищенная страница (доступна только после входа)
        [Authorize] 
        public IActionResult Profile()
        {
            return View();
        }

        // 4. Выход
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}