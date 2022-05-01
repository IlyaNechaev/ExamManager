using ExamManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamManager.Controllers
{
    public class PagesController : Controller
    {
        [HttpGet(Routes.HomePage)]
        public IActionResult Index()
        {
            return View("Home");
        }
    }
}
