using System;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;
namespace smiqs.Models
{
    public class tblSmiqsTelemetry
    {
        public static MobileServiceClient client = new MobileServiceClient("https://smiqsdbcrud.azurewebsites.net");
        // [PrimaryKey, AutoIncrement]
        [PrimaryKey]
        public int id { get; set; }
        public string DeviceID { get; set; }
        public string message { get; set; }
        public string SSA { get; set; }
        public string SSB { get; set; }
        public string SSC { get; set; }
        public string SSD { get; set; }

        public string VA { get; set; }
        public string VB { get; set; }
        public string VC { get; set; }
        public string VD { get; set; }

        public string GlobalGH { get; set; }
        public string GlobalGT { get; set; }

        public string GlobalWindSpeed { get; set; }
        public string GlobalWeatherDescription { get; set; }
        public string VWCMin { get; set; }
        public string VWCMax { get; set; }
        public string Status { get; set; }

        public string elapsedtime { get; set; }

        public string flowml { get; set; }


        public DateTime teltimestamp { get; set; }

        public DateTime eventDate { get; set; }

    }
}
