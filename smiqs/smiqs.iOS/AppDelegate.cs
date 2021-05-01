using Syncfusion.XForms.iOS.Core;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.SfGauge.XForms.iOS;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfCarousel.XForms.iOS;
using Syncfusion.XForms.iOS.Cards;

using Foundation;
using UIKit;
using UserNotifications;
using Syncfusion.XForms.iOS.TabView;
using Plugin.GoogleClient;
using Syncfusion.XForms.iOS.TextInputLayout;
using Syncfusion.SfPullToRefresh.XForms.iOS;
using smiqs.iOS.Services;
using WindowsAzure.Messaging;
using UserNotifications;
using System;
using System.Linq;
using System.Diagnostics;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using System.Collections.Generic;
using Syncfusion.SfRadialMenu.XForms.iOS;
using Syncfusion.XForms.iOS.Backdrop;

namespace smiqs.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        private SBNotificationHub Hub { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE5NTE5QDMxMzgyZTMyMmUzMEFZb2RZNUpXSUxTKzNYRENFNFdQU3ZwaG95cmYvcm02MWh2ZGk5MzNSNVE9");
            //   TabLayoutResource = Resource.Layout.Tabbar;
            new Syncfusion.SfNavigationDrawer.XForms.iOS.SfNavigationDrawerRenderer();
            new SfRotatorRenderer();
            new SfBusyIndicatorRenderer();

            global::Xamarin.Forms.Forms.Init();
            SfSegmentedControlRenderer.Init();
            SfBackdropPageRenderer.Init();

            SfCardViewRenderer.Init();
            //Core.Init();
            SfPullToRefreshRenderer.Init();
            SfRotatorRenderer.Init();
            SfComboBoxRenderer.Init();
            SfChartRenderer.Init();
            SfGradientViewRenderer.Init();
            SfRatingRenderer.Init();
            SfListViewRenderer.Init();
            SfButtonRenderer.Init();
            SfBorderRenderer.Init();
            SfSwitchRenderer.Init();
            SfLinearGaugeRenderer.Init();
            SfDigitalGaugeRenderer.Init();
            SfCalendarRenderer.Init();
            SfCarouselRenderer.Init();
            SfCardLayoutRenderer.Init();
            SfTextInputLayoutRenderer.Init();
        
            SfRadialMenuRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
            SfTabViewRenderer.Init();
            SfChartRenderer.Init();
            Syncfusion.XForms.iOS.BadgeView.SfBadgeViewRenderer.Init();
            SfGaugeRenderer.Init();
            SfCardViewRenderer.Init();
            Firebase.Core.App.Configure();
            global::Xamarin.Forms.Forms.Init();

           // UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();
            GoogleClientManager.Initialize();
            LoadApplication(new App(new OAuth2Service()));



            bool result = base.FinishedLaunching(app, options);

            RegisterForRemoteNotifications();

            return result;
        }

        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Badge |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }
        string ListenConnectionString { get; set; } = "Endpoint=sb://smiqsnhns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=rJX1V2cFu98PJBSLzjopcUUiXVQkb+jdfoHfn5UWbhs=";
        string NotificationHubName { get; set; } = "smiqsnh";
        string[] SubscriptionTags { get; set; } = { "8997103101022345764," };
        string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            try { 
            Hub = new SBNotificationHub(ListenConnectionString, NotificationHubName);

            // update registration with Azure Notification Hub
            Hub.UnregisterAll(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"Unable to call unregister {error}");
                    return;
                }
                List<string> list = new List<string>();
            var Devices = App.DevicesDatabase.getAllDevices();
                if (Devices.Result.Count == 0)
                {
                    list.Add("8997103101022345764");
                }
                else
                {
                    foreach (var device in Devices.Result)
                    {
                        list.Add(device.deviceICCID);
                    }
                }
             //   SubscriptionTags = list.ToArray();
                
                var tags = new NSSet("8997103101022345764");
                //var tags = new NSSet(SubscriptionTags);
                Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                    }
                });

                var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                Hub.RegisterTemplate(deviceToken, "defaultTemplate", APNTemplateBody, templateExpiration, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        if (errorCallback != null)
                        {
                            Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                        }
                    }
                });
            });
            }
            catch(Exception ex)
            {

            }
        }
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }
        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;

                //Extract the alert text
                // NOTE: If you're using the simple alert by just specifying
                // "  aps:{alert:"alert msg here"}  ", this will work fine.
                // But if you're using a complex alert with Localization keys, etc.,
                // your "alert" object from the aps dictionary will be another NSDictionary.
                // Basically the JSON gets dumped right into a NSDictionary,
                // so keep that in mind.
                if (aps.ContainsKey(new NSString("alert")))
                    alert = (aps[new NSString("alert")] as NSString).ToString();

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        var myAlert = UIAlertController.Create("Notification", alert, UIAlertControllerStyle.Alert);
                        myAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(myAlert, true, null);
                    }
                }
            }

        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return GoogleClientManager.OnOpenUrl(app, url, options);
        }
        public static void Initialize(string clientId = null, params string[] scopes)
        { }
    }
}
