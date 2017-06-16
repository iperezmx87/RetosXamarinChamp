using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace Isra.Xam.RetoFinal.Droid.Services
{
    public class ExpedienteStorageService
    {
        public async Task UploadImageAsync(byte[] image, string imageName)
        {
            try
            {
                // Subir una imagen a un blob storage de azure :D
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ihousepsychostorage;AccountKey=t94pgOvK021PPZVHRD2etTxDLyV69lhaUB2ooZFf0VHtugTXJ688sbaGJKJYqY7qSCKurMDNndjC+Tym+FSTbw==;EndpointSuffix=core.windows.net");
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("fotosemohistory");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);

                await blockBlob.UploadFromByteArrayAsync(image, 0, image.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}