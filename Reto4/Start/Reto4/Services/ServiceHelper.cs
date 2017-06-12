using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reto4.Services
{
    public class ServiceHelper
    {
        MobileServiceClient clienteServicio = new MobileServiceClient(@"http://xamarinchampions.azurewebsites.net");

        private IMobileServiceTable<TorneoItem> _TorneoItemTable;

        public async Task<List<TorneoItem>> BuscarRegistros(string correo)
        {
            _TorneoItemTable = clienteServicio.GetTable<TorneoItem>();
            System.Collections.Generic.List<TorneoItem> items = await _TorneoItemTable.Where(
                torneoItem => torneoItem.Email == correo
                ).ToListAsync();

            return items;
        }
    }
}
