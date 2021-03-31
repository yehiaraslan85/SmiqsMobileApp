using smiqs.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace smiqs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubscriptionsPage : Xamarin.Forms.TabbedPage
    {
        public SubscriptionsPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!Auth.IsAuthenticated())
            {
                await Task.Delay(300);
                await Navigation.PushAsync(new LoginPage());
            }    
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewSubscriptionPage());
        }
    }
}