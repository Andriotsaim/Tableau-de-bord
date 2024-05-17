using Microsoft.AspNetCore.Mvc;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;
using SUIVI.Authorization;

namespace SUIVI.Controllers
{
    [Authorize(Role.RESP)]
    public class PerformanceController : Controller
    {
        private readonly ISuiviService _suiviService;
        public PerformanceController(ISuiviService suiviService)
        {
            _suiviService = suiviService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Suivi(IFormCollection formCollection)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { data = new List<Object>(), message = "Une erreur est survenue" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            var (result, typesearch, exception) = await _suiviService.SuiviEnseigne(formCollection);
            return new JsonResult(new { data = result, typesearch = typesearch, message = exception }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
