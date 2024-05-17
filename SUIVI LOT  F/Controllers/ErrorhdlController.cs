using Microsoft.AspNetCore.Mvc;

namespace SUIVI.Controllers
{
    public class ErrorhdlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return RedirectToAction("Index", "Account");
        }
    }
}
