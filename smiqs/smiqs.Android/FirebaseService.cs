using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Messaging;
using WindowsAzure.Messaging;
using Android.Support.V4.App;
using smiqs.Views.Catalog;

namespace smiqs.Droid.Overrides
{
    [Service]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService : Firebase.Messaging.FirebaseMessagingService
    {
        public FirebaseService()
        {

        }
        public override void OnNewToken(string token)
        {
            SendRegistrationToAzure(token);
        }

        private void SendRegistrationToAzure(string token)
        {
            try { 
            List<string> iccidlist = new List<string>();
            NotificationHub hub = new NotificationHub("smiqsnh", "Endpoint=sb://smiqsnhns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=rJX1V2cFu98PJBSLzjopcUUiXVQkb+jdfoHfn5UWbhs=", this);
            var Devices = App.DevicesDatabase.getAllDevices();
            if (Devices.Result.Count == 0)
            { 
                hub.Register(token, "default"); 
            }
            else
            {
                foreach (var device in Devices.Result)
                {
                    iccidlist.Add(device.deviceICCID);
                }
                Registration registration = hub.Register(token, iccidlist.ToArray());
                string pnsHandle = registration.PNSHandle;
                hub.RegisterTemplate(pnsHandle, "defaultTemplate", "{\"notification\":{\"title\":\"Notification Hub Test Notification\",\"body\":\"This is a sample notification delivered by Azure Notification Hubs.\"},\"data\":{\"property1\":\"value1\",\"property2\":42}}", iccidlist.ToArray());
            }
            }
            catch(Exception ex)
            {

            }

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;
            if(message.GetNotification()!=null)
            {
                messageBody = message.GetNotification().Body;
            }
            else 
            {
                messageBody = message.Data.Values.First();
            }
            SendLocalNotification(messageBody);
         
            // Here we can store the notifications in a notifications hub
            //   (App.Current.MainPage as ArticleCardPage).
        }

        private void SendLocalNotification(string messageBody)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", messageBody);
            
            var requestCode = new Random().Next();
            var pendingIntent = PendingIntent.GetActivity(this, requestCode, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentTitle("Notification from Smiqs")
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            if(Build.VERSION.SdkInt>=BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel("SmiqsNotifyChannel", "Smiqs App", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(channel);

                notificationBuilder.SetChannelId("SmiqsNotifyChannel");
            }

            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}