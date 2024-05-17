using MySqlX.XDevAPI.Common;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services.IService;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;

namespace SUIVI.Services
{
    public class PreviewService : IPreviewService
    {
        private readonly IRedergroupRepository _redergroupRepository;
        private readonly IPreviewRepository _previewRepository;
        private readonly ISuiviRepository _suiviRepository;
        public PreviewService(IRedergroupRepository redergroupRepository, IPreviewRepository previewRepository, ISuiviRepository suiviRepository)
        {
            _redergroupRepository = redergroupRepository;
            _previewRepository = previewRepository;
            _suiviRepository = suiviRepository;
        }

        public async Task<(List<PreviewModel>, List<string>)> PreviewEnseigne(IFormCollection formCollection)
        {
            List<string> errorMessage = new List<string>();
            List<PreviewModel> concatenatedResults = new List<PreviewModel>();
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

            IEnumerable<EnseigneModel> Enseigne = _redergroupRepository.FindOnMycoreAllEnseigne();

            foreach (var element in Enseigne)
            {
                //Par Enseigne
                if (formCollection.ContainsKey(element.Name) && formCollection[element.Name][0] == "true")
                {


                    if (!string.IsNullOrEmpty(element.Dbname)
                            && !string.IsNullOrEmpty(element.User)
                            && !string.IsNullOrEmpty(element.Password)
                            && !string.IsNullOrEmpty(element.Domaine))
                    {

                        string ConnectionString = _redergroupRepository.CreateConnectionstring(element.Domaine, element.User, element.Password, element.Dbname, element.Port);
                        //DateTime From, DateTime To, string ConnectionString, string Dbname, string Name
                        var scannerList = _redergroupRepository.FindOnMycoreRederscanner(element.Name);
                        //context.Reder_scanner.Where(e => e.Enseigne_name == Name).ToList();
                        if (scannerList == null || !scannerList.Any())
                        {
                            scannerList = new List<Reder_scannerModel>() { new Reder_scannerModel() { Enseigne_name = null, Scanner_name = null } };
                        }
                        List<string> scanner = new List<string>();
                        var Enseignes = string.Empty;
                        //Par scanner dans reder_scanner
                        foreach (var elementsc in scannerList)
                        {
                            //string scanner = elementsc.Scanner_name == null ? "" : "-" + elementsc.Scanner_name; //string scanner = "08,09,29";                                                                                     
                            Enseignes = element.Name;
                            scanner.Add(elementsc.Scanner_name);
                        }
                        try
                        {
                            IEnumerable<Modelpourleslot> datas = null;
                            if (Enseignes == "TDP")
                            {
                                for (var enseignetype = 1; enseignetype <= 2; enseignetype++)
                                {
                                    var lotres = await _previewRepository.GetLotbetweenTwodate(parsedDateFrom, parsedDateTo, Enseignes, ConnectionString, enseignetype);
                                    var (lotIds, CountLotjour) = lotres;
                                    if (lotIds.Count() >= 1)
                                    {
                                        {
                                            datas = await _previewRepository.FindAllinfoByEnseigneName(lotIds, ConnectionString, Enseignes);
                                            var result = datas.Any() ? await _previewRepository.CreateFormatResult(scanner, enseignetype == 1 ? "TDP" : "TDPEG", datas, CountLotjour) : null;
                                            if (result != null)
                                            {
                                                concatenatedResults.AddRange(result);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var lotres = await _previewRepository.GetLotbetweenTwodate(parsedDateFrom, parsedDateTo, Enseignes, ConnectionString);
                                var (lotIds, CountLotjour) = lotres;
                                if (lotIds.Count() >= 1)
                                {   
                                    {
                                        datas = await _previewRepository.FindAllinfoByEnseigneName(lotIds, ConnectionString, Enseignes);
                                        var result = datas.Any() ? await _previewRepository.CreateFormatResult(scanner, Enseignes, datas, CountLotjour) : null;
                                        if (result != null)
                                        {
                                            concatenatedResults.AddRange(result);
                                        }
                                    }
                                
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessage.Add(ex.Message + ", " + ex.StackTrace);
                        }
                    }
                }
            }
            return (concatenatedResults, errorMessage);
        }
    }
}
