using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;

namespace SUIVI.Repository.InterfaceRepository
{
    public interface ITransfertRepository
    {
        public int GetAllNonTransferOrders(string Enseignes, string ConnectionString);
        public DateTime GetLastTransferOrderDate(string Enseignes, string ConnectionString);
        public Task<List<CMModel>> GetAllTransferBetweenTwoDate(string Enseignes, string ConnectionString, DateTime Debut, DateTime Fin);

    }
}
