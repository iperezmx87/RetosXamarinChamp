using Android.App;
using Android.OS;
using Android.Widget;
using Isra.Xam.RetoFinal.Droid.Services;
using System;
using System.Text;

namespace Isra.Xam.RetoFinal.Droid
{
    [Activity(Label = "Consulta de expediente.")]
    public class ConsultaExpedientesActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ConsultaExpedientes);

            FindViewById<Button>(Resource.Id.btnConsultaRegistros).Click += OnConsultaClicked;
        }

        private async void OnConsultaClicked(object sender, EventArgs e)
        {
            if (FindViewById<EditText>(Resource.Id.editTextNombrePaciente).Text != "")
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    ExpedienteService service = new ExpedienteService();
                    var items = await service.BuscarRegistros(FindViewById<EditText>(Resource.Id.editTextNombrePaciente).Text);

                    StringBuilder builder = new StringBuilder();

                    builder.Append("Emociones registradas: ");

                    builder.AppendLine();

                    foreach (var item in items)
                    {
                        builder.Append(string.Format("{0}: {1}\n", item.CREATEDAT, item.emocionprominente));
                    }

                    RunOnUiThread(() =>
                    {
                        FindViewById<TextView>(Resource.Id.textViewRegistros).Text = builder.ToString();
                        FindViewById<EditText>(Resource.Id.editTextNombrePaciente).Text = "";
                    });
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
    }
}