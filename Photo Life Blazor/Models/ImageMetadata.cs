using Unsplasharp.Models;
namespace Photo_Life_Blazor.Models
{
    public class ImageMetadata
    {
        public Location ImageLocationDesc { get; set; }
        public Position ImagePosition { get; set; }
        public Exif ImageExif { get; set; }
        public ImageMetadata(Photo photo) { 
            if (photo != null & photo.Exif != null & photo.Location != null & photo.Location.Position != null) {
                ImagePosition = photo.Location.Position;
                ImageExif = photo.Exif;
                ImageLocationDesc = photo.Location;
            }
        }
    }
}
