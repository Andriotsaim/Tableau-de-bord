using SUIVI.Models.AllModels.SuiviModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SUIVI.Models.AllModels.Suivimodel
{
    public class ResultModel
    {
        public string Enseignname { get; set; }
        public string? DateSaisie { get; set; }
        public int? CountLotglobal { get; set; }
        public int? Countplisglobal { get; set; }
        public int? CountPliRejeteglobal { get; set; }
        public int? Countplitraitesglobal { get; set; }
        public int? Countplirestantglobal { get; set; }
        public int? CountDocdistglobal { get; set; }
        public int? CountDocdisttraiteglobal { get; set; }
        public int? CountDocdistrejeteglobal { get; set; }
        public int? CountDocdistrestantglobal { get; set; }
        public IEnumerable<Object>? Usertimer { get; set; }
        public TimeSpan? TotalTimespan { get; set; }
        public int? Export { get; set; }
        public string? Date { get; set; }
        public string? Dateimport { get; set; }
        public string? Date_scan { get; set; }
        public string? Date_export { get; set; }
        public string Lotid { get; set; }
        public string Nom_fichier { get; set; }
        public int? No_pli { get; set; }
        public string? Impstatut { get; set; }
        public int? RejectedCount { get; set; }
        public int? Nbplis { get; set; }
        public int? nblot { get; set; }
        public string? Opr { get; set; }
        public string? Operatrice { get; set; }
        public string? Operatrice_initiale { get; set; }
        public List<DetailModel>? Details { get; set; }
        [NotMapped]
        public string? ErrorMessage { get; set; }
    }
}