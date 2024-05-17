using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels
{
    public class DateImportModel
    {
        public int Id { get; set; }
        public string? EnseigneLotId { get; set; }
        public string? Lotid { get; set; }
        public string Nom_fichier { get; set; } = "";
        public int? No_pli { get; set; }
        public string? Operatrice { get; set; }
        public string? Operatrice_initiale { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Date_saisie { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Dateimport { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Date_scan { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Date_export { get; set; }
        public string? Statut { get; set; }
        public int? EnseignetypeId { get; set; } = null;
    }
}
