using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SUIVI.Models.AllModels
{
    public class Edms_docModel
    {
        [Key]
        public string DocId { get; set; }
        public string LotId { get; set; }
        public string? User_input { get; set; } = "";
        public string Type { get; set; }
        public string Coderejet { get; set; }
    }
}
