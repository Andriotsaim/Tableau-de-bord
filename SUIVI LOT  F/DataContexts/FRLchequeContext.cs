using Microsoft.EntityFrameworkCore;
using SUIVI.Models.AllModels;

namespace SUIVI.DataContexts
{
    public class FRLchequeContext : DbContext
    {
    #nullable disable
        public FRLchequeContext(DbContextOptions<FRLchequeContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// Create multiples connections databases
        /// No need scafolding
        /// </summary>
        /// <returns></returns>
        public static FRLchequeContext RedergroupeContextConnectionString(string dbconnectionString, string connectionString = null)
        {
            if (!string.IsNullOrEmpty(dbconnectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<FRLchequeContext>();
                optionsBuilder.UseMySQL(dbconnectionString);
                //optionsBuilder.UseMySql(dbconnectionString, ServerVersion.AutoDetect(dbconnectionString), b => b.MigrationsAssembly("SUIVI"));
                var context = new FRLchequeContext(optionsBuilder.Options);
                return context;
            }
            else
            {
                throw new ArgumentNullException("Missing connectionString");
            }
        }

        public DbSet<CHQedms_docModel> Edms_doc { get; set; }
        public DbSet<CHQedms_lotModel> Edms_lot { get; set; }
        public DbSet<CHQStatusModel> Status { get; set; }

    }
}
