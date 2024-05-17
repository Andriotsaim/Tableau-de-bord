using Google.Protobuf.WellKnownTypes;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SUIVI.Services
{
    public class SuiviService : ISuiviService
    {
        private readonly IRedergroupRepository _redergroupRepository;
        private readonly ISuiviRepository _suiviRepository;

        public SuiviService(IRedergroupRepository redergroupRepository, ISuiviRepository suiviRepository)
        {
            _redergroupRepository = redergroupRepository;
            _suiviRepository = suiviRepository;
        }

        public async Task<(List<Object>, string, List<string>)> SuiviEnseigne(IFormCollection formCollection)
        {
            List<string> errorMessage  = new List<string>();
            List<Object> concatenatedResults = new List<Object>();
            var datasoperateur = new List<User_timer_pli>();
            if (formCollection.Count() < 1)
            {
                errorMessage.Append("Le formulaire est vide. Veuillez saisir des données valides.");
                return (concatenatedResults,"",errorMessage);
            }
            var typesearch = formCollection["typesearch"][0];
            var dateFromStr = formCollection["dateFrom"][0];
            var dateToStr = formCollection["dateTo"][0];

            DateTime dateFrom = DateTime.ParseExact(dateFromStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(dateToStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var parsedDateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
            var parsedDateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);
            IEnumerable<EnseigneModel> Enseigne = Enumerable.Empty<EnseigneModel>();
            try { 
                Enseigne = _redergroupRepository.FindOnMycoreAllEnseigne();
            }
            catch(Exception ex)
            {
                errorMessage.Add(ex.Message + ", " + ex.StackTrace);
            }
            var count = 0;
            foreach (var element in Enseigne)
            {   
                count++;
                //Par Enseigne
                //if (element.Name == "ALIM" || element.Name == "FRL")
                //{
                    if (!string.IsNullOrEmpty(element.Dbname)
                            && !string.IsNullOrEmpty(element.User)
                            && !string.IsNullOrEmpty(element.Password)
                            && !string.IsNullOrEmpty(element.Domaine))
                    {
                        string ConnectionString = _redergroupRepository.CreateConnectionstring(element.Domaine, element.User, element.Password, element.Dbname, element.Port);
                        try
                        {
                            var datas = await _suiviRepository.TrackbetweenTwodate(parsedDateFrom, parsedDateTo, element.Name, ConnectionString);

                            if (typesearch == "enseigne")
                            {
                                var result = await _suiviRepository.CreateFormatResult(element.Name, datas, typesearch);
                                if (result != null && result.Any())
                                {
                                    concatenatedResults.AddRange(result);
                                }
                            }
                            else { 
                                if(datas != null && datas.Any())
                                {
                                    datasoperateur.AddRange(datas);
                                }
                                if (count == Enseigne.Count()) {  
                                    //if (element.Name == "ALIM")
                                    //{
                                        var result = await _suiviRepository.CreateFormatResult(element.Name, datasoperateur, typesearch);
                                        if (result != null && result.Any())
                                        {
                                            concatenatedResults.AddRange(result);
                                        }
                                        break;
                                    //}
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessage.Add(ex.Message + ", " + ex.StackTrace);
                        }
                    //}
                }
            }
            return (concatenatedResults, typesearch, errorMessage);
        }
    }
}
