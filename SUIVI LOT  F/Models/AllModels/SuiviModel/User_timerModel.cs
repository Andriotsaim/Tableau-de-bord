using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SUIVI.Models.AllModels.SuiviModel
{
    public class User_timerModel
    {
        public int Id { get; set; }
        public string LotId { get; set; }
        public string? User_input { get; set; }
        public DateTime? Date_deb { get; set; }
        public DateTime? Date_fin { get; set; }
        [NotMapped]
        public string? Date_debstring { get; set; }
        [NotMapped]
        public string? Date_finstring { get; set; }
        public TimeSpan? Total { get; set; }
        //public string? Opex { get; set; }
        public DateTime? Last_update { get; set; }
    }
}