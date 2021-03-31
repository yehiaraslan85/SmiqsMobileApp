using System;
using System.Collections.Generic;
using System.Text;

namespace smiqs.ViewModels.Helpers
{
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}
