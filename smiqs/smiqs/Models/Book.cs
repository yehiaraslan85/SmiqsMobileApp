using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
namespace smiqs.Models
{
   public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public DateTime FinishedDate { get; set; }
        public string Status { get; set; }
        public string ValveName { get; set; }
        public string Flowml { get; set; }
        public string Elapsedtime { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://smiqsdbcrud.azurewebsites.net");

        public async Task<bool> SaveBook()
        {
            try
            {
                await client.GetTable<Book>().InsertAsync(this);
                return true;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                var response = await msioe.Response.Content.ReadAsStringAsync();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static async Task<List<Book>> ReadBook()
        {

            try {
                return await client.GetTable<Book>().ToListAsync();

            }
            catch (Exception e)
            {
                string ex = e.Message;
                return null;
            }


        }
    }
}
