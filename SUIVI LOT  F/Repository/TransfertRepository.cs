using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using SUIVI.DataContexts;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.Suivimodel;
using SUIVI.Models.AllModels.SuiviModel;
using SUIVI.Repository.InterfaceRepository;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace SUIVI.Repository
{
    public class TransfertRepository : ITransfertRepository
    {

        public int GetAllNonTransferOrders(string Enseignes, string ConnectionString)
        {
            if (Enseignes.Contains("CHQ"))
            {
                using (var contextCHQ = FRLchequeContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    var result =
                        from edmsLot in contextCHQ.Edms_lot
                        join status in contextCHQ.Status on edmsLot.Statusid equals status.StatusId
                        where status.Name == "SAI" || status.Name == "VER"
                        select new TransfertModel()
                        {
                            Commande = contextCHQ.Edms_doc.Count(x => (x.LotId == edmsLot.LotId && string.IsNullOrEmpty(x.RejectCode))),
                        };
                    return result.Sum(x=> x.Commande);
                };
            }
            else
            {
                using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
                {
                    var result =
                    from dateImport in context.DateImport
                    join paiement in context.Paiement on ((Enseignes.Contains("LAD") || Enseignes.Contains("TDP")) ? dateImport.Lotid : dateImport.EnseigneLotId) equals paiement.LotId
                    where dateImport.Statut == "SAISI" && (string.IsNullOrEmpty(paiement.Statut) || paiement.Statut == "TRT" || paiement.Statut == "VLD")
                    select new TransfertModel()
                    {
                        Enseigname = Enseignes,
                    };
                    return result.Count();
                } 
            }
            return 0;
        }

        public DateTime GetLastTransferOrderDate(string Enseignes, string ConnectionString)
        {
            //lfg_papier, rvi_papier, vlm_papier, trad_papier TRAD  tsisy
            using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
            {
                var lastTransferOrder = context.Commande_mensuel.OrderByDescending(c => c.Last_update).FirstOrDefault();
                if (lastTransferOrder == null)
                {
                    return new DateTime();
                }
                return lastTransferOrder!.Last_update;
            }
            return new DateTime();
        }

        public async Task<List<CMModel>> GetAllTransferBetweenTwoDate(string Enseignes, string ConnectionString, DateTime Debut, DateTime Fin)
        {
            using (var context = RedergroupContext.RedergroupeContextConnectionString(ConnectionString))
            {
                var lastTransferOrder = await context.Commande_mensuel
                                        .Where(cm =>
                                            (cm.Date.HasValue &&
                                            (cm.Date.Value.Year > Debut.Year || (cm.Date.Value.Year == Debut.Year && cm.Date.Value.Month > Debut.Month) || (cm.Date.Value.Date.Year == Debut.Year && cm.Date.Value.Month == Debut.Month && cm.Date.Value.Day >= Debut.Day))
                                            &&
                                            (cm.Date.Value.Year < Fin.Year || (cm.Date.Value.Year == Fin.Year && cm.Date.Value.Month < Fin.Month) || (cm.Date.Value.Year == Fin.Year && cm.Date.Value.Month == Fin.Month && cm.Date.Value.Day <= Fin.Day)))
                                        )
                                        .Select(cm => new CMModel
                                        {
                                            Date = cm.Date,
                                            Lot_jour = cm.Lot_jour,
                                            Pli_jour = cm.Pli_jour,
                                            Pli_valide = cm.Pli_valide,
                                            Pli_rejete = cm.Pli_rejete,
                                            Chq_pm1_valide = cm.Chq_pm1_valide,
                                            Cba_valide = cm.Cba_valide,
                                            Nb_alpha = cm.Nb_alpha,
                                            Nb_non_alpha = cm.Nb_non_alpha,
                                            Pli_manquant = cm.Pli_manquant,
                                            Scanner = string.IsNullOrEmpty(cm.Scanner)? string.Empty : cm.Scanner,
                                            Last_update = cm.Last_update,
                                        }).OrderByDescending(cm => cm.Last_update)
                                        .ToListAsync();
                return lastTransferOrder;
            }
            return new List<CMModel>();
        }
    }
}
