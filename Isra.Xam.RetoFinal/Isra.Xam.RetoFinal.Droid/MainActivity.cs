using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Isra.Xam.RetoFinal.Droid.Services;
using System;

namespace Isra.Xam.RetoFinal.Droid
{
    [Activity(Label = "EmoHistory", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public string urlGitHub = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            // botones
            FindViewById<Button>(Resource.Id.btnConsultaFoto).Click += OnConsultaExpedienteClicked;
            FindViewById<Button>(Resource.Id.btnRegistrarFoto).Click += OnRegistraExpedienteClicked;
            FindViewById<Button>(Resource.Id.btnRegistrarReto).Click += OnRegistraRetoClicked;

            // revisar si esta conectado a internet
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        // revisando la conectividad de la app
        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            // si esta conectado a internet, se habilitan los botones, de lo contrario se deshabilitan los botones.
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(this, "Conectado a Internet.", ToastLength.Long).Show();
                FindViewById<Button>(Resource.Id.btnConsultaFoto).Enabled = true;
                FindViewById<Button>(Resource.Id.btnRegistrarFoto).Enabled = true;
                FindViewById<Button>(Resource.Id.btnRegistrarReto).Enabled = true;
            }
            else
            {
                Toast.MakeText(this, "No hay una conexión disponible.", ToastLength.Long).Show();
                FindViewById<Button>(Resource.Id.btnConsultaFoto).Enabled = false;
                FindViewById<Button>(Resource.Id.btnRegistrarFoto).Enabled = false;
                FindViewById<Button>(Resource.Id.btnRegistrarReto).Enabled = false;
            }
        }

        // registro del reto final :(
        private void OnRegistraRetoClicked(object sender, EventArgs e)
        {
            TorneoItem registro = new TorneoItem() {
                DeviceId = Android.Provider.Settings.Secure.GetString(
                    ContentResolver,
                    Android.Provider.Settings.Secure.AndroidId
                    ),
                Email = "neomatrixisra25@hotmail.com",
                Reto = "RetoFinal + " + urlGitHub
            };

            ChampServiceHelper helper = new ChampServiceHelper();
            helper.InsertarEntidad(registro);
        }

        // actividad de registro de expediente
        private void OnRegistraExpedienteClicked(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegistrarExpedienteAcivity));
            StartActivity(intent);
        }

        // actividad consultar expediente
        private void OnConsultaExpedienteClicked(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ConsultaExpedientesActivity));
            StartActivity(intent);
        }
    }
}