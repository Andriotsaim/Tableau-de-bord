using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels
{
    public class CHQedms_docModel
    {
        [Key]
        public string DocId { get; set; }
        public string LotId { get; set; }
        public string StatusId { get; set; }
        public string? ProcessedByname { get; set; }
        public string? VerifiedBy { get; set; }
        public string? Lastaccessby { get; set; }
        public string? RejectCode { get; set; }
        public string? Verifiedby { get; set; }
        public DateTime Date { get; set; }
        //public string? Rejectcode { get; set; }

    }
}
