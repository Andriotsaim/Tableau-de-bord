using System.ComponentModel.DataAnnotations.Schema;

namespace SUIVI.Models.AllModels
{
    public class Reder_scannerModel
    {
        public int Id { get; set; }
        public string? Enseigne_name { get; set; }
        public string? Scanner_name { get; set; }
        public string? Sous_filename { get; set; }
        public string? Tlmc_suffixe { get; set; }
        [NotMapped]
        public string? ErrorMessage { get; set; }

    }
}
