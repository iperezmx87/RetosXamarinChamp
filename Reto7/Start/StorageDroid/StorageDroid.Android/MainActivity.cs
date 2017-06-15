using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Threading;

namespace StorageDroid.Droid
{
    [Activity(Label = "StorageDroid.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public string email = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += Button_Click;

            Button btnEnviarImagen = FindViewById<Button>(Resource.Id.btnEnviarImagen);

            btnEnviarImagen.Click += BtnEnviarImagen_Click;
        }

        private async void BtnEnviarImagen_Click(object sender, EventArgs e)
        {
            var view = this.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Android.Graphics.Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 0, stream);
                bitmapData = stream.ToArray();

                StorageDroid.StorageService storageSvc = new StorageService();

                await storageSvc.UploadImageAsync(bitmapData, string.Format("{0}.jpg", Guid.NewGuid().ToString()));

                Thread.Sleep(2000);

                Toast.MakeText(this, "Captura enviada correctamente", ToastLength.Short).Show();
            }
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            StorageDroid.StorageService storageSvc = new StorageService();
            await storageSvc.performBlobOperation(email);

            TorneoItem registro = new TorneoItem()
            {

                DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver,
               Android.Provider.Settings.Secure.AndroidId
               ),
                Email = email,
                Reto = "Reto7 + " + email
            };

            Toast.MakeText(this, "Enviando registro.", ToastLength.Long).Show();

            await ItemManager.DefaultManager.SaveTaskAsync(registro);

            Toast.MakeText(this, "Registro enviado exitosamente.", ToastLength.Long).Show();
        }
    }
}


