using smiqs.ViewModels.Dashboard;
using smiqs.ViewModels.Helpers;
using smiqs.Views.Catalog;
using smiqs.Views.Forms;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace smiqs.Views.Dashboard
{
    /// <summary>
    /// My wallet page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyWalletPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FillInDetailsPage"/> class.
        /// </summary>
        public MyWalletPage()
        {
            InitializeComponent();
        }
        public MyWalletPage(string iccid,string sensor,string gname)
        {
            InitializeComponent();
            this.BindingContext = new MyWalletViewModel(iccid, sensor,gname);
         
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!Auth.IsAuthenticated())
            {
                await Task.Delay(300);
              //  await Navigation.PushAsync(new LoginWithSocialIconPage(oAuth2Service));
            }
        }
        private void BackButton_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new ArticleCardPage());
        }
        private void ViewAllButton_Clicked(object sender, System.EventArgs e)
        {
          

        }
        
    }
}