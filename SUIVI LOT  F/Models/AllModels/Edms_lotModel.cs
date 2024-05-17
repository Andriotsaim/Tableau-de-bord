using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels
{
    public class Edms_lotModel
    {
        [Key]
        public string LotId { get; set; }
        public string? Verrou { get; set; }
        public string? User_input { get; set; }
    }
}
