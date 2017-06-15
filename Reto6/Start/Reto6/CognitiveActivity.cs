using Android.App;
using Android.OS;
using Android.Widget;
using Emotions;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reto6
{
    [Activity(Label = "Registrar datos")]
    public class CognitiveActivity : Activity
    {

        ItemManager manager;
        static Stream streamCopy;
        string resultadoEmociones = "Reto6 + MX + Nombre: ";
        TextView txtResultado;
        Button btnRegistrarResultados;
        Button btnAnalizaFoto;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            manager = ItemManager.DefaultManager;
            SetContentView(Resource.Layout.Cognitive);

            Button btnCamara = FindViewById<Button>(Resource.Id.btnCamara);
            btnAnalizaFoto = FindViewById<Button>(Resource.Id.btnAnalizaFoto);
            btnRegistrarResultados = FindViewById<Button>(Resource.Id.btnRegistraResultados);
            txtResultado = FindViewById<TextView>(Resource.Id.txtOutput);

            btnRegistrarResultados.Visibility = Android.Views.ViewStates.Invisible;
            btnAnalizaFoto.Visibility = Android.Views.ViewStates.Invisible;

            btnCamara.Click += BtnCamara_Click;
            btnAnalizaFoto.Click += BtnAnalizaFoto_Click;
            btnRegistrarResultados.Click += BtnRegistrarResultados_Click;
        }

        private async void BtnRegistrarResultados_Click(object sender, System.EventArgs e)
        {
            btnRegistrarResultados.Visibility = Android.Views.ViewStates.Invisible;
            Toast.MakeText(this, "Registrando tus resultados", ToastLength.Short).Show();
            TorneoItem registro = new TorneoItem();
            registro.DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            registro.Email = "";
            registro.Reto = resultadoEmociones;
            await manager.SaveTaskAsync(registro);
            Toast.MakeText(this, "Resultados registrados correctamente.", ToastLength.Short).Show();
        }

        private async void BtnAnalizaFoto_Click(object sender, System.EventArgs e)
        {

            if (streamCopy != null)
            {
                btnAnalizaFoto.Visibility = Android.Views.ViewStates.Invisible;
                Toast.MakeText(this, "Analizndo imagen usando cognitive services", ToastLength.Short).Show();
                Dictionary<string, float> emotions = null;
                try
                {
                    streamCopy.Seek(0, SeekOrigin.Begin);
                    emotions = await ServiceEmotions.GetEmotions(streamCopy);

                }
                catch (Exception)
                {
                    Toast.MakeText(this, "Ha ocurrido un error en la conexion a los servicios.", ToastLength.Short).Show();
                    return;
                }

                StringBuilder builder = new StringBuilder();
                if (emotions != null)
                {
                    txtResultado.Text = "---Analisis de emociones---";
                    builder.AppendLine();
                    foreach (var emotion in emotions)
                    {
                        string toAdd = emotion.Key + " : " + emotion.Value + " ";
                        builder.Append(toAdd);
                    }

                    txtResultado.Text += builder.ToString();
                    btnRegistrarResultados.Visibility = Android.Views.ViewStates.Visible;
                }
                else
                {
                    txtResultado.Text = "---No se detecto cara---";
                }
                resultadoEmociones += builder.ToString();
            }
            else
            {
                txtResultado.Text = "--no se ha seleccionado una imagen---";
            }
        }

        private async void BtnCamara_Click(object sender, System.EventArgs e)
        {
            MediaFile file = null;
            try
            {
                file = await ServiceImage.TakePicture(true);

            }
            catch (Android.OS.OperationCanceledException) { }
            SetImageControl(file);
            btnAnalizaFoto.Visibility = Android.Views.ViewStates.Visible;
        }

        private void SetImageControl(MediaFile file)
        {
            if (file == null)
            {
                return;
            }

            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageViewFoto);
            imageView.SetImageURI(Android.Net.Uri.Parse(file.Path));
            var stream = file.GetStream();
            streamCopy = new MemoryStream();
            stream.CopyTo(streamCopy);
            stream.Seek(0, SeekOrigin.Begin);
            file.Dispose();
        }

    }
}