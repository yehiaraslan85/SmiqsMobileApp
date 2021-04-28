using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using smiqs.Models.Dashboard;
using System.Linq;
using smiqs.Models;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.SfCalendar.XForms;
using System.Net.Http;
using static smiqs.Views.MainMenu;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Input;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.DataSource.Extensions;
using smiqs.Views.Dashboard;
using Device = Xamarin.Forms.Device;
using smiqs.Views.Detail;
using Xamarin.Essentials;
using smiqs.Helper;
using Syncfusion.SfChart.XForms;

namespace smiqs.ViewModels.Dashboard
{
    /// <summary>
    /// ViewModel for my wallet page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MyWalletViewModel : BaseViewModel
    {
        #region Fields

        WebAPIService webAPIService;
        public event PropertyChangedEventHandler PropertyChanged;

        private int selectedIndex;
        private string feedback;
        private string feedbackcolor;
        private double totalBalance;
        private string lastirrigationduration;
        private string gardenname;
        private bool isrefreshing;
        public ICommand RefreshCommand { get; set; }

        private string[] today,days, weeks, months,years, xValues;
        public bool _Gauge_IsVisible;

        private ObservableCollection<tblEvents> dayListItems;

        private ObservableCollection<tblEvents> weekListItems;

        private ObservableCollection<tblEvents> monthListItems;

        private ObservableCollection<tblEvents> yearListItems;

        private ObservableCollection<tblEvents> listItems;

        private ObservableCollection<TransactionChartData> chartdata;
        private ObservableCollection<TelemetryEventsModel> datasource { get; set; }
        private ObservableCollection<splinedata> data;
        private ObservableCollection<splinedata> irrigationdata;

        public class splinedata
        {
            public string Month { get; set; }

            public double Target { get; set; }

            public splinedata(string xValue, double yValue)
            {
                Month = xValue;
                Target = yValue;
            }
        }

        private Command<object> itemTappedCommand;
        private Command<object> calTappedCommand;
        public ObservableCollection<tblEvents> Telemtries { get; private set; }
        public ObservableCollection<tblEvents> eventsTelemtries { get; private set; }

        public string queryReadUser = "SELECT * FROM dbo.tblUsers where firbaseUID LIKE @fbuserID";
        public string queryReadDevice = "SELECT * FROM dbo.tblDevices where userID LIKE @ID";
        public string queryReadTelemetry_All = "SELECT * FROM dbo.tblSmiqsTelemetry where DeviceID LIKE @DevID";
        public string queryReadEventsTelemetry_All = "SELECT * FROM dbo.tblEvents where DeviceID LIKE @DevID and valveName='VA' and status = 'Finish' ORDER BY Id";

        public string queryReadTelemetry_dt = "SELECT * FROM dbo.tblSmiqsTelemetry where DeviceID LIKE @DevID and teltimestamp BETWEEN @starttime and @endtime";
        public string queryReadEventsTelemetry_dt = "SELECT * FROM dbo.tblEvents where DeviceID LIKE @DevID and valveName='VA' and status = 'Finish' and eventDate BETWEEN @starttime and @endtime";

        public string queryReadTelemetry_On = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID and VA = 1";
        public string queryReadTelemetry_Off = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID and VA = 0";
        public string queryReadDevices = "SELECT * FROM dbo.tblDevices where U";
        public static ObservableCollection<tblEvents> AllTelemetry;
        public static ObservableCollection<tblEvents> AllEventsTelemetry;
        public static ObservableCollection<tblEvents> AllTelemetryThisMonth;
        public static List<Models.Device> AllDevices;

       
        public ObservableCollection<tblEvents> EventsTelemtries
        {
            get
            {
                return eventsTelemtries;
            }
            set
            {
                eventsTelemtries = value;
                this.NotifyPropertyChanged();
            }
        }
        public ObservableCollection<splinedata> Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                this.NotifyPropertyChanged();
            }
        }
        public ObservableCollection<splinedata> IrrigationData
        {
            get
            {
                return irrigationdata;
            }
            set
            {
                irrigationdata = value;
                this.NotifyPropertyChanged();
            }
        }
        public ObservableCollection<TransactionChartData> ChartData
        {
            get
            {
                return chartdata;
            }
            set
            {
                chartdata = value;
                this.NotifyPropertyChanged();
            }
        }
       
        public ObservableCollection<TelemetryEventsModel> DataSource
        {
            get
            {
                return datasource;
            }
            set
            {
                datasource = value;
                this.NotifyPropertyChanged();
            }
        }

        public string SmiqsFirebaseUID { get; set; }
        public string SmiqsUID { get; set; }
        public string SmiqsICCID { get; set; }
        public string Sensor { get; set; }
        public string SmiqsCommandName { get; set; }
        public string SmiqsAuthorization { get; set; }
        public string SmiqsComponentName { get; set; }
        public string realtemperature { get; set; }

        public string realstatus { get; set; }
        public string realtimevalue { get; set; }
        public string realtimevwcminvalue { get; set; }
        public string realtimevwcmaxvalue { get; set; }

        public string realtimevwcminvaluechart { get; set; }
        public string realtimevwcmaxvaluechart { get; set; }
        public string realtimevwcwidth { get; set; }
        #endregion
        public string realtimestamp { get; set; }

        public bool spline_isvisible { get; set; }

        public bool chart_isvisible { get; set; }
        public bool status_isvisible { get; set; }
        public string totalspenttext { get; set; }
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="MyWalletViewModel" /> class.
        /// </summary>
        /// 
        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();
       

        public MyWalletViewModel()
        {
           
        }
        public MyWalletViewModel(string iccid,string sensor,string gname)
        {
            IsRefreshing = true;
            SmiqsICCID = iccid;
            Sensor = sensor;
            GardenName = gname;
            RefreshCommand = new Command(Refresh);
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                Application.Current.MainPage.Navigation.PushAsync(new Views.ErrorAndEmpty.NoInternetConnectionPage());

            }
            else
            {
                try
                {
                    object o = new object();
                  
                    Refresh(o);
                   
                }
                catch (Exception e)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new Views.ErrorAndEmpty.SomethingWentWrongPage());

                }
            }
            IsRefreshing = false;
        }


        private List<RotatorModel> imageCollection = new List<RotatorModel>();
        public List<RotatorModel> ImageCollection
        {
            get { return imageCollection; }
            set { imageCollection = value; }
        }
        async private void Refresh(object obj)
        {
            IsRefreshing = true;
            TotalBalance = 0;
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
               await Application.Current.MainPage.Navigation.PushAsync(new Views.ErrorAndEmpty.NoInternetConnectionPage());

            }
            else {
                await GetRequestVWCMin("TVWCMin", SmiqsICCID);
                await GetRequestVWCMax("TVWCMax", SmiqsICCID);
                //Item source which needs to be displayed on the list view.
                EventsTelemtries = new ObservableCollection<tblEvents>();

              // await InitiateDBAsync(SmiqsICCID, Sensor);
                //Get Telemetries
                var EventHighestTelemetry = App.EventsDatabase.getLastReadingDate(SmiqsICCID, Sensor);

               

               
                days = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                today= new string[] { "3", "6", "9", "12", "15", "18", "21","24" };
                weeks = new string[] { "Week 1", "Week 2", "Week 3", "Week 4" };
                months = new string[] { "Jan", "Mar", "May", "Jul", "Sep", "Nov" };
                years = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep","Oct","Nov","Dec" };



                //Calculates Balance per Month
                CalculateTotalCostPerMonth(SmiqsICCID, Sensor);

                //Updates the recent irrigation time
                UpdateDeviceStatus(SmiqsICCID, Sensor);

                //Populates the list of all items
               PopulateListViewData(SmiqsICCID, Sensor);

                //Get latest temp and weather status


                await UpdateListViewDataAsync(Sensor);

                ChartData = new ObservableCollection<TransactionChartData>();
                DataSource = new ObservableCollection<TelemetryEventsModel>()
            {
                new TelemetryEventsModel(){ Duration = "Now" },
                new TelemetryEventsModel(){ Duration = "Today" },
                new TelemetryEventsModel(){ Duration = "Week" },
                new TelemetryEventsModel(){ Duration = "Month" },
                new TelemetryEventsModel(){ Duration = "Year" },
            };
                ListItems = DayListItems;
                Status_IsVisible = false;
                RealTemperature = EventHighestTelemetry.Result.GlobalGT + " °C";
                RealStatus = EventHighestTelemetry.Result.GlobalWeatherDescription.ToString();



                if(Sensor == "VA") { 
                await GetRequest("SSA", SmiqsICCID);
                await GetIrrigationStatus("SSA", SmiqsICCID);
                }
                if (Sensor == "VB")
                {
                    await GetRequest("SSA", SmiqsICCID);

                    await GetIrrigationStatus("SSA", SmiqsICCID);
                }
                if (Sensor == "VC")
                {
                    await GetRequest("SSA", SmiqsICCID);

                    await GetIrrigationStatus("SSA", SmiqsICCID);

                }
                if (Sensor == "VD")
                {
                    await GetRequest("SSA", SmiqsICCID);

                    await GetIrrigationStatus("SSA", SmiqsICCID);
                }
               
               
            }
            IsRefreshing = false;
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
                var FullEvents_Results = from mytable in EventsTelemtries
                                         where
                                         mytable.DeviceID == SmiqsICCID && mytable.valveName.Contains(sensor)
                                         select mytable;
                List<tblEvents> FullEvents = FullEvents_Results.ToList();

                foreach (var T in FullEvents)
                    await App.EventsDatabase.SaveTelemetryAsync(T);
            }
            else
            {
                var DateRange_result = from mytable in EventsTelemtries
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
        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                  //  stackLayout.Children.Add(msg);
            });
        }
        void RaisePropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the my wallet items collection in a day.
        /// </summary>
        /// 
        public bool IsRefreshing
        {
            get
            {
                return this.isrefreshing;
            }
            set
            {
                this.isrefreshing = value;
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
        public string FeedbackColor
        {
            get
            {
                return this.feedbackcolor;
            }
            set
            {
                this.feedbackcolor = value;
                this.NotifyPropertyChanged();
            }
        }
        public string LastIrrigationDuration
        {
            get
            {
                return this.lastirrigationduration;
            }
            set
            {
                this.lastirrigationduration = value;
                this.NotifyPropertyChanged();
            }
        }

        public string GardenName
        {
            get
            {
                return this.gardenname;
            }
            set
            {
                this.gardenname = value;
                this.NotifyPropertyChanged();
            }
        }
        public ObservableCollection<tblEvents> DayListItems
        {
            get
            {
                return this.dayListItems;
            }

            set
            {
                this.dayListItems = value;
                this.NotifyPropertyChanged();
            }
        }

    
        /// <summary>
        /// Gets or sets the my wallet items collection in a week.
        /// </summary>
        public ObservableCollection<tblEvents> WeekListItems
        {
            get
            {
                return this.weekListItems;
            }

            set
            {
                this.weekListItems = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the my wallet items collection in a month.
        /// </summary>
        public ObservableCollection<tblEvents> MonthListItems
        {
            get
            {
                return this.monthListItems;
            }

            set
            {
                this.monthListItems = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the my wallet items collection in a year.
        /// </summary>
        public ObservableCollection<tblEvents> YearListItems
        {
            get
            {
                return this.yearListItems;
            }

            set
            {
                this.yearListItems = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the my wallet items collection.
        /// </summary>
        public ObservableCollection<tblEvents> ListItems
        {
            get
            {
                return this.listItems;
            }

            set
            {
                this.listItems = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> ItemTappedCommand
        {
            get
            {
                return this.itemTappedCommand ?? (this.itemTappedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }
        public Command<object> CalTappedCommand
        {
            get
            {
                return this.calTappedCommand ?? (this.calTappedCommand = new Command<object>(this.NavigateToCalPage));
            }
        }
        /// <summary>
        /// Gets or sets the selected index of combobox.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                this.selectedIndex = value;
                this.NotifyPropertyChanged();
                UpdateListViewDataAsync(Sensor);
            }
        }

        /// <summary>
        /// Gets or sets the total balance remaining in the wallet.
        /// </summary>
        public double TotalBalance
        {
            get
            {
                return totalBalance;
            }
            set
            {
                this.totalBalance = value;
                this.NotifyPropertyChanged();
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

        public string RealTimeVWCMinValueChart
        {
            get
            {
                return realtimevwcminvaluechart;
            }
            set
            {
                this.realtimevwcminvaluechart = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealTimeVWCMaxValueChart
        {
            get
            {
                return realtimevwcmaxvaluechart;
            }
            set
            {
                this.realtimevwcmaxvaluechart = value;
                this.NotifyPropertyChanged();
            }
        }
        public string RealTimeVWCWidth
        {
            get
            {
                return realtimevwcwidth;
            }
            set
            {
                this.realtimevwcwidth = value;
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
        public bool Chart_IsVisible
        {
            get { return chart_isvisible; }
            set
            {

                chart_isvisible = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool Spline_IsVisible
        {
            get { return spline_isvisible; }
            set
            {

                spline_isvisible = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Status_IsVisible
        {
            get { return status_isvisible; }
            set
            {

                status_isvisible = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Gauge_IsVisible
        {
            get { return _Gauge_IsVisible; }
            set
            {

                _Gauge_IsVisible = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Day data collection.
        /// </summary>
        /// 


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
        public async         Task
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
                if(temp>10)
                {
                    Feedback = "Device disconnected "+ RealTimeStamp;
                    FeedbackColor = "RED";
                } 
                else
                {
                    Feedback = "Happy Garden";
                    FeedbackColor = "White";
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
                var val = IoTCentralResult.value.ToString();
                DateTime RealTimeStamptemp = IoTCentralResult.timestamp;
                RealTimeStamp = DateTime.Now.Subtract(RealTimeStamptemp.AddHours(4)).TotalMinutes.ToString();
                var temp = (int)Convert.ToDouble(RealTimeStamp);
                if(val.Equals("1"))
                {
                    if (Telemetry.Equals("VA") || Telemetry.Equals("VB") || Telemetry.Equals("VC"))
                    {
                        Status_IsVisible = true;
                        Feedback = "Device Irrigating";
                       
                    }
                   
                }
                else
                {
                    Feedback = "Happy Garden";

                }
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

        public async         Task
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
        public async         Task
GetRequestVWCMax(string Telemetry, string DeviceID)
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
        async Task<ObservableCollection<tblEvents>> GetDataFromWebAPI(string iccid,string sensor)
        {
          
            EventsTelemtries = await webAPIService.RefreshDataAsync(iccid, sensor,"Finish");
            return EventsTelemtries;
        }
        void RaisepropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateDeviceStatus(string iccid,string sensor)
        {

            try { 
            var result = App.EventsDatabase.getAllActiveTelemetry(iccid, sensor);
            var hrsduration = DateTime.UtcNow.AddHours(4).Subtract(result.Result[0].eventDate);        
            var minutes = hrsduration.TotalMinutes;
            if (minutes > 60)
                LastIrrigationDuration = "Recent Irrigation (" + Math.Round(hrsduration.TotalHours, 2).ToString() + " hrs ago)";
            else
                LastIrrigationDuration = "Recent Irrigation (" + hrsduration.Minutes.ToString() + " min ago)";
            }
            catch(Exception e)
            {
                LastIrrigationDuration = "No Recent Irrigations";
            }
         }
        private void CalculateTotalCostPerMonth(string iccid,string sensor)
        {
            try {
                var result = App.EventsDatabase.getAllActiveTelemetry(iccid,sensor);
                TotalBalance = 0;
                AllTelemetryThisMonth = new ObservableCollection<tblEvents>();
                foreach (var r in result.Result)
                {
                    if (r.eventDate.Month == DateTime.Today.Month && r.eventDate.Year == DateTime.Today.Year) {
                        if(int.Parse(r.elapsedtime) < 61)
                        {
                            r.Duration = r.elapsedtime.ToString() + " Seconds.";
                        }
                        else { 
                        r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                        }
                        // int liters = int.Parse(r.flowml) / 5;
                        float liters = (float)(int.Parse(r.elapsedtime)* 0.3);
                        r.flowL = (int)liters;
                        r.strflowL = liters + " Liters";
                        float galons = (float)(liters * 0.2641);
                        double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                        r.Cost = (float)tarrif;
                        r.Cost = (float)Math.Round(r.Cost, 2);
                        TotalBalance = Math.Round(TotalBalance + r.Cost, 2);

                        AllTelemetryThisMonth.Add(r);
                    }
                }

                TotalSpentText = MonthName(DateTime.Today.Month);
            }
            catch (Exception e)
            {
                TotalSpentText = "Error";
            }
        }
        private string MonthName(int m)
        {
            string res;
            switch (m)
            {
                case 1:
                    res = "Spent in Jan";
                    break;
                case 2:
                    res = "Spent in Feb";
                    break;
                case 3:
                    res = "Spent in Mar";
                    break;
                case 4:
                    res = "Spent in Apr";
                    break;
                case 5:
                    res = "Spent in May";
                    break;
                case 6:
                    res = "Spent in Jun";
                    break;
                case 7:
                    res = "Spent in Jul";
                    break;
                case 8:
                    res = "Spent in Aug";
                    break;
                case 9:
                    res = "Spent in Sep";
                    break;
                case 10:
                    res = "Spent in Oct";
                    break;
                case 11:
                    res = "Spent in Nov";
                    break;
                case 12:
                    res = "Spent in Dec";
                    break;
                default:
                    res = "Error";
                    break;
            }
            return res;
        }
        /// <summary>
        /// Week data collection.
        /// </summary>
        /// 
        private void PopulateListViewData(string iccid, string sensor)
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

            // var result = AllTelemetry.Where(a => a.eventDate >= startDateTime && a.eventDate <= endDateTime && a.VA == true && a.elapsedtime != 0);
            // var result = AllTelemetry.Where(a => a.VA == true && a.elapsedtime != 0).OrderByDescending(a => a.eventDate).First();
            
            //Get This month data
            var result = App.EventsDatabase.getAllActiveTelemetry(iccid,sensor);
            //var result = App.Database.getLastThreeReadings();
            ObservableCollection<tblEvents> mydayListItems = new ObservableCollection<tblEvents>();
            var ThisMonthData = result.Result.Where(a => a.eventDate.Month == DateTime.Today.Month && a.eventDate.Year == DateTime.Today.Year);
            foreach (var r in ThisMonthData)
            {
                if(int.Parse(r.elapsedtime)<60)
                {
                    r.Duration = r.elapsedtime.ToString() + " Seconds.";

                }
                else { 
                r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                }
                //  int liters = int.Parse(r.flowml) / 5;
                float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                r.flowL = (int)liters;                                                                                                                                                                                                                                                                     
                r.strflowL = liters + " Liters";
                float galons = (float)(liters * 0.2641);
                double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                r.Cost = (float)tarrif;
                r.Cost = (float)Math.Round(r.Cost, 2);
                mydayListItems.Add(r);

            }

            dayListItems = mydayListItems;
            //ListItems

        }


        private async Task UpdateListViewDataAsync(string s)
        {
            switch (SelectedIndex)
            {
                case 0:
                    //ListItems = WeekListItems;
                    UpdateNowUI();
                    xValues = days;
                    break;
                case 1:
                    UpdateSplineUI();
                    xValues = today;
                    break;
                case 2:
                    UpdateWeekMonthUI();
                    xValues = weeks;
                    break;
                case 3:
                    UpdateWeekMonthUI();
                    xValues = months;
                    break;
                case 4:
                    UpdateWeekMonthUI();
                    xValues = years;
                    break;
                default:
                    break;
            }
            await UpdateChartDataAsync(SmiqsICCID, s);
        }

        private void UpdateNowUI()
        {
            Gauge_IsVisible = true;
            Chart_IsVisible = false;
            Spline_IsVisible = false;
        }
        private void UpdateWeekMonthUI()
        {
            Gauge_IsVisible = false;
            Spline_IsVisible = false;
            Chart_IsVisible = true;

        }
        private void UpdateSplineUI()
        {
            Gauge_IsVisible = false;
            Spline_IsVisible = true;
            Chart_IsVisible = false;

        }
        /// <summary>
        /// 
        /// 
        /// Method for update the chart data.
        /// </summary>
        private async Task UpdateChartDataAsync(string iccid,string Sensor)
        {
            try {
                ChartData.Clear();
               
                switch (SelectedIndex)
                {
                    case 0:

                        ListItems.Clear();
                        DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                        DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                        float cost = 0;
                        // var result = AllTelemetry.Where(a => a.eventDate >= startDateTime && a.eventDate <= endDateTime && a.VA == true && a.elapsedtime != 0);
                        // var result = AllTelemetry.Where(a => a.VA == true && a.elapsedtime != 0).OrderByDescending(a => a.eventDate).First();

                        //Get This month data
                        var result = App.EventsDatabase.getAllActiveTelemetry(iccid, Sensor);
                        //var result = App.Database.getLastThreeReadings();
                        ObservableCollection<tblEvents> mydayListItems = new ObservableCollection<tblEvents>();
                        var ThisMonthData = result.Result.Where(a => a.eventDate.Month == DateTime.Today.Month).OrderByDescending(a => a.eventDate).Take(4);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            mydayListItems.Add(r);

                        }

                        dayListItems = mydayListItems;
                        ListItems = DayListItems;

                        break;
                    case 1:
                        webAPIService = new WebAPIService();
                        await GetDataFromWebAPI(SmiqsICCID, Sensor);

                       var myTempTelemetries = await webAPIService.GetTelemetriesSmiqs(iccid);
                        Data = new ObservableCollection<splinedata>();
                        IrrigationData = new ObservableCollection<splinedata>();
                        var IrrigationDataGrouped = EventsTelemtries;
                        if (Sensor == "VA")
                        {
                           var zz = myTempTelemetries.Where(f => f.SSA != null).GroupBy(s => s.eventDate.Ticks / TimeSpan.FromHours(3).Ticks)
                             .Select(s => new {
                                 series = s
                                 ,
                                 timestamp = s.First().eventDate
                                 ,
                                 max = s.Max(x => float.Parse(x.SSA))
                                 ,
                                 min=s.Min(x=> float.Parse(x.SSA))
                                 ,
                                 elt = s.Where(x=> x.elapsedtime !=null)
                            }).OrderBy(s => s.timestamp);
                            float maxTempTelemetries = float.Parse(myTempTelemetries.Select(x => x.SSA).Max());
                            float minTempTelemetries = float.Parse(myTempTelemetries.Select(x => x.SSA).Min());
                            maxTempTelemetries += 1;
                            minTempTelemetries -= 1;
                            int maxTemplTelemetrieInt = (int)maxTempTelemetries;
                            int minTempTelemetriesInt = (int)minTempTelemetries;
                            RealTimeVWCMaxValueChart = maxTemplTelemetrieInt.ToString();
                            RealTimeVWCMinValueChart = minTempTelemetriesInt.ToString();
                            int diffe = int.Parse(RealTimeVWCMaxValue) - int.Parse(RealTimeVWCMinValue);
                            RealTimeVWCWidth = diffe.ToString();
                            foreach (var si in zz)
                            {
                              
                                Data.Add(new splinedata(si.timestamp.ToShortTimeString(), si.max));
                                Data.Add(new splinedata(si.timestamp.ToShortTimeString(), double.Parse(si.elt.First().elapsedtime)));
                                // Data.Add(new ChartDataModel("Sun", 15));
                            }
                            foreach (var igd in IrrigationDataGrouped.Where(dz => dz.eventDate > DateTime.Today.AddDays(-1).AddTicks(-1)).OrderBy(s => s.timestamp))
                            {
                           //     IrrigationData.Add(new splinedata(igd.eventDate.ToShortTimeString(), double.Parse(igd.SSA)));
                            }
                        }
                        if (Sensor == "VB")
                        {
                            var zb = myTempTelemetries.Where(f => f.SSB != null).GroupBy(s => s.eventDate.Ticks / TimeSpan.FromHours(3).Ticks)
                            .Select(s => new {
                                series = s
                                ,
                                timestamp = s.First().eventDate
                                ,
                                max = s.Max(x => float.Parse(x.SSB))
                            }).OrderBy(s => s.timestamp);
                            int maxTempTelemetries = int.Parse(myTempTelemetries.Select(x => x.SSB).Max()) + 5;
                            int minTempTelemetries = int.Parse(myTempTelemetries.Select(x => x.SSB).Min()) - 5;
                            RealTimeVWCMaxValueChart = maxTempTelemetries.ToString();
                            RealTimeVWCMinValueChart = minTempTelemetries.ToString();
                            foreach (var si in zb)
                            {

                                 Data.Add(new splinedata(si.timestamp.ToShortTimeString(), si.max));
                               // Data.Add(new ChartDataModel("Sun", 15));
                            }

                            foreach (var igd in IrrigationDataGrouped.Where(dz => dz.timestamp > DateTime.Today.AddDays(-1).AddTicks(-1)))
                            {
                                IrrigationData.Add(new splinedata(igd.timestamp.ToShortTimeString(), double.Parse(igd.SSA)));
                            }
                        }
                        if (Sensor == "VC")
                        {
                            
                            var zc = myTempTelemetries.Where(f => f.SSC != null).GroupBy(s => s.eventDate.Ticks / TimeSpan.FromHours(3).Ticks)
                            .Select(s => new {
                                series = s
                                ,
                                timestamp = s.First().eventDate
                                ,
                                max = s.Max(x => float.Parse(x.SSC))
                            }).OrderBy(s => s.timestamp);
                            int maxTempTelemetries = int.Parse(myTempTelemetries.Select(x => x.SSC).Max()) + 5;
                            int minTempTelemetries = int.Parse(myTempTelemetries.Select(x => x.SSC).Min()) - 5;
                            RealTimeVWCMaxValueChart = maxTempTelemetries.ToString();
                            RealTimeVWCMinValueChart = minTempTelemetries.ToString();
                            foreach (var si in zc)
                            {

                                 Data.Add(new splinedata(si.timestamp.ToShortTimeString(), si.max));
                              //  Data.Add(new ChartDataModel("Sun", 15));
                            }
                            foreach (var igd in IrrigationDataGrouped.Where(dz => dz.timestamp > DateTime.Today.AddDays(-1).AddTicks(-1)))
                            {
                                IrrigationData.Add(new splinedata(igd.timestamp.ToShortTimeString(), double.Parse(igd.SSA)));
                            }
                        }
                        if (Sensor == "VD")
                        {
                            
                            var zd = myTempTelemetries.Where(f => f.SSD != null).GroupBy(s => s.eventDate.Ticks / TimeSpan.FromHours(3).Ticks)
                            .Select(s => new
                            {
                                series = s
                                ,
                                timestamp = s.First().eventDate
                                ,
                                max = s.Max(x => float.Parse(x.SSD))
                            }).OrderBy(s => s.timestamp);
                            int maxTempTelemetries = int.Parse(myTempTelemetries.Select(x => x.SSD).Max()) + 5;
                            int minTempTelemetries = int.Parse(myTempTelemetries.Select(x => x.SSD).Min()) - 5;
                            RealTimeVWCMaxValueChart = maxTempTelemetries.ToString();
                            RealTimeVWCMinValueChart = minTempTelemetries.ToString();
                            Data = new ObservableCollection<splinedata>();
                            foreach (var si in zd)
                            {

                                 Data.Add(new splinedata(si.timestamp.ToShortTimeString(), si.max));
                              //  Data.Add(new ChartDataModel("Sun", 15));
                            }
                            foreach (var igd in IrrigationDataGrouped.Where(dz => dz.timestamp > DateTime.Today.AddDays(-1).AddTicks(-1)))
                            {
                                IrrigationData.Add(new splinedata(igd.timestamp.ToShortTimeString(), double.Parse(igd.SSA)));
                            }
                        }
                       
                      
                       
                        break;
                    case 2:
                        var TodayDate = DateTime.Now;
                        var SevenDays = DateTime.Now.AddDays(-7);
                        var SixDays = DateTime.Now.AddDays(-6);
                        var FiveDays = DateTime.Now.AddDays(-5);
                        var FourDays = DateTime.Now.AddDays(-4);
                        var ThreeDays = DateTime.Now.AddDays(-3);
                        var TwoDays = DateTime.Now.AddDays(-2);
                        var OneDay = DateTime.Now.AddDays(-1);

                        ListItems.Clear();
                        TodayDate = DateTime.Now;

                        startDateTime = DateTime.Today; //Today at 00:00:00
                        endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                        ObservableCollection<tblEvents> myweekListItems = new ObservableCollection<tblEvents>();
                        cost = 0;

                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == SixDays.Date.Month && a.eventDate.Day == SixDays.Date.Day && a.eventDate.Year == SixDays.Date.Year).OrderByDescending(a => a.eventDate);                        
                        
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);                           
                        }                        
                        ChartData.Add(new TransactionChartData(SixDays.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == FiveDays.Date.Month && a.eventDate.Day == FiveDays.Date.Day && a.eventDate.Year == FiveDays.Date.Year).OrderByDescending(a => a.eventDate);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);

                        }
                        ChartData.Add(new TransactionChartData(FiveDays.Date.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == FourDays.Date.Month && a.eventDate.Day == FourDays.Date.Day && a.eventDate.Year == FourDays.Date.Year).OrderByDescending(a => a.eventDate);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);

                        }
                        ChartData.Add(new TransactionChartData(FourDays.Date.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == ThreeDays.Date.Month && a.eventDate.Day == ThreeDays.Date.Day && a.eventDate.Year == ThreeDays.Date.Year).OrderByDescending(a => a.eventDate);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);

                        }
                        ChartData.Add(new TransactionChartData(ThreeDays.Date.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == TwoDays.Date.Month && a.eventDate.Day == TwoDays.Date.Day && a.eventDate.Year == TwoDays.Date.Year).OrderByDescending(a => a.eventDate);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);

                        }
                        ChartData.Add(new TransactionChartData(TwoDays.Date.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == OneDay.Date.Month && a.eventDate.Day == OneDay.Date.Day && a.eventDate.Year == OneDay.Date.Year).OrderByDescending(a => a.eventDate);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);

                        }
                        ChartData.Add(new TransactionChartData(OneDay.Date.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.eventDate.Month == DateTime.Today.Month && a.eventDate.Day == DateTime.Today.Day && a.eventDate.Year == DateTime.Today.Year).OrderByDescending(a => a.eventDate);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                            //  int liters = int.Parse(r.flowml) / 5;
                            float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                            r.flowL = (int)liters;
                            r.strflowL = liters + " Liters";
                            float galons = (float)(liters * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            r.Cost = (float)tarrif;
                            r.Cost = (float)Math.Round(r.Cost, 2);
                            cost += r.Cost;
                            myweekListItems.Add(r);

                        }
                        ChartData.Add(new TransactionChartData(TodayDate.Date.ToString("ddd", new CultureInfo("en-US")), cost, ThisMonthData.Count(), 4));


                        weekListItems = myweekListItems;
                        ListItems = WeekListItems;

                        break;
                    case 3:

                        ObservableCollection<tblEvents> mymonthListItems = new ObservableCollection<tblEvents>();
                        float monthlyCost = 0;

                        var OneMonth = DateTime.Now;
                        int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        int week1 = days / 4;
                        DateTime reference = DateTime.Now;
                        var calendar = CultureInfo.CurrentCulture.Calendar;
                        IEnumerable<int> daysInMonth = Enumerable.Range(1, calendar.GetDaysInMonth(reference.Year, reference.Month));
                        List<Tuple<DateTime, DateTime>> weeks = daysInMonth.Select(day => new DateTime(reference.Year, reference.Month, day))
                        .GroupBy(d => calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday))
                        .Select(g => new Tuple<DateTime, DateTime>(g.First(), g.Last()))
                        .ToList();
                        string weekstr = "Week ";
                        int weekint = 1;
                        weeks.ForEach(x => Console.WriteLine("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}", x.Item1, x.Item2));
                        foreach(var wee in weeks)
                        {
                            
                            var resultOnMonthly = AllTelemetryThisMonth.Where(a => a.eventDate.Date >= wee.Item1.Date && a.eventDate.Date <= wee.Item2.Date).OrderByDescending(a => a.eventDate);
                            
                            foreach (var r in resultOnMonthly)
                            {
                                r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                                //  int liters = int.Parse(r.flowml) / 5;
                                float liters = (float)(int.Parse(r.elapsedtime) * 0.3);
                                r.flowL = (int)liters;
                                r.strflowL = liters + " Liters";
                                float galons = (float)(liters * 0.2641);
                                double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                                r.Cost = (float)tarrif;
                                r.Cost = (float)Math.Round(r.Cost, 2);
                                monthlyCost += r.Cost;
                                mymonthListItems.Add(r);

                            }
                            weekstr = weekstr + weekint.ToString();
                            weekint++;
                            // ChartData.Add(new TransactionChartData(OneMonth.ToString("m", new CultureInfo("en-US")), monthlyCost, resultOnMonthly.Count(), 4));
                            ChartData.Add(new TransactionChartData(weekstr, monthlyCost, resultOnMonthly.Count(), 4));
                            monthlyCost = 0;
                            weekstr = "Week ";
                        }
                       
                       
                        monthListItems = mymonthListItems;
                        ListItems = MonthListItems;
                        break;
                    
                    case 4:
                        var yearresult = App.EventsDatabase.getAllActiveTelemetry(iccid, Sensor);
                     
                        
                        var data = yearresult.Result.Select(k => new { k.eventDate.Year, k.eventDate.Month, k.elapsedtime }).GroupBy(x => new { x.Year, x.Month }, (key, group) => new
                        {
                            yr = key.Year,
                            mnth = key.Month,
                            tCharge = group.Sum(k => int.Parse(k.elapsedtime) * 0.3)
                        }).ToList();
                        foreach (var grp in data)
                        {
                           
                            float galons = (float)(grp.tCharge * 0.2641);
                            double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                            double Cost = (float)tarrif;
                            Cost = (double)Math.Round(Cost, 2);
                           
                            ChartData.Add(new TransactionChartData(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(grp.mnth).ToString(), Cost,0, 4));

                        }
                        
                        break;
                    
                    default:
                    break;
                }
              
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Invoked when an item is selected from the my wallet page.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private void NavigateToNextPage(object selectedItem)
        {
            // Do something

         //   Syncfusion.ListView.XForms.ItemTappedEventArgs x = (Syncfusion.ListView.XForms.ItemTappedEventArgs)selectedItem;
         //   TelemetryEventsModel T = (TelemetryEventsModel)x.ItemData;
         //   Application.Current.MainPage.Navigation.PushAsync(new MyAddressPage(T));
        }
        private async void NavigateToCalPage(object selectedItem)
        {
            // Do something

            await Application.Current.MainPage.Navigation.PushAsync(new AllIrrigationsPage(SmiqsICCID, Sensor));
        }
        #endregion
    }
}