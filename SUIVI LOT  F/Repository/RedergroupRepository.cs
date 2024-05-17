using Microsoft.Extensions.Configuration;
using SUIVI.DataContexts;
using SUIVI.Helpers;
using SUIVI.Models.AllModels;
using SUIVI.Repository.InterfaceRepository;

namespace SUIVI.Repository
{
    public class RedergroupRepository : IRedergroupRepository
    {
        private readonly IConfiguration _configuration;
        public RedergroupRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<EnseigneModel> FindOnMycoreAllEnseigne(bool NameOnly = false)
        {
            string connectionString = _configuration.GetConnectionString(SD.Mycore);
            using (var context = MycoreContext.MycoreContextCreateConnectionString(connectionString)) {
                var result = context.Enseignes
                    .Select(e => new EnseigneModel
                    {
                        Dbname = NameOnly ? "" : e.Dbname,
                        Domaine = NameOnly ? "" : e.Domaine,
                        Name = e.Name,
                        Password = NameOnly ? "" : e.Password,
                        User = NameOnly ? "" : e.User,
                        Port = NameOnly ? 80 : e.Port,
                    })
                    .ToList();
                return result;
            }     
        }

        public IEnumerable<Reder_scannerModel> FindOnMycoreRederscanner(string Name)
        {
            string connectionString = _configuration.GetConnectionString(SD.Mycore);
            using (var context = MycoreContext.MycoreContextCreateConnectionString(connectionString))
            {
                var result = context.Reder_scanner
                    .Where(e => e.Enseigne_name == Name).ToList()
                    .Select(r => new Reder_scannerModel
                    {
                        Enseigne_name = r.Enseigne_name,
                        Scanner_name = r.Scanner_name,
                    })
                    .ToList();
                return result;
            }
           
        }

        public EnseigneModel FindEnseigne(string dbname)
        {
            string connectionString = _configuration.GetConnectionString(SD.Mycore);
            using (var context = MycoreContext.MycoreContextCreateConnectionString(connectionString))
            {
                var result = context.Enseignes.Where(e => e.Dbname == dbname).FirstOrDefault();
                return result;
            }
        }

        public string CreateConnectionstring(string Domaine, string User, string Password, string Dbname, int? Port)
        {
            string connectionString = $"server={Domaine},{Port}; uid={User}; pwd={SD.DecryptString(Password)}; database={Dbname}";
            return connectionString;
        }
    }
}
