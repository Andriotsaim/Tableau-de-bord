namespace SUIVI.Models.AllModels.Suivimodel;

public class PreviewModel
{
    public string Enseigne { get; set; }
    public List<string>? Listscanner { get; set; }
    public int? NbLotsglobal { get; set; }
    public int? NbLotTraite { get; set; }
    public int Lotjour { get; set; } = 0;
    public int? Nbplisglobal { get; set; } = 0;
    public int? NbPlisdeleted { get; set; } = 0;
    public int? NbLotsreco { get; set; }
    public int? NbPliTraites { get; set; }
    public int? NbPlinonTraites { get; set; }
    public int? NbPlireco { get; set; } = 0;
    public int? NbLotdeleted { get; set; } = 0;
    public int NbPliRejected { get; set; }
    public List<DetailModel> DetailModel { get; set; }
}

public class DetailModel
{
    public string? Date { get; set; }
    public string Lotid { get; set; }
    public string Nom_fichier { get; set; }
    public string? Dateimport { get; set; }
    public string? Date_scan { get; set; }
    public string? Date_saisie { get; set; }
    public string? Date_export { get; set; }
    public string? Statut { get; set; }
    public string? processedBy { get; set; }
    public string? Operatrice { get; set; }
    public string? Operatrice_initiale { get; set; }
    public int? RejectedCount { get; set; } = 0;
    public int? NbPli { get; set; } = 0;
}