using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;

namespace SUIVI.Services.IService
{
    public interface ITransfertService
    {
        IEnumerable<TransfertModel> TransfertEnseigne();
        Task<(List<CMModel>, List<string>)> Historique(IFormCollection formCollection);
    }
}
