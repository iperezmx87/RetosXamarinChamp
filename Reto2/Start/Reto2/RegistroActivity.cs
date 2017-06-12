using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Reto2.Services;

namespace Reto2
{
    [Activity(Label = "Registrar datos")]
    public class RegistroActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registro);

            FindViewById<Button>(Resource.Id.btnRegistro).Click += OnBtnClicked;
        }

        private async void OnBtnClicked(object sender, EventArgs e)
        {
            try
            {
                var serviceHelper = new ServiceHelper();
                string email = FindViewById<EditText>(Resource.Id.editTextEmail).Text;
                string reto = Intent.GetStringExtra("Reto");
                string AndroidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

                if (string.IsNullOrEmpty(email))
                {
                    Toast.MakeText(this, "Por favor introduce un correo electrónico válido", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Enviando tu registro", ToastLength.Short).Show();
                    await serviceHelper.InsertarEntidad(email, reto, AndroidId);
                    Toast.MakeText(this, "Gracias por registrarte", ToastLength.Long).Show();
                    SetResult(Result.Ok, Intent);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                SetResult(Result.Canceled, Intent);
            }
        }
    }
}