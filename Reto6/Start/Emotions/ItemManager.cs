using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace Emotions
{
    public partial class ItemManager
    {
        static ItemManager defaultInstance = new ItemManager();

        MobileServiceClient client;

        IMobileServiceTable<TorneoItem> todoTable;

        public ItemManager()
        {
            client = new MobileServiceClient(@"https://xamarinchampions.azurewebsites.net/");
            todoTable = client.GetTable<TorneoItem>();
        }

        public static ItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }
        public MobileServiceClient CurrentClient
        {
            get
            {

                return client;
            }
        }

        public async Task SaveTaskAsync(TorneoItem item)
        {
            if (item.Id == null)
            {
                await todoTable.InsertAsync(item);
            }
            else
            {
                await todoTable.UpdateAsync(item);
            }
        }
    }
}
