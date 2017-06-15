using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace Emotions
{
    public class ServiceImage
    {
        public static async Task<MediaFile> TakePicture(bool UseCam = true)
        {
            await CrossMedia.Current.Initialize();

            if (UseCam)
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }
            }
            var file = UseCam ? await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Name = "Reto6_Test.jpg",
                Directory = "Championship"
            }) : await CrossMedia.Current.PickPhotoAsync();

            return file;
        }
    }
}
