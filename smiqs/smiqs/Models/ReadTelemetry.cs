using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smiqs.Models
{
   public class ReadTelemetry
    {
        public string SSA { get; set; }
        public string id { get; set; }

        public static MobileServiceClient client = new MobileServiceClient("https://smartiqswebapp.azurewebsites.net");

        public static async Task<List<ReadTelemetry>> ReadTelemetryfromDB()
        {
            return await client.GetTable<ReadTelemetry>().ToListAsync();
        }
    }
}
