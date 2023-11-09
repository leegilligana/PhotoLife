using Photo_Life_Blazor.Models;
using Unsplasharp.Models;
namespace Photo_Life_Blazor.Services
{
    public interface IUnsplashService
    {
        Task<List<Photo>> GetRandomPhotosAsync(int num);
        Task<List<ImageMetadata>> GetImageMetadataAsync(int num);
        int GetRemainingRequests();
    }
}
