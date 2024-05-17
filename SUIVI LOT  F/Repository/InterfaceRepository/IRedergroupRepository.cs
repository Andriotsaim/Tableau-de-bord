using SUIVI.Models.AllModels;

namespace SUIVI.Repository.InterfaceRepository
{
    public interface IRedergroupRepository
    {
        IEnumerable<EnseigneModel> FindOnMycoreAllEnseigne(bool Nameonly = false); // For the check boxes
        IEnumerable<Reder_scannerModel> FindOnMycoreRederscanner(string Name);
        EnseigneModel FindEnseigne(string dbname);
        string CreateConnectionstring(string Dbname, string User, string Password, string Domaine, int? Port);
    }
}
