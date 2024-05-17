using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SUIVI.Models.AllModels
{
    public class User_timer_pli
    {
        public int Id { get; set; }
        [NotMapped]
        public string Enseigne { get; set; }
        [NotMapped]
        public string Lotid { get; set; }
        public string Docid { get; set; }
        public string Pli { get; set; }
        public string Operateur { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Start_date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? End_date { get; set; }
        public TimeSpan Duration { get; set; }    
    }
}
