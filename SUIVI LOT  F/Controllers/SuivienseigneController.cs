using Microsoft.AspNetCore.Mvc;
using SUIVI.Authorization;
using SUIVI.Helpers;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Repository;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;
using System.Collections;
using System.Linq;
using System.Text.Json.Serialization;

namespace SUIVI.Controllers
{
    [Authorize(Role.RESP)]
    public class SuivienseigneController : Controller
    {
        private readonly IRedergroupRepository _redergroupRepository;
        private readonly ISuiviService _suiviService;
        public SuivienseigneController(IRedergroupRepository RedergroupRepository, ISuiviService suiviService)
        {
            _redergroupRepository = RedergroupRepository;
            _suiviService = suiviService;
        }
        public IActionResult Index()
        {
            IEnumerable<EnseigneModel> Enseigne = _redergroupRepository.FindOnMycoreAllEnseigne();
            return View(Enseigne);
        }

        [HttpPost]
        public async Task<IActionResult> Suivi(IFormCollection formCollection) 
        {
            //if (!ModelState.IsValid)
            //{
            //    return new JsonResult(new { data = new List<ResultModel>() , message = "Une erreur est survenue" }) { StatusCode = StatusCodes.Status400BadRequest };
            //}
            //var (result, exception) = await _suiviService.SuiviEnseigne(formCollection);
            //return new JsonResult(new { data = result, message = exception }) { StatusCode = StatusCodes.Status200OK };
            return Ok();
        }  
    }
}
