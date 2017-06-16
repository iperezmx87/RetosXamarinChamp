using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetoN.Services
{
    public class ServiceHelper
    {
        MobileServiceClient clienteServicio = new MobileServiceClient(@"http://xamarinchampions.azurewebsites.net");

        private IMobileServiceTable<TorneoItem> _TorneoItemTable;

        public async Task InsertarEntidad(string direccionCorreo, string reto, string AndroidId)
        {
            _TorneoItemTable = clienteServicio.GetTable<TorneoItem>();


            await _TorneoItemTable.InsertAsync(new TorneoItem
            {
                Email = direccionCorreo,
                Reto = reto,
                DeviceId = AndroidId
            });
        }

        public async Task<List<TorneoItem>> BuscarRegistros(string correo)
        {
            _TorneoItemTable = clienteServicio.GetTable<TorneoItem>();
            List<TorneoItem> items = await _TorneoItemTable.Where(
                    torneoItem => torneoItem.Email == correo
                ).ToListAsync();

            return items;
        }
    }
}