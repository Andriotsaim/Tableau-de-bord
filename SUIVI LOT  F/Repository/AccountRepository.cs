using SUIVI.DataContexts;
using SUIVI.Helpers;
using SUIVI.Models.AllModels;
using SUIVI.Repository.InterfaceRepository;
using System.Text;

namespace SUIVI.Repository
{

    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        public AccountRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public OperateurModel FindOperateur(string Login)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString(SD.Reder);
                var context = RedergroupContext.RedergroupeContextConnectionString(connectionString);
                var Operateur = context.Operateur.FirstOrDefault(o => o.Login == Login);

                if (Operateur != null)
                {
                    Operateur.Passwordhash = Encoding.UTF8.GetString(Operateur.Password);
                    return Operateur;
                }
                else
                {
                    return new OperateurModel { ErrorMessage = "Erreur de données d'identification" };
                }
            }
            catch (Exception ex) {
                return new OperateurModel { ErrorMessage = "Une erreur s'est produite lors de la tentative de connexion." };
            }
           
           
        }
    }
}
