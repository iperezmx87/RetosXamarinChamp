using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StorageDroid
{
    public class StorageService
    {
        public StorageService()
        {
        }

        public async Task performBlobOperation(string participante)
        {
            try
            {
                string blobSAS = "https://torneoxamarin.blob.core.windows.net/devs/" + participante + ".txt?sv=2016-05-31&ss=b&srt=sco&sp=wlac&se=2017-07-01T15:00:00Z&st=2017-05-22T18:00:00Z&sip=0.0.0.0-255.255.255.255&spr=https,http&sig=UDaqkQEJrd8E%2FCPhLwWmrM3ZZobdTlTqQadVIy67pXc%3D";
                CloudBlockBlob blob = new CloudBlockBlob(new Uri(blobSAS));

                string blobContent = "Israel Perez Saucedo+MX";
                MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                msWrite.Position = 0;
                using (msWrite)
                {
                    await blob.UploadFromStreamAsync(msWrite);
                }
            }
            catch (Exception exc)
            {
                string msgError = exc.Message;
            }

        }

        public async Task UploadImageAsync(byte[] image, string imageName)
        {
            try
            {
                string blobSAS = "https://torneoxamarin.blob.core.windows.net/devs/" + imageName + "?sv=2016-05-31&ss=b&srt=sco&sp=wlac&se=2017-07-01T15:00:00Z&st=2017-05-22T18:00:00Z&sip=0.0.0.0-255.255.255.255&spr=https,http&sig=UDaqkQEJrd8E%2FCPhLwWmrM3ZZobdTlTqQadVIy67pXc%3D";
                CloudBlockBlob blob = new CloudBlockBlob(new Uri(blobSAS));

                await blob.UploadFromByteArrayAsync(image, 0, image.Length);
            }
            catch (Exception exc)
            {
                string msgError = exc.Message;
            }
        }
    }
}

