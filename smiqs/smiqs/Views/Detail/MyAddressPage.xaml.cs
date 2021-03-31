using smiqs.Models;
using smiqs.ViewModels.Detail;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace smiqs.Views.Detail
{
    /// <summary>
    /// Page to show my address page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyAddressPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyAddressPage" /> class.
        /// </summary>
        public MyAddressPage(TelemetryEventsModel T)
        {
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#373668");
            navigationPage.Title = "Details";
            navigationPage.BarTextColor = Color.White;
            this.BindingContext = new MyAddressViewModel(T);
        }
    }
}