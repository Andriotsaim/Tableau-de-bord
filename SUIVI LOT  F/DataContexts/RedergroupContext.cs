using Microsoft.EntityFrameworkCore;
using SUIVI.Models.AllModels;
using SUIVI.Models.AllModels.SuiviModel;

namespace SUIVI.DataContexts
{
    public class RedergroupContext : DbContext
    {
        #nullable disable
        public RedergroupContext(DbContextOptions<RedergroupContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// Create multiples connections databases
        /// No need scafolding
        /// </summary>
        /// <returns></returns>
        public static RedergroupContext RedergroupeContextConnectionString(string dbconnectionString, string connectionString = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(dbconnectionString))
                {
                    var optionsBuilder = new DbContextOptionsBuilder<RedergroupContext>();
                    optionsBuilder.UseMySQL(dbconnectionString);
                    var context = new RedergroupContext(optionsBuilder.Options);
                    return context;
                }
                else
                {
                    throw new ArgumentNullException("Missing connectionString");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur Server");
            }
        }

        public DbSet<OperateurModel> Operateur { get; set; }
        public DbSet<Edms_docModel> Edms_doc { get; set; }
        public DbSet<Edms_lotModel> Edms_lot { get; set; }
        public DbSet<DateImportModel> DateImport { get; set; }
        public DbSet<User_timerModel> User_timer { get; set; }
        public DbSet<PaiementModel> Paiement { get; set; }
        public DbSet<CMModel> Commande_mensuel { get; set; }
        public DbSet<User_timer_pli> User_timer_pli { get; set; }

    }
}
