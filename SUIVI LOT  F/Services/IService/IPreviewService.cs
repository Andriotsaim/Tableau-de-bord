using SUIVI.Models.AllModels.Suivimodel;

namespace SUIVI.Services.IService
{
    public interface IPreviewService
    {
        Task<(List<PreviewModel>, List<string>)> PreviewEnseigne(IFormCollection formCollection);
    }
}
