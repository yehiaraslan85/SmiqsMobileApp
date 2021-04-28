using Xamarin.Forms;
using System.Collections.ObjectModel;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms.Internals;
using Model = smiqs.Models.Article;
using smiqs.Models;
using smiqs.Views.Dashboard;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using static smiqs.Views.MainMenu;
using Newtonsoft.Json;
using System.Net;
using smiqs.Helper;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using smiqs.ViewModels.Helpers;
using System.Windows.Input;

namespace smiqs.ViewModels.Catalog
{
    /// <summary>
    /// ViewModel for Article card type page.
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class ArticleCardViewModel : BaseViewModel
    {
        #region Fields
        WebAPIService webAPIService;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<tblEvents> eventsTelemtries { get; private set; }
        private ObservableCollection<tblEvents> items;
        public static ObservableCollection<tblEvents> AllTelemetryThisMonth;
        public string username { get; set; }
        public string userimage { get; set; }
        private bool isrefreshing;
        private bool isloading;
        private string feedback;
        private string feedbackcolor;
        private double totalBalance;
        private string lastirrigationduration;
        public bool isconnected { get; set; }
        public bool isdisconnected { get; set; }
        public string realtemperature { get; set; }
        public string realstatus { get; set; }
        public string weatherstatus { get; set; }
        public string temperaturerstatus { get; set; }
        public string humiditystatus { get; set; }
        public string windstatus { get; set; }
        public string realtimevalue { get; set; }
        public string totalspenttext { get; set; }
        public ObservableCollection<Model> articles { get; set; }
        public string realtimestamp { get; set; }
        
        public List<Models.Device> DevResult = new List<Models.Device>();
        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets a collction of value to be displayed in articles card page.
        /// </summary>
        public ObservableCollection<Model> Articles
        {
            get
            {
                return articles;
            }
            set
            {
                this.articles = value;
                this.NotifyPropertyChanged();
            }
        }

        private bool _isStopVisible;

        public bool IsStopVisible
        {
            get
            {
                return _isStopVisible;
            }
            set
            {
                _isStopVisible = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return isconnected; }
            set
            {

                if (isconnected != value)
                {
                    isconnected = value;
                    this.NotifyPropertyChanged();


                }
            }
        }
        public bool IsDisconnected
        {
            get { return isdisconnected; }
            set
            {

                isdisconnected = value;
                this.NotifyPropertyChanged();

            }
        }
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
        public string RealStatus
        {
            get
            {
                return this.realstatus;
            }
            set
            {
                this.realstatus = value;
                this.NotifyPropertyChanged();
            }
        }
        public string WaetherStatus
        {
            get
            {
                return this.weatherstatus;
            }
            set
            {
                this.weatherstatus = value;
                this.NotifyPropertyChanged();
            }
        }
        public string TemperatureStatus
        {
            get
            {
                return this.temperaturerstatus;
            }
            set
            {
                this.temperaturerstatus = value;
                this.NotifyPropertyChanged();
            }
        }
        public string HumidityStatus
        {
            get
            {
                return this.humiditystatus;
            }
            set
            {
                this.humiditystatus = value;
                this.NotifyPropertyChanged();
            }
        }
        public string WindStatus
        {
            get
            {
                return this.windstatus;
            }
            set
            {
                this.windstatus = value;
                this.NotifyPropertyChanged();
            }
        }
        
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

        public ObservableCollection<tblEvents> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                this.NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>

        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                this.username = value;
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

        public string UserImage
        {
            get
            {
                return userimage;
            }
            set
            {
                this.userimage = value;
                this.NotifyPropertyChanged();
            }
        }
        public string LabelString
        {
            get { return labelString; }
            set
            {
                labelString = value;
                this.NotifyPropertyChanged();
            }
        }
     
        public string ShowDetail
        {
            get { return showdetail; }
            set
            {
                showdetail = value;
                this.NotifyPropertyChanged();
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool PopupOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        bool isOpen, visible;
        string labelString, userName, password, showdetail;
        public ICommand PopupAcceptCommand { get; set; }
        public ICommand ShowPopupCommand { get; set; }

        Command<ItemTappedEventArgs> tapCommand;
        public Command<ItemTappedEventArgs> TapCommand
        {
            get { return tapCommand; }
            protected set { tapCommand = value; }
        }
        private Command itemTappedCommand;

        #region Constructor
        public Command ItemTappedCommand
        {
            get { return itemTappedCommand; }
            protected set { itemTappedCommand = value; }
        }

        private Command edititemTappedCommand;
        public ICommand EditItemTappedCommand
        {
            get { return edititemTappedCommand; }
            set
            {
                edititemTappedCommand = (Command)value;
                this.NotifyPropertyChanged();
            }
        }
    private Command deleteitemTappedCommand;
        public Command DeleteItemTappedCommand
        {
            get { return deleteitemTappedCommand; }
            protected set { deleteitemTappedCommand = value; }
        }

        private bool displayPopup;

        public ICommand OpenPopupCommand { get; set; }

        public bool DisplayPopup
        {
            get
            {
                return displayPopup;
            }
            set
            {
                displayPopup = value;
                this.NotifyPropertyChanged();
            }
        }
        private void OpenPopup()
        {
            DisplayPopup = true;
        }
        public ArticleCardViewModel()
        {
            this.BookmarkCommand = new Command(this.BookmarkButtonClicked);
            this.AddFavouriteCommand = new Command(this.FavouriteButtonClicked);
            this.ShareCommand = new Command(this.ShareButtonClicked);
            this.ItemTappedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(NavigateToNextPage);
            EditItemTappedCommand = new Command(EditItemButtonClicked);
            this.DeleteItemTappedCommand = new Command(this.DeleteItemButtonClicked);
            PopupAcceptCommand = new Command(PopupAccept); //CanExecute() will be call the PopupAccept method
            ShowPopupCommand = new Command(Popup);  //CanExecute() will be call the Popup method.
            OpenPopupCommand = new Command(OpenPopup);

            UpdateNowUI();
            this.RefreshCommand = new Command(Refresh);
           this.IrrigateCommand = new Command(IrrigateFunction);
            PopupOpen = false;
            // object o = new object();
            // Refresh(o);
            try { 
            Articles = new ObservableCollection<Model>();
        
             /*   foreach (var device in App.ListOfDevices)
            {
                    
                    Image image = new Image();
                    Stream stream = new MemoryStream(device.ImageBytes);
                    image.Source = ImageSource.FromStream(() => { return stream; });
                    device.DeviceImageSource = image.Source;
                    
               
            }
           */
            Articles = App.ListOfDevices;
                UpdateWeatherWidget();

            }
            catch (Exception e)
            { 
            
            }
            
            IsStopVisible = true;
            isdisconnected = false;
        }
        private void Popup()
        {
            PopupOpen = true;
            LabelString = "User Login";
            Visible = true;
        }
        private void PopupAccept()
        {
            // You can write your set of codes that needs to be executed.
            if ((UserName == "Syncfusion") && (Password == "12345"))
            {
                ShowDetail = "Login Successfully...";
                UserName = "";
                Password = "";
            }
            else
            {
                ShowDetail = "Login Failed";
                UserName = "";
                Password = "";
            }
        }
        public void UpdateWeatherWidget()
        {
            /*Calling API http://openweathermap.org/api */
            string apiKey = "fafe746236c9129097ac496b36811d31";
            HttpWebRequest apiRequest =
            WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=292223" +
             "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            /*End*/
            TimeSpan day = DateTime.Now.TimeOfDay;
            bool IsDarkTheme = true;
            if (!(day.Hours < 19 && day.Hours > 6))
            {
                IsDarkTheme = false;
            }
            /*http://json2csharp.com*/
            ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);
            RealStatus = "https://openweathermap.org/img/w/" + rootObject.weather[0].icon + ".png";
            int iit = rootObject.weather[0].id;
            if (iit == 200)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 201)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 202)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 210)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 211)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 212)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 221)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 230)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 231)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 232)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-sun.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/thunderstorm-night.png";
                }
            }
            if (iit == 300)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/drizzle.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 301)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/drizzle.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 302)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 310)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/drizzle.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 312)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 313)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 314)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 321)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/drizzle.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 500)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/drizzle.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 501)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 502)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/shower-rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 503)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 504)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/shower-rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 511)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/heavy-snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 520)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/drizzle.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 521)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 522)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/shower-rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 531)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/shower-rain.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/rain-night.png";
                }
            }
            if (iit == 600)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 601)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 602)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 611)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 612)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 615)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 616)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 620)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 621)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/heavy-snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 622)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/heavy-snow.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/snow-night.png";
                }
            }
            if (iit == 701)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/mist.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/mist-night.png";
                }
            }
            if (iit == 711)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/smoke.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/dust-night.png";
                }
            }
            if (iit == 721)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/haze.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/haze-night.png";
                }
            }
            if (iit == 731)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/dust.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/dust-night.png";
                }
            }
            if (iit == 741)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/mist.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/mist-night.png";
                }
            }
            if (iit == 751)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/dust.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/dust-night.png";
                }
            }
            if (iit == 761)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/dust.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/dust-night.png";
                }
            }
            if (iit == 762)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/dust.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/dust-night.png";
                }
            }

            if (iit == 771)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/squalls.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/squalls-night.png";
                }
            }
            if (iit == 781)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/tornado.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/squalls-night.png";
                }
            }
            if (iit == 800)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/clear-sky.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/clear-sky-night.png";
                }
            }
            if (iit == 801)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/few-clouds.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/few-clouds-night.png";
                }
            }
            if (iit == 802)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/scattered-clouds.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/scattered-clouds-night.png";
                }
            }
            if (iit == 803)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/few-clouds.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/few-clouds-night.png";
                }
            }
            if (iit == 804)
            {
                if (IsDarkTheme)
                    RealStatus = "https://armte.github.io/FCC4/img/scattered-clouds.png";
                else
                {
                    RealStatus = "https://armte.github.io/FCC4/img/scattered-clouds-night.png";
                }
            }

            WaetherStatus = rootObject.weather[0].description;
            float flnum = 0;
            int num = 0;
            bool number = float.TryParse(rootObject.main.temp.ToString(), out flnum);
            num = (int)flnum;
            //TemperatureStatus = num.ToString()+ " °C";
            TemperatureStatus = num.ToString() + " °";
            HumidityStatus = rootObject.main.humidity.ToString() + " %";
            WindStatus = rootObject.wind.speed.ToString() + " knot";
        }

        #endregion

        #region Methods
       
        /// <summary>
        /// Invoked when an item is selected from the articles card list page.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private void NavigateToNextPage(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {         
            if (e != null)
            {
                var ICCID = (e.ItemData as Model).Description;
                var sensor = (e.ItemData as Model).DeviceType;
                string gname = (e.ItemData as Model).Name;
                Application.Current.MainPage.Navigation.PushAsync(new MyWalletPage(ICCID, sensor, gname));
            }           
        }

        async 
        Task
GetDataFromWebAPI(string iccid,string sensor)
        {
            Items = await webAPIService.RefreshDataAsync(iccid,sensor,"Finish");
        }
       

        public void OnTapped(ItemTappedEventArgs eventArgs)
        {
            var name = (eventArgs.Item as Model).Name;
        }
        /// <summary>
        /// Invoked when the favourite button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void FavouriteButtonClicked(object obj)
        {
           // PopupOpen = true;
           
            if (obj != null && (obj is Model))
            {
                UserName = (obj as Model).Name;
               (obj as Model).IsFavourite = (obj as Model).IsFavourite ? false : true;
            }
            else
            {
                var button = obj as SfButton;
                if (button != null)
                {
                    button.Text = (button.Text == "\ue701") ? "\ue732" : "\ue701";
                }
            }
        }
        
        public void BookmarkButtonClicked(object eventArgs)
        {
            
        }

        void RaisepropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            DevResult = JsonConvert.DeserializeObject<List<Models.Device>>(s.Result);
           

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return DevResult;
            }
            else
            {
                return null;
            }

        }

        public async Task GetRequest(string Telemetry, string DeviceID)
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
                    IsConnected = false;
                    IsDisconnected = true;
                }
                else
                {
                    IsConnected = true;
                    IsDisconnected = false;
                }
            }
            else
            {
                RealTimeValue = "0";
                RealTimeStamp = "Error";
                IsConnected = false;
                IsDisconnected = true;
            }

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

        private void IrrigateFunction(object obj)
        {
            PopupOpen = true;
        }
            async private void Refresh(object obj)
        {
            IsRefreshing = true;
            IsLoading = true;
            UpdateNowUI();

          
            //   var events = await tblEvents.getAllReadings();
            //var events = await Book.ReadBook();
            var APIDevices = await GetDevicePerUserAsync(Application.Current.Properties["UserEmail "].ToString());

            if (APIDevices.Count == 0)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Forms.FillInDetailsPage());

            }
            else
            {
                Articles = new ObservableCollection<Model>();
                // Get Devices
                // betshoof fe kam gehaz
                foreach (var device in APIDevices)
                {
                    try {
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
                        
                   // Stream ms = new MemoryStream(device.deviceImageBytes);
                  //  Dev.ImageBytes = device.deviceImageBytes;
                    Dev.DeviceType = device.deviceType;
                  //  Dev.DeviceImageSource = ImageSource.FromStream(() => ms);
                    Dev.ImagePath = device.deviceImagePath;
                   
                  
                    //Dev.ImagePath = ImageSource.FromStream(() => ms);
                    Dev.Date = device.deviceInDate; 
                    Dev.AverageReadingTime = device.deviceICCID;
                    Dev.Description = device.deviceICCID;
                    Dev.BookmarkedCount = RealTimeValue;
                    Dev.FavouritesCount = LastIrrigationDuration;
                    Dev.SharedCount = TotalBalance.ToString();
                    Articles.Add(Dev);
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }

            IsLoading = false;
            IsRefreshing = false;
        }
        private void UpdateNowUI()
        {
            isconnected = false;
            isdisconnected = false;
        }
        private void CalculateTotalCostPerMonth(string iccid,string sensor)
        {
            try
            {
                var result = App.EventsDatabase.getAllActiveTelemetry(iccid,sensor);
                TotalBalance = 0;
                AllTelemetryThisMonth = new ObservableCollection<tblEvents>();
                foreach (var r in result.Result)
                {
                    if (r.eventDate.Month == DateTime.Today.Month && r.eventDate.Year == DateTime.Today.Year)
                    {
                        if (int.Parse(r.elapsedtime) < 61)
                        {
                            r.Duration = r.elapsedtime.ToString() + " Seconds.";
                        }
                        else
                        {
                            r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                        }
                        int liters = int.Parse(r.flowml) / 5;
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
        private async Task UpdateDeviceStatusAsync(string iccid,string sensor)
        {

            try
            {
                var httpClient = new HttpClient();
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://smiqsdbcrud.azurewebsites.net/api/tblEvents/" + iccid +"/"+ sensor),

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
        private async Task InitiateDBAsync(string SmiqsICCID,string sensor)
        {
            webAPIService = new WebAPIService();
            //Item source which needs to be displayed on the list view.
         
            await GetDataFromWebAPI(SmiqsICCID, sensor);


            //Get Telemetries
            var EventHighestTelemetry = App.EventsDatabase.getLastReadingDate(SmiqsICCID, sensor);       
            if (EventHighestTelemetry.Result == null)
            {
                var FullEvents_Results = from mytable in items
                                         where
                                         mytable.DeviceID == SmiqsICCID && mytable.valveName.Contains(sensor)
                                         select mytable;
                List<tblEvents> FullEvents = FullEvents_Results.ToList();

                foreach (var T in items)
                    await App.EventsDatabase.SaveTelemetryAsync(T);
            }
            else
            {
                var DateRange_result = from mytable in items
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

        public class TelemetryValue
        {
            public string value { get; set; }
            public DateTime timestamp { get; set; }
        }
        private void ShareButtonClicked(object obj)
        {
            // Do Something.
        }
        private void EditItemButtonClicked(object obj)
        {
            if (obj != null)
            {
                var ICCID = (obj as Model).Author;
                PopupOpen = true;
                LabelString = ICCID.ToString();
                Visible = true;
                //  Application.Current.MainPage.Navigation.PushAsync(new MyWalletPage(ICCID));
            }
        }
        private void DeleteItemButtonClicked(object obj)
        {
            if (obj != null)
            {
                var ICCID = (obj as Model).Author;
                var device = obj as Models.Device;
                
                App.DevicesDatabase.DeleteTelemetryAsync(device);
                //  Application.Current.MainPage.Navigation.PushAsync(new MyWalletPage(ICCID));
            }
        }
        #endregion

        #region commands

        /// <summary>
        /// Gets or sets the command is executed when the bookmark button is clicked.
        /// </summary>
        public Command BookmarkCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the favourite button is clicked.
        /// </summary>
        public Command AddFavouriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the share button is clicked.
        /// </summary>
        public Command ShareCommand { get; set; }


        public Command RefreshCommand { get; set; }

        public Command IrrigateCommand { get; set; }

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

        public bool IsLoading
        {
            get
            {
                return this.isloading;
            }
            set
            {
                this.isloading = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion
    }
}
