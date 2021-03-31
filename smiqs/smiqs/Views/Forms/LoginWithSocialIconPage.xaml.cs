using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System;
using smiqs.DataService;
using smiqs.ViewModels;

namespace smiqs.Views.Forms
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginWithSocialIconPage:ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWithSocialIconPage" /> class.
        /// </summary>
        public LoginWithSocialIconPage(IOAuth2Service oAuth2Service)
        {
            InitializeComponent();
            this.BindingContext = new LoginVM(oAuth2Service);     
        }
        async void RegisterLabel_Tapped(object sender, EventArgs args)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Forms.SignUpPage());

        }

        void LoginLabel_Tapped(object sender, EventArgs args)
        {
        
        }
    }
}