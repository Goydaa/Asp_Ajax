using Microsoft.AspNetCore.Mvc;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(FeedbackForm model)
        {
            if (ModelState.IsValid)
            {
                // Здесь просто возвращаем успех (в реальности можно сохранить в БД)
                return View("Success", model);
            }
            return View(model);
        }
    }
}