using SUIVI.Models.AllModels;

namespace SUIVI.Repository.InterfaceRepository
{
    public interface IAccountRepository
    {
        OperateurModel FindOperateur(string Login); // Login
    }
}
