using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SUIVI.Helpers;
using SUIVI.Models;
using SUIVI.Models.AllModels.UserModels;
using SUIVI.Repository;
using SUIVI.Repository.InterfaceRepository;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace SUIVI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Preview");
            }
            return View();
        }

        [TempData]
        public string? ErrorMessage { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexAsync(LoginModel model)
        {
            var operateur = _accountRepository.FindOperateur(model.Login);
            string inputpasswordhash = SD.EncryptString(model.Password);
            if (ModelState.IsValid)
            {
                if (operateur.ErrorMessage == null)
                {
                    if (inputpasswordhash == operateur.Passwordhash)
                    {
                        var claim = new List<Claim> {
                            new Claim("Username", operateur.Login),
                            new Claim("role", operateur.Droits),
                            new Claim(ClaimTypes.Role, Convert.ToString(operateur.Droits))
                        };
                        await HttpContext.SignInAsync(
                                new ClaimsPrincipal(new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme, "Username","role")),
                                new AuthenticationProperties {
                                    IsPersistent = true,
                                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                                }
                            );
                        return RedirectToAction("Index", "Preview");
                    }
                    else
                    {
                        ErrorMessage = "Utilisateur ou mot de passe incorrect";
                        return RedirectToAction("Index");
                    }
                } 
                else 
                {
                    ErrorMessage = operateur.ErrorMessage;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir les champs correctement.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}