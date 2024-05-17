using Microsoft.AspNetCore.Mvc;
using SUIVI.Authorization;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;

namespace SUIVI.Controllers
{
    [Authorize(Role.RESP)]
    public class TransfertController : Controller
    {
        private readonly IRedergroupRepository _redergroupRepository;
        private readonly ITransfertService _transfertService;
        public TransfertController(IRedergroupRepository redergroupRepository, ITransfertService transfertService)
        {
            _redergroupRepository = redergroupRepository;
            _transfertService = transfertService;
        }

        public IActionResult Index()
        {
            var Trasnfert = _transfertService.TransfertEnseigne();
            return View(Trasnfert);
        }

        [HttpPost]
        public async Task<IActionResult> Historiques(IFormCollection formCollection)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { data = new List<ResultModel>(), message = "Une erreur est survenue" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            var (result, message) = await _transfertService.Historique(formCollection);
            return new JsonResult(new { data = result, message = message }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}