using System;
using System.Collections.Generic;
using System.Text;

namespace smiqs.ViewModels.Helpers
{
    public class OpenWeatherMap
    {
        public string apiResponse { get; set; }

        public Dictionary<string, string> cities
        {
            get; set;
        }
    }
}
