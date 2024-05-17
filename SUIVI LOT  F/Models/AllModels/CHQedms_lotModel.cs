using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels
{
    public class CHQedms_lotModel
    {
        [Key]
        public string LotId { get; set; }
        public string? Statusid { get; set; }
        public string Oxifilename { get; set; }
        public string? Lockedby { get; set; }
        public string? Status { get; set; }
        public string? Inputby { get; set; }    
        public string? Lastaccessby { get; set; }
        public string? InputBy { get; set; }
        public DateTime? InputEndEDOn { get; set; }
        public string? LastaccessBy { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime Date { get; set; }

    }

}
