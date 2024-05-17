using SUIVI.Models.AllModels.Suivimodel;

namespace SUIVI.Repository.InterfaceRepository
{
    public interface IPreviewRepository
    {
        Task<(List<String>, int)> GetLotbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString, int? enseignetype = null);
        int CountLotbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString, int? enseignetype = null);
        public Task<IEnumerable<Modelpourleslot>> FindAllinfoByEnseigneName(List<string> lotIds, string ConnectionString, string Enseignes);
        public Task<IEnumerable<PreviewModel>> CreateFormatResult(List<string> ScannerList, string Enseignes, IEnumerable<Modelpourleslot> datas, int CountLotjour);
    }
}
