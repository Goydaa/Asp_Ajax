using Microsoft.AspNetCore.Mvc;
using MyProject.Models; // Подключаем нашу модель

namespace MyProject.Controllers
{
    public class FormController : Controller
    {
        // 1. Показ самой анкеты (GET)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // 2. Обработка нажатия кнопки "Отправить" (POST)
        [HttpPost]
        public IActionResult Index(PersonForm model)
        {
            // Проверяем, правильно ли заполнены поля (срабатывают атрибуты из Модели)
            if (ModelState.IsValid)
            {
                // Если всё ок — передаем данные на страницу успеха
                return View("Success", model);
            }
            
            // Если есть ошибки — возвращаем обратно на форму
            return View(model);
        }
    }
}