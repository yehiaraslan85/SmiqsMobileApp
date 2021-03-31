using Plugin.FacebookClient;
using Plugin.GoogleClient;
using smiqs.Models;
using smiqs.ViewModels.Helpers;
using smiqs.Views.Forms;
using smiqs.Views.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;


namespace smiqs.Views.Dashboard
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage(NetworkAuthData networkAuthData)
        {
            BindingContext = networkAuthData;
            InitializeComponent();
            lblEmail.Text= networkAuthData.Email;
            lblName.Text = networkAuthData.Name;
        }

        async void OnLogout(object sender, System.EventArgs e)
        {
            if (BindingContext is NetworkAuthData data)
            {
                switch (data.Name)
                {
                    case "Facebook":
                        CrossFacebookClient.Current.Logout();
                        break;
                    case "Google":
                        CrossGoogleClient.Current.Logout();
                        break;
                }

                await Navigation.PopModalAsync();
            }
        }
        async void GotoHomePage(object sender, System.EventArgs e)
        {
            if (ConfirmPassword.Text != Password.Text)
                await App.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "Ok");
            else
            {
                bool result = await Auth.RegisterUser(lblName.Text, lblEmail.Text, Password.Text);
                if (result)
                {
                    await Application.Current.MainPage.Navigation.PopAsync();



                    Application.Current.Properties["SmiqsICCID "] = txtICCIDvar.Text;
                    Application.Current.Properties["username "] = lblName.Text;
                    Application.Current.Properties["useraddress "] = txtAddress.Text;
                    Application.Current.Properties["useremail "] = lblEmail.Text;

                    Application.Current.Properties["SmiqsCommandA "] = "COMMANDVALVESTATUSA";
                    Application.Current.Properties["SmiqsCommandB "] = "COMMANDVALVESTATUSB";
                    Application.Current.Properties["SmiqsCommandC "] = "COMMANDVALVESTATUSC";
                    Application.Current.Properties["SmiqsCommandD "] = "COMMANDVALVESTATUSD";
                    Application.Current.Properties["SmiqsAuthorization "] = "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563";
                    Application.Current.Properties["SmiqsComponentName"] = "SmartiqsGSMV1_6j7";
                    Application.Current.Properties["SmiqsVWCMin"] = "52";
                    Application.Current.Properties["SmiqsVWCMax"] = "65";
                    await Application.Current.SavePropertiesAsync();

                    await App.Current.MainPage.Navigation.PushModalAsync(new BottomNavigationPage());
                }
            }
        }
    }
}