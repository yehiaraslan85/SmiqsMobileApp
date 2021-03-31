using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Model = smiqs.Models.Article;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using static smiqs.Views.MainMenu;
using static smiqs.Models.Telemetry;
using smiqs.Models;
using smiqs.Helper;
using System.Collections.ObjectModel;

namespace smiqs.Views.Catalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private WebAPIService webAPIService;

        public string RealTimeValue { get; set; }
        public bool IsStopVisible { get; set; }
        public string LastIrrigationDuration { get; set; }
        public string RealTimeStamp { get; set; }
        public ObservableCollection<tblEvents> Items { get; set; }

        public LoadingPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            overlay.IsVisible = true;
            App.DevResult = await GetDevicePerUserAsync(Application.Current.Properties["UserEmail "].ToString());

            if (App.DevResult.Count == 0)
            {
                overlay.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new Views.Forms.FillInDetailsPage());

            }
            else
            {
                foreach (var device in App.DevResult)
                {
                    try
                    {
                        Model Dev = new Model();

                        //Get List of device IDs from SQL
                        //  await InitiateDBAsync(device.deviceICCID, device.deviceType);
                        // CalculateTotalCostPerMonth(device.deviceICCID,device.deviceType);
                        // Updates the recent irrigation time
                        await UpdateDeviceStatusAsync(device.deviceICCID, device.deviceType);

                        Dev.Name = device.deviceName;
                        Dev.Author = device.deviceMode;
                        Dev.ImagePath = "";
                        // await GetRequest(device.deviceType, device.deviceICCID);

                        await GetIrrigationStatus(device.deviceType, device.deviceICCID);
                        try
                        {
                            Stream ms = new MemoryStream(device.deviceImageBytes);
                            Dev.ImageBytes = device.deviceImageBytes;
                            Dev.DeviceType = device.deviceType;
                            Dev.DeviceImageSource = ImageSource.FromStream(() => ms);
                        }
                        catch (Exception e)
                        {

                        }
                        //Dev.ImagePath = ImageSource.FromStream(() => ms);
                        Dev.Date = device.deviceInDate;
                        Dev.AverageReadingTime = device.deviceICCID;
                        Dev.Description = device.deviceICCID;
                        Dev.BookmarkedCount = RealTimeValue;
                        Dev.FavouritesCount = LastIrrigationDuration;
                        App.ListOfDevices.Add(Dev);
                       await InitiateDBAsync(device.deviceICCID, device.deviceType);
                    }
                    catch (Exception ex)
                    {

                    }
                }


                overlay.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new ArticleCardPage());
            }
            overlay.IsVisible = false;
        }
        private async Task InitiateDBAsync(string SmiqsICCID, string sensor)
        {
            webAPIService = new WebAPIService();
            //Item source which needs to be displayed on the list view.

            await GetDataFromWebAPI(SmiqsICCID, sensor);


            //Get Telemetries
            var EventHighestTelemetry = App.EventsDatabase.getLastReadingDate(SmiqsICCID, sensor);
            if (EventHighestTelemetry.Result == null)
            {
                var FullEvents_Results = from mytable in Items
                                         where
                                         mytable.DeviceID == SmiqsICCID && mytable.valveName.Contains(sensor)
                                         select mytable;
                List<tblEvents> FullEvents = FullEvents_Results.ToList();

                foreach (var T in Items)
                    await App.EventsDatabase.SaveTelemetryAsync(T);
            }
            else
            {
                var DateRange_result = from mytable in Items
                                       where
                                       mytable.eventDate > EventHighestTelemetry.Result.eventDate
                                       &&
                                       mytable.DeviceID == SmiqsICCID
                                       && mytable.valveName.Contains(sensor)
                                       select mytable;
                List<tblEvents> NewEvents = DateRange_result.ToList();
                foreach (var T in NewEvents)
                    await App.EventsDatabase.SaveTelemetryAsync(T);
            }
        }

        async Task GetDataFromWebAPI(string iccid, string sensor)
        {
            Items = await webAPIService.RefreshDataAsync(iccid, sensor, "Finish");
            

        }

        public async Task GetIrrigationStatus(string Telemetry, string DeviceID)
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
                var val = IoTCentralResult.value.ToString();
                DateTime RealTimeStamptemp = IoTCentralResult.timestamp;
                RealTimeStamp = DateTime.Now.Subtract(RealTimeStamptemp.AddHours(4)).TotalMinutes.ToString();
                var temp = (int)Convert.ToDouble(RealTimeStamp);
                if (val.Equals("1"))
                {
                    //  if (Telemetry.Equals("VA") || Telemetry.Equals("VB") || Telemetry.Equals("VC"))
                    //  {
                    // Status_IsVisible = true;
                    LastIrrigationDuration += " - Device Irrigating";

                    //    }

                }
                else
                {
                    // LastIrrigationDuration += " - Happy Garden";

                }
                if (temp > 10)
                {
                    IsStopVisible = false;
                    LastIrrigationDuration = "Device disconnected";
                }
            }
            else
            {
                RealTimeValue = "0";
                RealTimeStamp = "Error";
            }

        }
        private async Task UpdateDeviceStatusAsync(string iccid, string sensor)
        {

            try
            {
                var httpClient = new HttpClient();
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://smiqsdbcrud.azurewebsites.net/api/tblEvents/" + iccid + "/" + sensor),

                };
                var httpResponse = await httpClient.SendAsync(request);
                var s = httpResponse.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<tblEvents>(s.Result);


                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var hrsduration = DateTime.UtcNow.AddHours(4).Subtract(result.eventDate);
                    var minutes = hrsduration.TotalMinutes;
                    if (minutes > 60)
                        LastIrrigationDuration = "Last Irrigation (" + Math.Round(hrsduration.TotalHours, 2).ToString() + " hrs ago)";
                    else
                        LastIrrigationDuration = "Last Irrigation (" + hrsduration.Minutes.ToString() + " min ago)";

                }
                else
                {
                    LastIrrigationDuration = "No Recent Irrigations";
                }

            }
            catch (Exception e)
            {
                LastIrrigationDuration = e.Message;

            }
        }
        public async Task<List<Models.Device>> GetDevicePerUserAsync(string useremail)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://smiqsdbcrud.azurewebsites.net/api/tbldevices/" + useremail),

            };
            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            var DevResult = JsonConvert.DeserializeObject<List<Models.Device>>(s.Result);


            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return DevResult;
            }
            else
            {
                return null;
            }

        }
    }
}