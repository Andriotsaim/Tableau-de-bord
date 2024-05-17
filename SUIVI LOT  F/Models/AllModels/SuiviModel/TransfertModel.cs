using System;
using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels.SuiviModel
{
    public class TransfertModel
    {
        public int Id { get; set; }
        public string? Enseigname { get; set; }
        public string? Fackname { get; set; }
        public int Commande { get; set; } = 0;
        public int Remaining { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Last_update { get; set; }
    }
}
