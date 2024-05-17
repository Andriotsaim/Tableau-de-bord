using Microsoft.EntityFrameworkCore;
using SUIVI.Models.AllModels;

namespace SUIVI.DataContexts
{
    public class MycoreContext: DbContext
    {
        #nullable disable
        public MycoreContext(DbContextOptions<MycoreContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// Create multiples connections databases
        /// No need scafolding
        /// </summary>
        /// <returns></returns>
        public static MycoreContext MycoreContextCreateConnectionString(string dbconnectionString, string connectionString = null)
        {
            try { 
                if (!string.IsNullOrEmpty(dbconnectionString))
                {
                    var optionsBuilder = new DbContextOptionsBuilder<MycoreContext>();
                    optionsBuilder.UseMySQL(dbconnectionString);
                    //optionsBuilder.UseMySql(dbconnectionString, ServerVersion.AutoDetect(dbconnectionString), b => b.MigrationsAssembly("SUIVI"));
                    var context = new MycoreContext(optionsBuilder.Options);
                    return context;
                }
                else
                {
                    throw new ArgumentNullException("Missing connectionString");
                }
            }catch (Exception ex)
            {
                throw new Exception("Connection to mycore failed");
            }
        }

        public DbSet<EnseigneModel> Enseignes { get; set; } //Mycore
        public DbSet<Reder_scannerModel> Reder_scanner { get; set; } //Mycore

    }
}
