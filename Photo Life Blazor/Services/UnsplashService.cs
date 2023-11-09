using Photo_Life_Blazor.Models;
using System.Diagnostics;
using Unsplasharp;
using Unsplasharp.Models;

namespace Photo_Life_Blazor.Services
{
    public class UnsplashService : IUnsplashService
    {
        public async Task<List<Photo>> GetRandomPhotosAsync(int num)
        {
            Debug.WriteLine(Constants.UnsplashAPIKey);
            UnsplasharpClient client = new UnsplasharpClient(Constants.UnsplashAPIKey);
            var photosfound = await client.GetRandomPhoto(count: num);
            return photosfound;
        }
        public async Task<List<ImageMetadata>> GetImageMetadataAsync(int num)
        {
            List<ImageMetadata> metadata = new List<ImageMetadata>();
            var photosfound = await GetRandomPhotosAsync(num);
            foreach (var photo in photosfound)
            {
                metadata.Add(new ImageMetadata(photo));
            }
            return metadata;
        }

        public int GetRemainingRequests()
        {
            UnsplasharpClient client = new UnsplasharpClient(Constants.UnsplashAPIKey);
            return client.RateLimitRemaining;
        }
    }
}
