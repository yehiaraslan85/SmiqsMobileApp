using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using smiqs.Views;
using smiqs.Views.Dashboard;
using smiqs.Views.Forms;
using smiqs.Views.Navigation;
using smiqs.ViewModels.Helpers;
using System.Threading.Tasks;
using smiqs.DataService;
using Plugin.GoogleClient;
using smiqs.Data;
using System.IO;
using smiqs.Views.Settings;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using smiqs.Views.ErrorAndEmpty;
using smiqs.Views.Catalog;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Model = smiqs.Models.Article;
using System.Collections.ObjectModel;
using smiqs.Models;

namespace smiqs
{
    public partial class App : Application
    {
        static TelemetryDB database;
        static TelemetryEvents eventsdatabase;
        static DevicesDB devicesdatabase;
        public static List<Models.Device> DevResult = new List<Models.Device>();
        public static ObservableCollection<Model> ListOfDevices = new ObservableCollection<Model>();
        public static ObservableCollection<tblSmiqsTelemetry> SmiqsItems = new ObservableCollection<tblSmiqsTelemetry>();

        public static string menuicon { get; } = "https://drive.google.com/file/d/1VvlmyJD_iN6N4hLoHzeeGU3WhZiiUzYD/view?usp=sharing";
        public static string MyBaseImageURL { get; } = "https://drive.google.com/file/d/1vWEIukxpx9QhC5tQcUaCFP_hwhQn7cVy/view";
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App(IOAuth2Service oAuth2Service)
        {
            //comment
            try { 
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE3ODY2QDMxMzgyZTM0MmUzMFovZmJpMktRaW82aXRnYTRIZ1dZMVZ1SUt6L3ZSTFBpTkFoUGliWUxuQXM9");

            
              if (Connectivity.NetworkAccess == NetworkAccess.None)
              {
                  InitializeComponent();

                    MainPage = new NavigationPage(new NoInternetConnectionPage());

              }
              else
              {
                  InitializeComponent();
                
                Plugin.Media.CrossMedia.Current.Initialize();
                    //  DependencyService.Get<INotificationManager>().Initialize();
                    //First time to access the application



                if (Auth.IsAuthenticated() && Application.Current.Properties.ContainsKey("UserEmail "))
                {
                        MainPage = new NavigationPage(new LoadingPage());


                       
                }

            
               else 
                {
                    Task.Delay(300);
                        MainPage = new NavigationPage(new LoginWithSocialIconPage(oAuth2Service));

                }
               

              }
            }
            catch(Exception e)
            {
                 App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }

        }

        
        public static TelemetryDB Database
        {
            get
            {
                if (database == null)
                {
                    database = new TelemetryDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TelemetryDB.db3"));
                }
                return database;
            }
        }
        public static TelemetryEvents EventsDatabase
        {
            get
            {
                if (eventsdatabase == null)
                {
                    eventsdatabase = new TelemetryEvents(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TelemetryEventsDB.db3"));
                }
                return eventsdatabase;
            }
        }

        public static DevicesDB DevicesDatabase
        {
            get
            {
                if (devicesdatabase == null)
                {
                    devicesdatabase = new DevicesDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DevicesDB.db3"));
                }
                return devicesdatabase;
            }
        }
        protected override void OnStart()
        {
          /*  AppCenter.Start("ios=a7071100-1523-4542-bdb2-e54c13a11edc;" +
                  "uwp={Your UWP App secret here};" +
                  "android={Your Android App secret here}",
                  typeof(Analytics), typeof(Crashes));*/
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
