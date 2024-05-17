using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels
{
    public class Commande_mensuelModel
    {
        [Key]
        public int Id { get; set; }
        public int? Lot_jour { get; set; }
        public int? Pli_jour { get; set; }
        public int? Pli_valide { get; set; }
        public int? Pli_rejete { get; set; }
        public int? Chq_pm1_valide { get; set; }
        public int? Cba_valide { get; set; }
        public int? Nb_alpha { get; set; }
        public int? Nb_non_alpha { get; set; }
        public int? Pli_manquant { get; set; }
        public DateTime? Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Last_update { get; set; }
        public string? Scanner { get; set; }
    }
}
