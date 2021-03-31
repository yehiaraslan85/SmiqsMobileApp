using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Plugin.GoogleClient;
using Plugin.FacebookClient;
using Acr.UserDialogs;
using smiqs.Droid.Services;
using Android.Content;
using Java.Security;
using System.Threading.Tasks;
using Android.Util;
using Xamarin.Forms;
using Android.Gms.Common;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace smiqs.Droid
{
    
    [Activity(Label = "smiqs", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("@31382e312e30JVQkJeyZ3M+kvyM75TU7jMiiftYCuVIoNPmOsCbjkx4=@352e302e31ZDt2w8Iig07ZxPA2c26TrFBEP + viZNP7uOoYw7j3PsE =@31382e322e30I4CrcYKClt7fhrh / CJzFBkv + MfovlNMzP9hxDAES2kY ="); TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            GoogleClientManager.Initialize(this);

            base.OnCreate(savedInstanceState);
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            IsPlayServicesAvailable();
          
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FacebookClientManager.Initialize(this);
            GoogleClientManager.Initialize(this);
            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();

            UserDialogs.Init(() => this);
            LoadApplication(new App(new OAuth2Service()));

#if DEBUG
            PrintHashKey(this);
#endif
        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }
       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, data);

            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
        }
      
        [Obsolete]
        public static void PrintHashKey(Context pContext)
        {
            try
            {
                PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());

                    System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(md.Digest()));
                }
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
       
    }
}