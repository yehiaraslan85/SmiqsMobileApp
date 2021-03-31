using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using static smiqs.Models.TelemetryEventsModel;
using static smiqs.Views.MainMenu;

namespace smiqs.Models
{
    public class Device : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string deviceICCID { get; set; }
        public string deviceName { get; set; }
        public string userID { get; set; }
        public string deviceMode { get; set; }
        public string deviceType { get; set; }
        public string deviceVWCMin { get; set; }
        public string deviceVWCMax { get; set; }
        public string deviceAddress { get; set; }
        public string deviceInterval { get; set; }
        public string deviceInDate { get; set; }
        public string deviceTimestamp { get; set; }
        public string centralDnsSuffixInPath { get; set; }
        public string command_name { get; set; }
        public string component_name { get; set; }
        public string device_id { get; set; }
        public string subdomain { get; set; }
        public string ContentType { get; set; }
        public string Authorization { get; set; }

        public string lastSSA { get; set; }

        public string lastSSADate { get; set; }

        public string Feedback { get; set; }
        public string FeedbackColor { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;        
        public string deviceImagePath { get; set; }
        public Byte[] deviceImageBytes { get; set; }

        public string ValveName { get; set; }
        public string OwnerEmail { get; set; }
        #region Methods
        public Device()
        {
         
        }      
        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
