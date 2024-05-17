using MySqlX.XDevAPI.Common;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;

namespace SUIVI.Services
{
    public class TransfertService : ITransfertService
    {
        private readonly IRedergroupRepository _redergroupRepository;
        private readonly ITransfertRepository _transfertRepository;


        public TransfertService(IRedergroupRepository redergroupRepository, ITransfertRepository transfertRepository)
        {
            _redergroupRepository = redergroupRepository;
            _transfertRepository = transfertRepository;
        }

        public IEnumerable<TransfertModel> TransfertEnseigne()
        {
            IEnumerable<EnseigneModel> Enseigne = _redergroupRepository.FindOnMycoreAllEnseigne();
            List<TransfertModel> concatenatedResults = new List<TransfertModel>();
            foreach (var element in Enseigne)
            {
                if (!string.IsNullOrEmpty(element.Dbname)
                            && !string.IsNullOrEmpty(element.User)
                            && !string.IsNullOrEmpty(element.Password)
                            && !string.IsNullOrEmpty(element.Domaine))
                {
                    string ConnectionString = _redergroupRepository.CreateConnectionstring(element.Domaine, element.User, element.Password, element.Dbname, element.Port);
                    try
                    {
                        var Count = _transfertRepository.GetAllNonTransferOrders(element.Name, ConnectionString);
                        var lastTransfert = _transfertRepository.GetLastTransferOrderDate(element.Name, ConnectionString);
                        concatenatedResults.Add(new TransfertModel { Enseigname = element.Name, Fackname = element.Dbname, Remaining = Count, Last_update = lastTransfert });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }

            }
            return concatenatedResults;
        }

        public async Task<(List<CMModel>, List<string>)> Historique(IFormCollection formCollection)
        {
            List<string> errorMessage = new List<string>();
            List<CMModel> concatenatedResults = new List<CMModel>();
            if (formCollection.Count() < 1)
            {
                errorMessage.Append("Le formulaire est vide. Veuillez saisir des données valides.");
                return (concatenatedResults, errorMessage);
            }
            var dateFromStr = formCollection["dateFrom"][0];
            var dateToStr = formCollection["dateTo"][0];

            DateTime dateFrom = DateTime.ParseExact(dateFromStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(dateToStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var parsedDateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
            var parsedDateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);
            if (formCollection.ContainsKey("fackname"))
            {
                var a = formCollection["fackname"][0];
                var Enseigne = _redergroupRepository.FindEnseigne(formCollection["fackname"][0]);
                if (!string.IsNullOrEmpty(Enseigne.Dbname)
                             && !string.IsNullOrEmpty(Enseigne.User)
                             && !string.IsNullOrEmpty(Enseigne.Password)
                             && !string.IsNullOrEmpty(Enseigne.Domaine))
                {
                    try {
                        string ConnectionString = _redergroupRepository.CreateConnectionstring(Enseigne.Domaine, Enseigne.User, Enseigne.Password, Enseigne.Dbname, Enseigne.Port);
                        concatenatedResults = await _transfertRepository.GetAllTransferBetweenTwoDate(Enseigne.Name, ConnectionString, parsedDateFrom, parsedDateTo);
                    }
                    catch(Exception ex) {
                        errorMessage.Add(ex.Message + ", " + ex.StackTrace);
                    }
                    
                }
            }

            return (concatenatedResults, errorMessage);
        }

    }
}
