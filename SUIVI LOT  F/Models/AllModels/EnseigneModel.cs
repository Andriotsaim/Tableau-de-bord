namespace SUIVI.Models.AllModels
{
    public class EnseigneModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Domaine { get; set; }
        public string? Dbname { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public int? Port { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<System.DateTime> Update_at { get; set; }
    }
}
