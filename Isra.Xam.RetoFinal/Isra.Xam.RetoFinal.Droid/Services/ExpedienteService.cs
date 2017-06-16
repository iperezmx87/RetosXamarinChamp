using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Isra.Xam.RetoFinal.Droid.Services
{
    public class ExpedienteService
    {
        MobileServiceClient clienteServicio = new MobileServiceClient(@"http://ihousemx87mobapp.azurewebsites.net");

        private IMobileServiceTable<RegistroExpediente> _PsychoItemTable;

        public async Task<List<RegistroExpediente>> BuscarRegistros(string nombreUsuario)
        {
            _PsychoItemTable = clienteServicio.GetTable<RegistroExpediente>();
            List<RegistroExpediente> items = await _PsychoItemTable.Where(
                registro => registro.nombreusuario == nombreUsuario
                ).ToListAsync();

            return items;
        }

        public async Task InsertarRegistro(RegistroExpediente item)
        {
            _PsychoItemTable = clienteServicio.GetTable<RegistroExpediente>();

            await _PsychoItemTable.InsertAsync(item);
        }
    }
}