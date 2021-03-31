

using smiqs.ViewModels.Calendar;
using smiqs.Views.Catalog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace smiqs.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllIrrigationsPage : ContentPage
    {
        public AllIrrigationsPage(string iccid,string sensor)
        {
            InitializeComponent();
            this.BindingContext = new CalendarPageViewModel(iccid, sensor);
        }
        public AllIrrigationsPage()
        {
            InitializeComponent();
            
        }
        private async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new ArticleCardPage());
        }
    }

}