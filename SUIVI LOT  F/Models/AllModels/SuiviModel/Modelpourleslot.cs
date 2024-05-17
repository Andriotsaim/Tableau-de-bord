using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels.Suivimodel
{
    public class Modelpourleslot
    {
        public string Enseigne { get; set; }
        public List<string>? Listscanner { get; set; }
        public string Lotid { get; set; }
        public string? Docid { get; set; }
        public string Nom_fichier { get; set; }
        public int? No_pli { get; set; } = 0;
        public string? Date { get; set; }
        public string? Date_saisie { get; set; }
        public string? Dateimport { get; set; }
        public string? Date_scan { get; set; }
        public string? Date_export { get; set; }
        public string? Operatrice { get; set; }
        public string? Operatrice_initiale { get; set; }
        public string? User_input { get; set; }
        public string? Type { get; set; }
        public string? Statut { get; set; }
        public string? Impstatut { get; set; }
        public string? Verrou { get; set; }
        public string? Opr { get; set; }
        public int RejectedCount { get; set; } = 0;
        public int? Rejectcode { get; set; }
        public List<string> ModePaiement { get; set; }
        public string? Paiement { get; set; }
        public TimeSpan? UserTimer { get; set; }
    }  
}
