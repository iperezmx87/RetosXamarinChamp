using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace Isra.Xam.RetoFinal.Droid.Services
{
    public class RegistroExpediente
    {
        private string _id;
        private string _nombreusuario;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        [JsonProperty(PropertyName = "nombreusuario")]
        public string nombreusuario
        {
            get { return _nombreusuario; }
            set
            {
                _nombreusuario = value;
            }
        }

        [JsonProperty(PropertyName = "CREATEDAT")]
        public DateTime CREATEDAT { get; set; }

        public string emocionprominente { get; set; }

        [Version]
        public string Version { get; set; }
    }
}