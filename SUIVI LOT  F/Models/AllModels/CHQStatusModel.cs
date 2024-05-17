using System.ComponentModel.DataAnnotations;

namespace SUIVI.Models.AllModels
{
    public class CHQStatusModel
    {
        [Key]
        public string StatusId { get; set; }
        public string Name { get; set; }

    }
}
