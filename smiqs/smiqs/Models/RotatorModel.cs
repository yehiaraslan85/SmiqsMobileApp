using Newtonsoft.Json;
using smiqs.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static smiqs.Models.Telemetry;
using static smiqs.Views.MainMenu;

namespace smiqs.Models
{
    public class RotatorModel : BaseViewModel
    {
        public string realtemperature { get; set; }

        public string realstatus { get; set; }
        public string realtimevalue { get; set; }
        public string realtimevwcminvalue { get; set; }
        public string realtimevwcmaxvalue { get; set; }
        private string feedback { get; set; }

        public string realtimestamp { get; set; }

        public RotatorModel(string imageString)
        {
            Image = imageString;
            TotalSpentText = "hi";
          
        }
        private String _image;
        public String Image
        {
            get { return _image; }
            set { _image = value; }
        }
      
        private string totalspenttext { get; set; }
        public async Task
GetRequest(string Telemetry, string DeviceID)
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
                RealTimeValue = IoTCentralResult.value.ToString();
                DateTime RealTimeStamptemp = IoTCentralResult.timestamp;
                RealTimeStamp = DateTime.Now.Subtract(RealTimeStamptemp.AddHours(4)).TotalMinutes.ToString();
                var temp = (int)Convert.ToDouble(RealTimeStamp);
                RealTimeStamp = temp + " minutes ago";
                if (temp > 10)
                {
                    Feedback = "Device disconnected";
                }
            }
            else
            {
                RealTimeValue = "0";
                RealTimeStamp = "Error";
            }

        }

        public async Task
GetIrrigationStatus(string Telemetry, string DeviceID)
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
                RealTimeValue = IoTCentralResult.value.ToString();
                DateTime RealTimeStamptemp = IoTCentralResult.timestamp;
                RealTimeStamp = DateTime.Now.Subtract(RealTimeStamptemp.AddHours(4)).TotalMinutes.ToString();
                var temp = (int)Convert.ToDouble(RealTimeStamp);
                RealTimeStamp = temp + " minutes ago";
                if (temp > 10)
                {
                    Feedback = "Device disconnected";
                }
            }
            else
            {
                RealTimeValue = "0";
                RealTimeStamp = "Error";
            }

        }

        public async Task
GetRequestVWCMin(string Telemetry, string DeviceID)
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
                RealTimeVWCMinValue = IoTCentralResult.value.ToString();
                DateTime RealTimeStamptemp = IoTCentralResult.timestamp;
                // RealTimeStamp = DateTime.Now.Subtract(RealTimeStamptemp).TotalMinutes.ToString();

            }
            else
            {
                RealTimeVWCMinValue = "0";
                //RealTimeStamp = "Error";
            }

        }
        public async Task GetRequestVWCMax(string Telemetry, string DeviceID)
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
                RealTimeVWCMaxValue = IoTCentralResult.value.ToString();
                DateTime RealTimeStamptemp = IoTCentralResult.timestamp;
                //   RealTimeStamp = DateTime.Now.Subtract(RealTimeStamptemp).TotalMinutes.ToString();

            }
            else
            {
                RealTimeVWCMaxValue = "0";
                //  RealTimeStamp = "Error";
            }

        }

        public string RealTimeValue
        {
            get
            {
                return realtimevalue;
            }
            set
            {
                this.realtimevalue = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealTemperature
        {
            get
            {
                return realtemperature;
            }
            set
            {
                this.realtemperature = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealStatus
        {
            get
            {
                return realstatus;
            }
            set
            {
                this.realstatus = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealTimeVWCMinValue
        {
            get
            {
                return realtimevwcminvalue;
            }
            set
            {
                this.realtimevwcminvalue = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealTimeVWCMaxValue
        {
            get
            {
                return realtimevwcmaxvalue;
            }
            set
            {
                this.realtimevwcmaxvalue = value;
                this.NotifyPropertyChanged();
            }
        }
        public string TotalSpentText
        {
            get
            {
                return totalspenttext;
            }
            set
            {
                this.totalspenttext = value;
                this.NotifyPropertyChanged();
            }
        }

        public string Feedback
        {
            get
            {
                return this.feedback;
            }
            set
            {
                this.feedback = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealTimeStamp
        {
            get
            {
                return realtimestamp;
            }
            set
            {
                this.realtimestamp = value;
                this.NotifyPropertyChanged();
            }
        }
    }
}
