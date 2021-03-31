using System;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;

namespace smiqs.Models
{
    public class tblEvents
    {

        public static MobileServiceClient client = new MobileServiceClient("https://smiqsdbcrud.azurewebsites.net");
        // [PrimaryKey, AutoIncrement]
        [PrimaryKey]
        public int id { get; set; }
        public string DeviceID { get; set; }
        public DateTime eventDate { get; set; }
        public string status { get; set; }
        public string valveName { get; set; }
        public string flowml { get; set; }
        public string elapsedtime { get; set; }
        public string GlobalWindSpeed { get; set; }
        public string GlobalWindDegree { get; set; }
        public string GlobalWeatherDescription { get; set; }
        public string VWCMin { get; set; }
        public string VWCMax { get; set; }

        public string GlobalGH { get; set; }
        public string GlobalGT { get; set; }
        public string SSA { get; set; }
        public string message { get; set; }
        public string valvaStatus { get; set; }
        public string groupID { get; set; }

        public string valveValue { get; set; }

        public DateTime timestamp { get; set; }

        public string Duration { get; set; }
        public float Cost { get; set; }
        public int flowL { get; set; }
        public string strflowL { get; set; }
    }
}
