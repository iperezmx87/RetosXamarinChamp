using Android.App;
using Android.OS;
using Android.Widget;
using EmotionApiDemo.Core;
using EmotionApiDemo.Core.Model;
using Isra.Xam.RetoFinal.Droid.Services;
using Plugin.Media.Abstractions;
using System;
using System.IO;

namespace Isra.Xam.RetoFinal.Droid
{
    [Activity(Label = "Registro de expediente.")]
    public class RegistrarExpedienteAcivity : Activity
    {
        static Stream streamCopy;

        TextView txtResultado;
        EditText txtUsuario;

        // registrar rsultados sube el registro a la tabla de azure y sube la foto a storage
        Button btnRegistrarResultados;

        // consumir la emocion
        Button btnAnalizaFoto;
        private static string ApiKey = "26bc5b91bc3d4b3b968673130596e90c";

        public string imagePath { get; set; }

        public string imageName { get; set; }

        public string emocionProminente { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.RegistraExpediente);

            Button btnCamara = FindViewById<Button>(Resource.Id.btnCamara);
            btnAnalizaFoto = FindViewById<Button>(Resource.Id.btnAnalizaFoto);
            btnRegistrarResultados = FindViewById<Button>(Resource.Id.btnRegistraResultados);
            txtResultado = FindViewById<TextView>(Resource.Id.txtOutput);

            txtUsuario = FindViewById<EditText>(Resource.Id.editTextUsuario);

            btnRegistrarResultados.Visibility = Android.Views.ViewStates.Invisible;
            btnAnalizaFoto.Visibility = Android.Views.ViewStates.Invisible;

            btnCamara.Click += BtnCamara_Click;
            btnAnalizaFoto.Click += BtnAnalizaFoto_Click;
            btnRegistrarResultados.Click += BtnRegistrarResultados_Click;
        }

        private async void BtnRegistrarResultados_Click(object sender, System.EventArgs e)
        {
            if (txtUsuario.Text != "")
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        btnRegistrarResultados.Visibility = Android.Views.ViewStates.Invisible;

                        // subir foto y registro a azure
                        Toast.MakeText(this, "Registrando la emoción al expediente.", ToastLength.Short).Show();

                        ExpedienteService serviceIsra = new ExpedienteService();
                        await serviceIsra.InsertarRegistro(new RegistroExpediente()
                        {
                            emocionprominente = emocionProminente,
                            nombreusuario = txtUsuario.Text
                        });

                        Toast.MakeText(this, "Registro generado correctamente.", ToastLength.Short).Show();

                        // subiendo foto a storage
                        ExpedienteStorageService storageService = new ExpedienteStorageService();

                        await storageService.UploadImageAsync(GetImageAsByteArray(imagePath), string.Format("{0}{1}.jpg", txtUsuario.Text, DateTime.Now.ToString("yyyMMdd hh:mm:ss")));

                        Toast.MakeText(this, "Foto cargada a nube de forma correcta.", ToastLength.Short).Show();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Short).Show();
                        btnRegistrarResultados.Visibility = Android.Views.ViewStates.Invisible;
                    }
                }
                else
                {
                    Toast.MakeText(this, "No hay una conexión disponible.", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "No se ha ingresado nombre de paciente.", ToastLength.Long).Show();
            }
        }

        private async void BtnAnalizaFoto_Click(object sender, System.EventArgs e)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    if (streamCopy != null)
                    {
                        btnAnalizaFoto.Visibility = Android.Views.ViewStates.Invisible;
                        Toast.MakeText(this, "Detectando emociones... ", ToastLength.Short).Show();

                        using (EmotionEngine emoEngine = new EmotionEngine(ApiKey))
                        {
                            streamCopy.Seek(0, SeekOrigin.Begin);
                            FaceEmotion[] emotionFaces = await emoEngine.CalculateEmotion(this.GetImageAsByteArray(imagePath));

                            if (emotionFaces.Length > 0)
                            {
                                emocionProminente = emoEngine.DetectEmocion(emotionFaces[0].scores);

                                emocionProminente = emocionProminente.Replace("Un rostro parece estar ", "");

                                emocionProminente = emocionProminente.Replace("Un rostro parece no mostrar emociones (Neutral)", "NEUTRAL");

                                txtResultado.Text += "Emoción prominente: " + emocionProminente;
                                btnRegistrarResultados.Visibility = Android.Views.ViewStates.Visible;
                            }
                            else
                            {
                                Toast.MakeText(this, "No se detectaron caras.", ToastLength.Short).Show();
                            }
                        }
                    }
                    else
                    {
                        txtResultado.Text = "--no se ha seleccionado una imagen---";
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "No hay una conexión disponible.", ToastLength.Long).Show();
            }
        }

        private async void BtnCamara_Click(object sender, System.EventArgs e)
        {
            txtResultado.Text = "Output: ";
            MediaFile file = null;
            try
            {
                file = await ServiceImage.TakePicture(txtUsuario.Text, true);
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

            imagePath = file.Path;

            imageView.SetImageURI(Android.Net.Uri.Parse(file.Path));
            var stream = file.GetStream();
            streamCopy = new MemoryStream();
            stream.CopyTo(streamCopy);
            stream.Seek(0, SeekOrigin.Begin);
            file.Dispose();
        }

        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}