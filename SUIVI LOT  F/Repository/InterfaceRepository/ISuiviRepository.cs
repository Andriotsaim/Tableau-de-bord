using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;

namespace SUIVI.Repository.InterfaceRepository
{
    public interface ISuiviRepository
    {
        Task<List<User_timer_pli>> TrackbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString);
        Task<IEnumerable<Object>> CreateFormatResult(string Enseigne_name, IEnumerable<User_timer_pli> datas, string typesearch);
        //Task<List<String>> GetLotbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString, int? enseignetype = null);
        //Task<IEnumerable<User_timerModel>> GetUserTimer(List<string> lotIds, string Name, string ConnectionString);
        //Task<IEnumerable<Modelpourleslot>> FindAllinfoByEnseigneName(List<string> lotIds, string ConnectionString, string Dbname, string Name);
        //Task<IEnumerable<ResultModel>> CreateFormatResult(string Scanner_name, string Ens_scanner, IEnumerable<Modelpourleslot> datas, IEnumerable<User_timerModel> usertimer);
    }
}