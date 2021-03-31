using SQLite;
using System;
using System.ComponentModel;
namespace smiqs.Models

{


    public class TelemetryEventsModel : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// The declaration of the property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        [PrimaryKey]
        public int telID { get; set; }
        public int ID { get; set; }
        public string DeviceICCID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string valveName { get; set; }
        public bool valveValue { get; set; }
        public string Message { get; set; }
        public float SSA { get; set; }
        public float SSB { get; set; }
        public float SSC { get; set; }
        public float SSD { get; set; }

        public float Cost { get; set; }
        public bool VA { get; set; }
        public bool VB { get; set; }
        public bool VC { get; set; }
        public bool VD { get; set; }

        public string status { get; set; }

        public float GlobalGH { get; set; }

        public float GlobalGT { get; set; }

        public int VWCMin { get; set; }

        public int VWCMax { get; set; }

        public DateTime teltimestamp { get; set; }

        public string TelemtryTimeStamp { get; set; }

        public string GlobalWindSpeed { get; set; }
        public string GlobalWindDegree { get; set; }
        public string GlobalWeatherDescription { get; set; }
        public string DeviceID { get; set; }
        public int elapsedtime { get; set; }
        public float flowml { get; set; }
        public string Duration { get; set; }

        public float flowL { get; set; }
        public string strflowL { get; set; }


        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        public void getValve_On_Telemetry()
        {

        }
        public void getValve_Off_Telemetry()
        {

        }
        public void calculateElapsed()
        {

        }
        public int calculateFlowLiters(int pulses)
        {
            return (int)(pulses * 2.25 / 1000);
        }

        public int getCurrentSSA(string deviceID)
        {
            return 0;
        }
        public int getCurrentSSB(string deviceID)
        {
            return 0;
        }

        public TelemetryEventsModel(string DeviceID)
        {

        }


        public TelemetryEventsModel()
        {

        }
        public class TelemetryValue
        {
            public string value { get; set; }
            public DateTime timestamp { get; set; }
        }
      
       
        #endregion
    }

   
}
