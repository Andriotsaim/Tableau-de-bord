using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels.UserModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Le champ vide est vide.")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Le champ vide est vide.")]
        public string? Password { get; set; }
    }
}
