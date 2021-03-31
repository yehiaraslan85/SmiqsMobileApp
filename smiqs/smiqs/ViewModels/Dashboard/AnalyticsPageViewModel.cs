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

namespace smiqs.ViewModels.Dashboard
{
    [Preserve(AllMembers = true)]

    public class AnalyticsPageViewModel : BaseViewModel
    {
        #region Fields

        private int selectedIndex;

        private double totalBalance;
        private string lastirrigationduration;
        private bool isrefreshing;
        public ICommand RefreshCommand { get; set; }

        private string[] days, weeks, months, xValues;

        bool _Chart_IsVisible;
        bool _Gauge_IsVisible;

        private ObservableCollection<Telemetry> dayListItems;

        private ObservableCollection<Telemetry> weekListItems;

        private ObservableCollection<Telemetry> monthListItems;

        private ObservableCollection<Telemetry> yearListItems;

        private ObservableCollection<Telemetry> listItems;

        public ObservableCollection<TransactionChartData> ChartData { get; set; }

        public ObservableCollection<Telemetry> DataSource { get; set; }

        private Command<object> itemTappedCommand;
        private Command<object> calTappedCommand;
        public ObservableCollection<Telemetry> Telemtries { get; private set; }


        public string queryReadUser = "SELECT * FROM dbo.tblUsers where firbaseUID LIKE @fbuserID";
        public string queryReadDevice = "SELECT * FROM dbo.tblDevices where userID LIKE @ID";
        public string queryReadTelemetry_All = "SELECT * FROM dbo.tblSmiqsTelemetry where DeviceID LIKE @DevID order by telId";
        public string queryReadTelemetry_dt = "SELECT * FROM dbo.tblSmiqsTelemetry where DeviceID LIKE @DevID and teltimestamp BETWEEN @starttime and @endtime";

        public string queryReadTelemetry_On = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID and VA = 1";
        public string queryReadTelemetry_Off = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID and VA = 0";
        public string queryReadDevices = "SELECT * FROM dbo.tblDevices where U";
        public static ObservableCollection<Telemetry> AllTelemetry;
        public static ObservableCollection<Telemetry> AllTelemetryThisMonth;
        public static List<Models.Device> AllDevices;

        public string SmiqsFirebaseUID { get; set; }
        public string SmiqsUID { get; set; }
        public string SmiqsICCID { get; set; }
        public string SmiqsCommandName { get; set; }
        public string SmiqsAuthorization { get; set; }
        public string SmiqsComponentName { get; set; }
        public string realtemperature { get; set; }

        public string realstatus { get; set; }
        public string realtimevalue { get; set; }
        public string realtimevwcminvalue { get; set; }
        public string realtimevwcmaxvalue { get; set; }
        #endregion
        public string realtimestamp { get; set; }
        public string totalspenttext { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="MyWalletViewModel" /> class.
        /// </summary>
        /// 
        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();

        public AnalyticsPageViewModel()
        {

            days = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            weeks = new string[] { "Week 1", "Week 2", "Week 3", "Week 4" };
            months = new string[] { "Jan", "Mar", "May", "Jul", "Sep", "Nov" };



            ChartData = new ObservableCollection<TransactionChartData>();
            DataSource = new ObservableCollection<Telemetry>()
            {
                new Telemetry(){ Duration = "Today" },
                new Telemetry(){ Duration = "Week" },
                new Telemetry(){ Duration = "Month" },

            };
            var TodayDate = DateTime.Now;
            var SevenDays = DateTime.Now.AddDays(-7);
            var SixDays = DateTime.Now.AddDays(-6);
            var FiveDays = DateTime.Now.AddDays(-5);
            var FourDays = DateTime.Now.AddDays(-4);
            var ThreeDays = DateTime.Now.AddDays(-3);
            var TwoDays = DateTime.Now.AddDays(-2);
            var OneDay = DateTime.Now.AddDays(-1);

            //ListItems.Clear();
            TodayDate = DateTime.Now;

            var startDateTime = DateTime.Today; //Today at 00:00:00
            var endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            ObservableCollection<Telemetry> myweekListItems = new ObservableCollection<Telemetry>();
            float cost = 0;


            var ThisMonthData = AllTelemetry.Where(a => a.teltimestamp.Month == ThreeDays.Date.Month && a.teltimestamp.Day >= ThreeDays.Date.Day && a.teltimestamp.Year == ThreeDays.Date.Year).OrderByDescending(a => a.telID);

            foreach (var r in ThisMonthData)
            {
                r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                float liters = r.flowml / 5;
                r.flowL = liters;
                r.strflowL = liters + " Liters";
                float galons = (float)(liters * 0.2641);
                double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                r.Cost = (float)tarrif;
                r.Cost = (float)Math.Round(r.Cost, 2);
                cost = r.SSA;
                myweekListItems.Add(r);
                ChartData.Add(new TransactionChartData(r.teltimestamp.Date.ToString(), r.SSA, 0, 4));

            }
        }
        async private void Refresh(object obj)
        {
            IsRefreshing = true;
            TotalBalance = 0;
            await GetRequest("SSA", SmiqsICCID);
            await GetRequestVWCMin("TVWCMin", SmiqsICCID);
            await GetRequestVWCMax("TVWCMax", SmiqsICCID);

            UpdateListViewData();

            //Calculates Balance per Month
            CalculateTotalCostPerMonth();

            //Updates the recent irrigation time
            UpdateDeviceStatus();

            //Populates the list of all items
            PopulateListViewData();

            try
            {
                //Get latest temp and weather status
                RealTemperature = Telemtries[0].GlobalGT.ToString();
                RealStatus = Telemtries[0].GlobalWeatherDescription.ToString();
            }
            catch (Exception es)
            {
                RealTemperature = "Error";
                RealStatus = "Error";
            }


            IsRefreshing = false;
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
        public event PropertyChangedEventHandler PropertyChanged;
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


        public ObservableCollection<Telemetry> DayListItems
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
        public ObservableCollection<Telemetry> WeekListItems
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
        public ObservableCollection<Telemetry> MonthListItems
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
        public ObservableCollection<Telemetry> YearListItems
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
        public ObservableCollection<Telemetry> ListItems
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
                UpdateListViewData();
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
            get { return _Chart_IsVisible; }
            set
            {

                _Chart_IsVisible = value;
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
                    RealTimeStamp = "Device disconnected";
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
        public async Task
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

        private void UpdateDeviceStatus()
        {

            try
            {
                var result = App.Database.getAllActiveTelemetry();
                var hrsduration = DateTime.UtcNow.AddHours(4).Subtract(result.Result[0].teltimestamp);
                var minutes = hrsduration.TotalMinutes;
                if (minutes > 60)
                    LastIrrigationDuration = "Recent Irrigation (" + hrsduration.Hours.ToString() + " hrs ago)";
                else
                    LastIrrigationDuration = "Recent Irrigation (" + hrsduration.Minutes.ToString() + " min ago)";
            }
            catch (Exception e)
            {
                LastIrrigationDuration = "No Recent Irrigations";
            }
        }
        private void CalculateTotalCostPerMonth()
        {
            try
            {
                var result = App.Database.getAllActiveTelemetry();
                TotalBalance = 0;
                AllTelemetryThisMonth = new ObservableCollection<Telemetry>();
                foreach (var r in result.Result)
                {
                    if (r.teltimestamp.Month == DateTime.Today.Month && r.teltimestamp.Year == DateTime.Today.Year)
                    {
                        r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                        float liters = r.flowml / 5;
                        r.flowL = liters;
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
                TotalSpentText = "";
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
        private void PopulateListViewData()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

            // var result = AllTelemetry.Where(a => a.timestamp >= startDateTime && a.timestamp <= endDateTime && a.VA == true && a.elapsedtime != 0);
            // var result = AllTelemetry.Where(a => a.VA == true && a.elapsedtime != 0).OrderByDescending(a => a.timestamp).First();

            //Get This month data
            var result = App.Database.getAllActiveTelemetry();
            //var result = App.Database.getLastThreeReadings();
            ObservableCollection<Telemetry> mydayListItems = new ObservableCollection<Telemetry>();
            var ThisMonthData = result.Result.Where(a => a.teltimestamp.Month == DateTime.Today.Month);
            foreach (var r in ThisMonthData)
            {
                r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                float liters = r.flowml / 5;
                r.flowL = liters;
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


    

      

        /// <summary>
        /// Method for update the listview items.
        /// </summary>
        private void UpdateListViewData()
        {
            switch (SelectedIndex)
            {
                case 0:
                    //ListItems = WeekListItems;

                    UpdateNowUI();
                    xValues = days;
                    break;
                case 1:
                    UpdateWeekMonthUI();
                    xValues = weeks;
                    break;
                case 2:
                    UpdateWeekMonthUI();
                    xValues = months;
                    break;
                default:
                    break;
            }
            UpdateChartData();
        }

        private void UpdateNowUI()
        {
            Gauge_IsVisible = true;
            Chart_IsVisible = false;
        }
        private void UpdateWeekMonthUI()
        {
            Gauge_IsVisible = false;
            Chart_IsVisible = true;
        }
        /// <summary>
        /// 
        /// 
        /// Method for update the chart data.
        /// </summary>
        private void UpdateChartData()
        {
            try
            {
                ChartData.Clear();

                switch (SelectedIndex)
                {
                    case 0:

                        ListItems.Clear();
                        DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                        DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                        float cost = 0;
                        // var result = AllTelemetry.Where(a => a.timestamp >= startDateTime && a.timestamp <= endDateTime && a.VA == true && a.elapsedtime != 0);
                        // var result = AllTelemetry.Where(a => a.VA == true && a.elapsedtime != 0).OrderByDescending(a => a.timestamp).First();

                        //Get This month data
                        var result = App.Database.getAllActiveTelemetry();
                        //var result = App.Database.getLastThreeReadings();
                        ObservableCollection<Telemetry> mydayListItems = new ObservableCollection<Telemetry>();
                        var ThisMonthData = result.Result.Where(a => a.teltimestamp.Month == DateTime.Today.Month).OrderByDescending(a => a.teltimestamp).Take(4);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ObservableCollection<Telemetry> myweekListItems = new ObservableCollection<Telemetry>();
                        cost = 0;
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == SixDays.Date.Month && a.teltimestamp.Day == SixDays.Date.Day && a.teltimestamp.Year == SixDays.Date.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == FiveDays.Date.Month && a.teltimestamp.Day == FiveDays.Date.Day && a.teltimestamp.Year == FiveDays.Date.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == FourDays.Date.Month && a.teltimestamp.Day == FourDays.Date.Day && a.teltimestamp.Year == FourDays.Date.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == ThreeDays.Date.Month && a.teltimestamp.Day == ThreeDays.Date.Day && a.teltimestamp.Year == ThreeDays.Date.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == TwoDays.Date.Month && a.teltimestamp.Day == TwoDays.Date.Day && a.teltimestamp.Year == TwoDays.Date.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == OneDay.Date.Month && a.teltimestamp.Day == OneDay.Date.Day && a.teltimestamp.Year == OneDay.Date.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                        ThisMonthData = AllTelemetryThisMonth.Where(a => a.teltimestamp.Month == DateTime.Today.Month && a.teltimestamp.Day == DateTime.Today.Day && a.teltimestamp.Year == DateTime.Today.Year).OrderByDescending(a => a.teltimestamp);
                        foreach (var r in ThisMonthData)
                        {
                            r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                            float liters = r.flowml / 5;
                            r.flowL = liters;
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
                    case 2:

                        ObservableCollection<Telemetry> mymonthListItems = new ObservableCollection<Telemetry>();
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
                        foreach (var wee in weeks)
                        {

                            var resultOnMonthly = AllTelemetryThisMonth.Where(a => a.teltimestamp.Date >= wee.Item1.Date && a.teltimestamp.Date <= wee.Item2.Date).OrderByDescending(a => a.teltimestamp);

                            foreach (var r in resultOnMonthly)
                            {
                                r.Duration = (r.elapsedtime / 60).ToString() + " Minutes.";
                                float liters = r.flowml / 5;
                                r.flowL = liters;
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

                        /* var TwoMonth = DateTime.Now.AddMonths(-2);
                         var ThreeMonth = DateTime.Now.AddMonths(-3);
                         var FourMonth = DateTime.Now.AddMonths(-4);
                         var FiveMonths = DateTime.Now.AddMonths(-5);
                         var SixMonths = DateTime.Now.AddMonths(-6);
                         var SevenMonth = DateTime.Now.AddMonths(-7);
                         var EightMonth = DateTime.Now.AddMonths(-8);
                         var NineMonth = DateTime.Now.AddMonths(-9);
                         var TenMonth = DateTime.Now.AddMonths(-10);
                         var ElevenMonth = DateTime.Now.AddMonths(-11);
                         var TwelveMonth = DateTime.Now.AddMonths(-12);
                         var resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == TwelveMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(TwelveMonth.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == ElevenMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(ElevenMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == TenMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(TenMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == NineMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(NineMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == EightMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(EightMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == SevenMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(SevenMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == SixMonths.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(SixMonths.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == FiveMonths.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(FiveMonths.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == FourMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(FourMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == ThreeMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(ThreeMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == TwoMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(TwoMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == OneMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                         if (resultOnMonthly.Count() > 0)
                             ChartData.Add(new TransactionChartData(OneMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                        // resultOnMonthly = AllTelemetry.Where(a => a.teltimestamp.Date.Month == startDateTimeMonth.Date.Month && a.VA == true && a.elapsedtime != 0);
                        // if (resultOnMonthly.Count() > 0)
                         //    ChartData.Add(new TransactionChartData(startDateTimeMonth.Date.ToString("m", new CultureInfo("en-US")), 0, resultOnMonthly.Count(), 4));
                         */

                        break;
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                var s = e.Message.ToString();
            }
        }

        /// <summary>
        /// Invoked when an item is selected from the my wallet page.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private void NavigateToNextPage(object selectedItem)
        {
            // Do something

            Syncfusion.ListView.XForms.ItemTappedEventArgs x = (Syncfusion.ListView.XForms.ItemTappedEventArgs)selectedItem;
            TelemetryEventsModel T = (TelemetryEventsModel)x.ItemData;
            Application.Current.MainPage.Navigation.PushAsync(new MyAddressPage(T));
        }
        private void NavigateToCalPage(object selectedItem)
        {
            // Do something

            
        }
        #endregion
    }
}