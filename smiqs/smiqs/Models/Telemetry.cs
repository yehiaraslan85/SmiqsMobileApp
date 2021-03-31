using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using static smiqs.Views.MainMenu;

namespace smiqs.Models
{
    public class Telemetry : INotifyPropertyChanged
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

        public string Status { get; set; }

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
       
        public Telemetry(string DeviceID)
        {
            GetRequest("SSA", DeviceID);
           
        }
        public Telemetry()
        {

        }
        public class TelemetryValue
        {
            public string value { get; set; }
            public DateTime timestamp { get; set; }
        }
        public async void PostRequest()
        {
            var httpClient = new HttpClient();
            //var data = "{'request': true}"; //May not need this, based on your app needs
            var contentType = "application/json"; //May vary based on your app
            var httpMethod = HttpMethod.Post; //or Get, or whatever HTTP verb your API endpoint needs
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/8997103101017701450/components/SmartiqsGSMV1_6j7/commands/COMMANDVALVESTATUSA"),
                Method = httpMethod,
                Content = new StringContent(content, System.Text.Encoding.UTF8, contentType),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);

            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {

            }
            else
            {
                //It didn't work, so do something else
            }
        }
        public async void GetRequest(string Telemetry, string DeviceID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/" + Telemetry),
               
            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            TelemetryValue IoTCentralResult = JsonConvert.DeserializeObject<TelemetryValue>(s.Result);

           

           // SSA = float.Parse(xx.ToString());

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                SSA = float.Parse(IoTCentralResult.value.ToString());
            }
            else
            {
                SSA = 0;
            }
        }
        #endregion
    }
}
