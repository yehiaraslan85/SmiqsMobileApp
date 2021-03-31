using Acr.UserDialogs;
using Plugin.GoogleClient;
using smiqs.Views.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace smiqs.ViewModels.Navigation
{
    /// <summary>
    /// Viewmodel of settings page
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SettingsViewModel : BaseViewModel
    {
      
        private string emailvar;
        private string namevar;
        private string addressvar;
        private string iccidvar;
        private string devicetypevar;

        public string queryReadUser = "SELECT * FROM dbo.tblUsers where firbaseUID LIKE @fbuserID";
        public string queryReadDevice = "SELECT * FROM dbo.tblDevices where userID LIKE @ID";
        public string queryReadTelemetry_All = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID";

        public string queryReadTelemetry_On = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID and VA = 1";
        public string queryReadTelemetry_Off = "SELECT * FROM dbo.tblTelemetry where DeviceID LIKE @DevID and VA = 0";
        public string queryReadDevices = "SELECT * FROM dbo.tblDevices where U";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:smiqs.ViewModels.Navigation.SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            this.ContTappedCommand = new Command(this.ContQualityTapped);          
            Emailvar = CrossGoogleClient.Current.CurrentUser.Email.ToString();         
            Namevar = CrossGoogleClient.Current.CurrentUser.Name.ToString();
            if(Application.Current.Properties.ContainsKey("SmiqsICCID "))
            ICCIDvar = Application.Current.Properties["SmiqsICCID "].ToString();
            if (Application.Current.Properties.ContainsKey("useraddress "))
                Addressvar = Application.Current.Properties["useraddress "].ToString();
        }
        public string Emailvar
        {
            get
            {
             
                    return emailvar;
            }
            set
            {
               
                emailvar = value;
                this.NotifyPropertyChanged();
            }
        }
        public string Namevar
        {
            get
            {
               
                    return namevar;
            }
            set
            {
              
                namevar = value;
                this.NotifyPropertyChanged();
            }
        }
        public string Addressvar
        {
            get
            {
               
                    return addressvar;
            }
            set
            {
               
                addressvar = value;
                this.NotifyPropertyChanged();
            }
        }
        public string ICCIDvar
        {
            get
            {
               
                    return iccidvar;
            }
            set
            {
                
                iccidvar = value;
                this.NotifyPropertyChanged();
            }
        }
        public string Devicetypevar
        {
            get
            {

                return devicetypevar;
            }
            set
            {

                devicetypevar = value;
                this.NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the value of command used for download quality click.
        /// </summary>
        public Command ContTappedCommand { get; set; }


        private void ContQualityTapped(object obj)
        {
            if (ICCIDvar == null || Addressvar == null || Emailvar == null || Namevar == null)
            {
                UserDialogs.Instance.AlertAsync("Error", "Please enter all fields", "Ok");
            }
            else
            {
                Application.Current.Properties["SmiqsICCID "] = ICCIDvar;
                Application.Current.Properties["username "] = Namevar;
                Application.Current.Properties["useraddress "] = Addressvar;
                Application.Current.Properties["useremail "] = Emailvar;
                Application.Current.Properties["SmiqsType "] = Emailvar;

                Application.Current.Properties["SmiqsCommandA "] = "COMMANDVALVESTATUSA";
                Application.Current.Properties["SmiqsCommandB "] = "COMMANDVALVESTATUSB";
                Application.Current.Properties["SmiqsCommandC "] = "COMMANDVALVESTATUSC";
                Application.Current.Properties["SmiqsCommandD "] = "COMMANDVALVESTATUSD";
                Application.Current.Properties["SmiqsAuthorization "]="SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563";
                Application.Current.Properties["SmiqsComponentName"] = "SmartiqsGSMV1_6j7";
                Application.Current.Properties["SmiqsVWCMin"] = "58";
                Application.Current.Properties["SmiqsVWCMax"] = "65";
                Application.Current.SavePropertiesAsync();

                App.Current.MainPage.Navigation.PushModalAsync(new BottomNavigationPage());
            }

        }       
    }
}
