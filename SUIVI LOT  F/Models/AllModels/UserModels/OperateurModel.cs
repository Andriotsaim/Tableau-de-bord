using System.ComponentModel.DataAnnotations.Schema;

namespace SUIVI.Models.AllModels
{
    public class OperateurModel
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public byte[]? Password { get; set;}
        [NotMapped]
        public string? Passwordhash { get; set; }
        public string? Droits { get; set; }
        [NotMapped]
        public string? ErrorMessage { get; set; }
    }
    public enum Role
    {
        RESP,
        INPUT
    }
}
