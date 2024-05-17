using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using SUIVI.DataContexts;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Repository.InterfaceRepository;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace SUIVI.Repository
{
    public class PreviewRepository: IPreviewRepository
    {
        public async Task<(List<String>, int)> GetLotbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString, int? enseignetype = null)
        {
            var currentdate = DateTime.Now;
            if (Name.Contains("CHQ"))
            {
                //Frl CHQ
                using (var contextCHQ = FRLchequeContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    var result = await contextCHQ.Edms_lot
                    .Where(edmslot =>
                        (edmslot.Date.Year > Debut.Year || (edmslot.Date.Year == Debut.Year && edmslot.Date.Month > Debut.Month) || (edmslot.Date.Year == Debut.Year && edmslot.Date.Month == Debut.Month && edmslot.Date.Day >= Debut.Day))
                        &&
                        (edmslot.Date.Year < Fin.Year || (edmslot.Date.Year == Fin.Year && edmslot.Date.Month < Fin.Month) || (edmslot.Date.Year == Fin.Year && edmslot.Date.Month == Fin.Month && edmslot.Date.Day <= Fin.Day))
                    )
                    .OrderBy(edmslot => edmslot.Date)
                    .Select(edmslot => edmslot.LotId)
                    .ToListAsync();
                    return (result, CountLotbetweenTwodate(Debut, Fin, Name, ConnectionString, enseignetype));
                };
            }
            else
            {
                //Other Enseigne
                using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    var result = await context.DateImport
                    .Where(imp =>
                         (
                         (enseignetype != null ?
                         (
                         (imp.Date_saisie.HasValue ?
                         imp.Date_saisie.Value.Year >= Debut.Year &&
                         imp.Date_saisie.Value.Month >= Debut.Month &&
                         imp.Date_saisie.Value.Day >= Debut.Day &&
                         imp.Date_saisie.Value.Year <= Fin.Year &&
                         imp.Date_saisie.Value.Month <= Fin.Month &&
                         imp.Date_saisie.Value.Day <= Fin.Day
                         :
                         ((imp.Statut != "IMP" && imp.Statut != "RECO") && imp.Dateimport.HasValue &&
                         imp.Dateimport.Value.Year >= Debut.Year &&
                         imp.Dateimport.Value.Month >= Debut.Month &&
                         imp.Dateimport.Value.Day >= Debut.Day &&
                         imp.Dateimport.Value.Year <= Fin.Year &&
                         imp.Dateimport.Value.Month <= Fin.Month &&
                         imp.Dateimport.Value.Day <= Fin.Day
                         ))
                         && imp.EnseignetypeId == enseignetype || (imp.EnseignetypeId == enseignetype && (imp.Statut == "IMP" || imp.Statut == "RECO") && (currentdate.Year == Fin.Year && currentdate.Month == Fin.Month && currentdate.Day == Fin.Day))
                         )
                         :
                         (
                         imp.Date_saisie.HasValue ?
                         imp.Date_saisie.Value.Year >= Debut.Year &&
                         imp.Date_saisie.Value.Month >= Debut.Month &&
                         imp.Date_saisie.Value.Day >= Debut.Day &&
                         imp.Date_saisie.Value.Year <= Fin.Year &&
                         imp.Date_saisie.Value.Month <= Fin.Month &&
                         imp.Date_saisie.Value.Day <= Fin.Day
                         :
                         ((imp.Statut != "IMP" && imp.Statut != "RECO") && imp.Dateimport.HasValue &&
                         imp.Dateimport.Value.Year >= Debut.Year &&
                         imp.Dateimport.Value.Month >= Debut.Month &&
                         imp.Dateimport.Value.Day >= Debut.Day &&
                         imp.Dateimport.Value.Year <= Fin.Year &&
                         imp.Dateimport.Value.Month <= Fin.Month &&
                         imp.Dateimport.Value.Day <= Fin.Day)
                         || ((imp.Statut == "IMP" || imp.Statut == "RECO") && (currentdate.Year == Fin.Year && currentdate.Month == Fin.Month && currentdate.Day == Fin.Day))
                         ))
                         )
                    )
                    .Select(imp => (Name.Contains("LAD") || Name.Contains("TDP") ? imp.Lotid : imp.EnseigneLotId))
                    .ToListAsync();
                    return (result, CountLotbetweenTwodate(Debut, Fin, Name, ConnectionString, enseignetype));
                };
            }

        }

        public int CountLotbetweenTwodate(DateTime Debut, DateTime Fin, string Name, string ConnectionString, int? enseignetype = null)
        {
            if (Name.Contains("CHQ"))
            {
                //Frl CHQ
                using (var contextCHQ = FRLchequeContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    return contextCHQ.Edms_lot
                    .Where(edmslot =>
                        (edmslot.Date.Year > Debut.Year || (edmslot.Date.Year == Debut.Year && edmslot.Date.Month > Debut.Month) || (edmslot.Date.Year == Debut.Year && edmslot.Date.Month == Debut.Month && edmslot.Date.Day >= Debut.Day))
                        &&
                        (edmslot.Date.Year < Fin.Year || (edmslot.Date.Year == Fin.Year && edmslot.Date.Month < Fin.Month) || (edmslot.Date.Year == Fin.Year && edmslot.Date.Month == Fin.Month && edmslot.Date.Day <= Fin.Day))
                    )
                    .Count();
                };
            }
            else
            {
                //Other Enseigne
                using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    return context.DateImport
                    .Where(imp =>
                         (
                         (enseignetype != null ?
                         (
                         (imp.Dateimport.HasValue &&
                         imp.Dateimport.Value.Year >= Debut.Year &&
                         imp.Dateimport.Value.Month >= Debut.Month &&
                         imp.Dateimport.Value.Day >= Debut.Day &&
                         imp.Dateimport.Value.Year <= Fin.Year &&
                         imp.Dateimport.Value.Month <= Fin.Month &&
                         imp.Dateimport.Value.Day <= Fin.Day
                         )
                         && imp.EnseignetypeId == enseignetype)
                         :
                         (
                         imp.Dateimport.HasValue &&
                         imp.Dateimport.Value.Year >= Debut.Year &&
                         imp.Dateimport.Value.Month >= Debut.Month &&
                         imp.Dateimport.Value.Day >= Debut.Day &&
                         imp.Dateimport.Value.Year <= Fin.Year &&
                         imp.Dateimport.Value.Month <= Fin.Month &&
                         imp.Dateimport.Value.Day <= Fin.Day)
                         )
                    )).Count();
                    
                };
            }

            return 0;
        }

        public async Task<IEnumerable<Modelpourleslot>> FindAllinfoByEnseigneName(List<string> lotIds, string ConnectionString, string Enseignes)
        {

            if (Enseignes.Contains("CHQ"))
            {
                using (var contextCHQ = FRLchequeContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    return await (
                        from edmsLot in contextCHQ.Edms_lot
                        //join edmsDoc in contextCHQ.Edms_doc on edmsLot.LotId equals edmsDoc.LotId
                        join status in contextCHQ.Status on edmsLot.Statusid equals status.StatusId
                        where lotIds.Contains(edmsLot.LotId)
                        //group new { edmsLot, edmsDoc, status } by edmsLot.LotId into grouped
                        select new Modelpourleslot()
                        {
                            Enseigne = Enseignes,
                            Date = edmsLot.Date.ToString("dd-MM-yyyy"),
                            Dateimport = edmsLot.Date.ToString("dd-MM-yyyy HH:mm"),
                            Date_saisie = edmsLot.InputEndEDOn.HasValue ? edmsLot.InputEndEDOn.Value.ToString("dd-MM-yyyy HH:mm") : null,
                            Date_scan = edmsLot.InputEndEDOn.HasValue ? edmsLot.InputEndEDOn.Value.ToString("dd-MM-yyyy HH:mm") : null,
                            Date_export = edmsLot.ExportDate.HasValue ? edmsLot.ExportDate.Value.ToString("dd-MM-yyyy HH:mm") : null,
                            Lotid = edmsLot.LotId,
                            Nom_fichier = edmsLot.Oxifilename.Replace(".oxi", string.Empty),
                            No_pli = contextCHQ.Edms_doc.Count(doc => (doc.LotId == edmsLot.LotId)),
                            Statut = status.Name,
                            Impstatut = edmsLot.Lockedby != null ? "EN COURS" :
                                        status.Name == "SAI" ? "SAISI":
                                        status.Name == "VER" ? "VERIFIE":
                                        status.Name == "EXP" ? "EXPORTE":
                                        edmsLot.Lastaccessby != null ? "Dernier accès": status.Name,
                            RejectedCount = contextCHQ.Edms_doc.Count(doc => (doc.LotId == edmsLot.LotId && !string.IsNullOrEmpty(doc.RejectCode))),
                            Opr = edmsLot.Lastaccessby,
                            Operatrice_initiale = !string.IsNullOrEmpty(edmsLot.Inputby) ? edmsLot.Inputby: null,
                            Operatrice = !string.IsNullOrEmpty(edmsLot.Inputby) ? edmsLot.Lockedby : null,
                            //UserTimer = TimeSpan.Zero,
                        }).ToListAsync();
                };
            }
            else
            {
                //Other Enseignes
                using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    return await (
                        from edmsLot in context.Edms_lot
                        join dateImport in context.DateImport on edmsLot.LotId equals (Enseignes.Contains("LAD") || Enseignes.Contains("TDP") ? dateImport.Lotid : dateImport.EnseigneLotId)
                        //join userTimer in context.User_timer on edmsLot.LotId equals userTimer.LotId into userTimerGroup
                        //from userTimer in userTimerGroup.DefaultIfEmpty()
                        where lotIds.Contains(edmsLot.LotId)
                        //where dateImport.Statut != "RECO" && dateImport.Statut != "SUPPRIMER"
                        select new Modelpourleslot()
                        {
                            Enseigne = Enseignes,
                            Date = dateImport.Dateimport == null ? "" : dateImport.Dateimport.Value.ToString("dd-MM-yyyy"),
                            Dateimport = dateImport.Dateimport == null ? "" : dateImport.Dateimport.Value.ToString("dd-MM-yyyy HH:mm"),
                            Date_saisie = dateImport.Date_saisie == null ? "" : dateImport.Date_saisie.Value.ToString("dd-MM-yyyy HH:mm:s"),
                            Date_scan = dateImport.Date_scan == null ? "" : dateImport.Date_scan.Value.ToString("dd-MM-yyyy HH:mm:ss"),
                            Date_export = dateImport.Date_export == null ? "" : dateImport.Date_export.Value.ToString("dd-MM-yyyy HH:mm:ss"),
                            Lotid = edmsLot.LotId,
                            Nom_fichier = dateImport.Nom_fichier.Replace(".oxi", string.Empty),
                            // No_pli = Enseignes == "VLM" ? dateImport.No_pli - 1 : dateImport.No_pli ,
                            No_pli = context.Edms_doc.Count(doc => (doc.LotId == edmsLot.LotId && doc.Type == "BDS")),
                            RejectedCount = context.Edms_doc.Count(doc => (doc.LotId == edmsLot.LotId && (dateImport.Statut == "SAISI" || dateImport.Statut == "EXPORTE" || string.IsNullOrEmpty(dateImport.Statut)) && !string.IsNullOrEmpty(doc.Coderejet) && doc.Type == "BDS")),
                            //ModePaiement = context.Paiement.Where(p => p.LotId == edmsLot.LotId ).Select(p => p.ModPai).ToList(),
                            ModePaiement = new List<string>(),
                            Statut = dateImport.Statut,
                            Impstatut = edmsLot.User_input != null ? "EN COURS":
                                     (edmsLot.Verrou != "" && dateImport.Statut == "SAISI") ? "SAISI" :
                                     (edmsLot.Verrou != "" && dateImport.Statut == "EXPORTE") ? "EXPORTE" :
                                     (edmsLot.User_input == null && edmsLot.Verrou == "" && dateImport.Operatrice_initiale != null && dateImport.Statut != "SAISI") ? "Dernier accès": dateImport.Statut,
                            Opr = edmsLot.User_input != null ? edmsLot.User_input :
                                          (edmsLot.Verrou != "") ? edmsLot.Verrou :
                                          (edmsLot.Verrou == "" && edmsLot.User_input == null /*&& userTimer.User_input != null*/) ? dateImport.Operatrice_initiale : null,
                            Operatrice = dateImport.Operatrice,
                            Operatrice_initiale = dateImport.Operatrice_initiale,
                            //UserTimer = userTimer.Total == null ? TimeSpan.Zero : userTimer.Total.Value,
                            //UserTimer = userTimer.Date_fin != null && userTimer.Date_deb != null ? userTimer.Date_fin.Value.Subtract(userTimer.Date_deb.Value): TimeSpan.Zero,
                        }
                    ).ToListAsync();
                };
            }
        }
        public async Task<IEnumerable<PreviewModel>> CreateFormatResult(List<string> Listscanner, string Enseignes, IEnumerable<Modelpourleslot> datas, int LotjourCount)
        {
            var result = datas
                .GroupBy(x => new { x.Enseigne })
                .Select(group => new PreviewModel()
                {
                    //Globale
                    //Date = group.Select(x => x.Dateimport).First(),
                    Enseigne = Enseignes,
                    NbLotsglobal = group.Count(),
                    Lotjour = LotjourCount,
                    NbLotTraite = group.Count(x => (x.Statut == "EXPORTE" || x.Statut == "SAISI" || x.Statut == "SAI" || x.Statut == "EXP")),
                    NbLotdeleted = group.Count(x => x.Statut == "SUPPRIMER"),
                    NbPlisdeleted = group.Where(x => x.Statut == "SUPPRIMER").Select(x => x.No_pli).Sum(),
                    Nbplisglobal = (Enseignes.Contains("CHQ")) ? group.Select(x => x.No_pli).FirstOrDefault() : group.Select(x => x.No_pli).Sum(),
                    NbLotsreco = group.Count(x => x.Statut == "RECO"),
                    //NbPlireco = group.Where(x => x.Statut == "RECO").Select(x => x.No_pli).Sum(),
                    NbPlinonTraites = (Enseignes.Contains("CHQ")) ? group.Where(x => (x.Statut != "SAI" && x.Statut != "EXP" && x.Statut != "VER") ).Select(x => x.No_pli).FirstOrDefault() ?? 0 : group.Where(x => ( x.Statut == "IMP" || x.Statut == "RECO" || string.IsNullOrEmpty(x.Statut) )).Select(x => x.No_pli).Sum(),
                    NbPliRejected = /*(Enseignes.Contains("CHQ")) ? group.Count(x => x.Rejectcode != null) :*/ group.Select(x => x.RejectedCount).Sum(),
                    Listscanner = Listscanner,
                    DetailModel = group
                    .Select(item => new DetailModel()
                    {
                        Date = item.Date,
                        Lotid = item.Lotid,
                        Nom_fichier = item.Nom_fichier,
                        Dateimport = item.Dateimport,
                        Date_scan = item.Date_scan,
                        Date_saisie = item.Date_saisie,
                        Date_export = item.Date_export,
                        Statut = item.Impstatut,
                        processedBy = item.Opr,
                        Operatrice = item.Operatrice,
                        Operatrice_initiale = item.Operatrice_initiale,
                        RejectedCount = item.RejectedCount,
                        NbPli = item.No_pli,
                    }).ToList(),
                })
                .ToList();
            return await Task.FromResult(result);
        }

    }
}
