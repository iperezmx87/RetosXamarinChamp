using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

namespace Isra.Xam.RetoFinal.Droid.Services
{
    public class ServiceImage
    {
        public static async Task<MediaFile> TakePicture(string usuario, bool UseCam = true)
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
                Name = string.Format("{0}{1}.jpg", usuario, DateTime.Now.ToString("yyyMMdd hh:mm:ss")),
                Directory = "XamChampFinal"
            }) : await CrossMedia.Current.PickPhotoAsync();

            return file;
        }
    }
}