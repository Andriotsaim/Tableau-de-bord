using Microsoft.AspNetCore.Mvc;
using SUIVI.Authorization;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Repository;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;

namespace SUIVI.Controllers
{
    [Authorize(Role.RESP)]
    public class PreviewController: Controller
    {
        private readonly IRedergroupRepository _redergroupRepository;
        private readonly IPreviewService _PreviewService;
        public PreviewController(IRedergroupRepository RedergroupRepository, IPreviewService PreviewService)
        {
            _redergroupRepository = RedergroupRepository;
            _PreviewService = PreviewService;
        }
        public IActionResult Index()
        {
            IEnumerable<EnseigneModel> Enseigne = _redergroupRepository.FindOnMycoreAllEnseigne(true);
            return View(Enseigne);
        }

        [HttpPost]
        public async Task<IActionResult> PreviewsAsync(IFormCollection formCollection)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { data = new List<PreviewModel>(), message = "Une erreur est survenue" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            var (result, exception) = await _PreviewService.PreviewEnseigne(formCollection);
            return new JsonResult(new { data = result, message = exception }) { StatusCode = StatusCodes.Status200OK };
        }

    }
}
