using SUIVI.Models.AllModels.Suivimodel;

namespace SUIVI.Services.IService
{
    public interface ISuiviService
    {
        Task<(List<Object>, string, List<string>)> SuiviEnseigne(IFormCollection formCollection);
    }
}
