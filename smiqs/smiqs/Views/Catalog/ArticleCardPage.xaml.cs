using smiqs.Controls;
using smiqs.Models;
using smiqs.Views.Dashboard;
using smiqs.Views.Forms;
using smiqs.Views.Navigation;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Model = smiqs.Models.Article;
namespace smiqs.Views.Catalog
{
    /// <summary>
    /// Page to display articles as a card type.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleCardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleCardPage" /> class.
        /// </summary>
        /// 


        public ArticleCardPage(string email, string name)
        {
            InitializeComponent();

            List<string> list = new List<string>();
            list.Add("Dashboard");
            list.Add("Devices");
            list.Add("History");
            list.Add("Settings");
            list.Add("Set Up a Device");
            list.Add("Account");
            list.Add("New Features");
            list.Add("Help");
            listView.ItemsSource = list;

         //   Application.Current.Properties["Id "] = networkAuthData.Id.ToString();
            Application.Current.Properties["useremail "] = email;
          //  Application.Current.Properties["Logo "] = networkAuthData.Logo.ToString();
          //  Application.Current.Properties["Foreground "] = networkAuthData.Foreground.ToString();
          //  Application.Current.Properties["Background "] = networkAuthData.Background.ToString();
          //  Application.Current.Properties["Picture "] = networkAuthData.Picture;
            Application.Current.Properties["username "] = name;
            Application.Current.Properties["SmiqsCommandA "] = "COMMANDVALVESTATUSA";
            Application.Current.Properties["SmiqsCommandB "] = "COMMANDVALVESTATUSB";
            Application.Current.Properties["SmiqsCommandC "] = "COMMANDVALVESTATUSC";
            Application.Current.Properties["SmiqsCommandD "] = "COMMANDVALVESTATUSD";
            Application.Current.Properties["SmiqsAuthorization "] = "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563";
            Application.Current.Properties["SmiqsComponentName"] = "SmartiqsGSMV1_6j7";
            Application.Current.Properties["SmiqsVWCMin"] = "52";
            Application.Current.Properties["SmiqsVWCMax"] = "65";

            Application.Current.SavePropertiesAsync();

        }
        public ArticleCardPage(NetworkAuthData networkAuthData)
        {
            InitializeComponent();
            
            List<string> list = new List<string>();
            list.Add("Dashboard");
            list.Add("Devices");
            list.Add("History");
            list.Add("Settings");
            list.Add("Set Up a Device");
            list.Add("Account");
            list.Add("New Features");
            list.Add("Help");
            listView.ItemsSource = list;

            Application.Current.Properties["Id "] = networkAuthData.Id.ToString();
            Application.Current.Properties["useremail "] = networkAuthData.Email.ToString();
            Application.Current.Properties["Logo "] = networkAuthData.Logo.ToString();
            Application.Current.Properties["Foreground "] = networkAuthData.Foreground.ToString();
            Application.Current.Properties["Background "] = networkAuthData.Background.ToString();
            Application.Current.Properties["Picture "] = networkAuthData.Picture;
            Application.Current.Properties["username "] = networkAuthData.Name.ToString();
            Application.Current.Properties["SmiqsCommandA "] = "COMMANDVALVESTATUSA";
            Application.Current.Properties["SmiqsCommandB "] = "COMMANDVALVESTATUSB";
            Application.Current.Properties["SmiqsCommandC "] = "COMMANDVALVESTATUSC";
            Application.Current.Properties["SmiqsCommandD "] = "COMMANDVALVESTATUSD";
            Application.Current.Properties["SmiqsAuthorization "] = "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563";
            Application.Current.Properties["SmiqsComponentName"] = "SmartiqsGSMV1_6j7";
            Application.Current.Properties["SmiqsVWCMin"] = "52";
            Application.Current.Properties["SmiqsVWCMax"] = "65";
           
            Application.Current.SavePropertiesAsync();

        }
        public ArticleCardPage()
        {
            InitializeComponent();

            List<string> list = new List<string>();
            list.Add("Dashboard");
            list.Add("Devices");
            list.Add("History");
            list.Add("Settings");
            list.Add("Set Up a Device");
            list.Add("Account");
            list.Add("New Features");
            list.Add("Help");
            listView.ItemsSource = list;

     
            Application.Current.Properties["SmiqsCommandA "] = "COMMANDVALVESTATUSA";
            Application.Current.Properties["SmiqsCommandB "] = "COMMANDVALVESTATUSB";
            Application.Current.Properties["SmiqsCommandC "] = "COMMANDVALVESTATUSC";
            Application.Current.Properties["SmiqsCommandD "] = "COMMANDVALVESTATUSD";
            Application.Current.Properties["SmiqsAuthorization "] = "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563";
            Application.Current.Properties["SmiqsComponentName"] = "SmartiqsGSMV1_6j7";
            Application.Current.Properties["SmiqsVWCMin"] = "52";
            Application.Current.Properties["SmiqsVWCMax"] = "65";
            Application.Current.SavePropertiesAsync();
        }
        private void ClickToShowPopup_Clicked(object sender, EventArgs e)
        {
            //popupLayout.ShowRelativeToView(label, RelativePosition.AlignBottom, 0, 0);

        }
       
        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>x
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
           //     if (Search.IsVisible)
          //      {
          //          Search.WidthRequest = width;
           //     }
            }
        }

        /// <summary>
        /// Invoked when search button is clicked.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">Event Args</param>
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Forms.FillInDetailsPage());
         /*   this.SearchButton.IsVisible = false;
            this.Search.IsVisible = true;
            this.Title.IsVisible = false;

            if (this.TitleView != null)
            {
                double opacity;

                // Animating Width of the search box, from 0 to full width when it added to the view.
                var expandAnimation = new Animation(
                    property =>
                    {
                        Search.WidthRequest = property;
                        opacity = property / TitleView.Width;
                        Search.Opacity = opacity;
                    }, 0, TitleView.Width, Easing.Linear);
                expandAnimation.Commit(Search, "Expand", 16, 250, Easing.Linear, (p, q) => this.SearchExpandAnimationCompleted());
            }
         */
        }

        /// <summary>
        /// Invoked when back to title button is clicked.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">Event Args</param>
        private void BackToTitle_Clicked(object sender, EventArgs e)
        {
         //   this.SearchButton.IsVisible = true;
       //     if (this.TitleView != null)
        //    {
          //      double opacity;
//
                // Animating Width of the search box, from full width to 0 before it removed from view.
            //    var shrinkAnimation = new Animation(property =>
          //      {
         //           Search.WidthRequest = property;
          //          opacity = property / TitleView.Width;
           //         Search.Opacity = opacity;
            //    },
            //    TitleView.Width, 0, Easing.Linear);
            //    shrinkAnimation.Commit(Search, "Shrink", 16, 250, Easing.Linear, (p, q) => this.SearchBoxAnimationCompleted());
          //  }

          //  SearchEntry.Text = string.Empty;
        }
        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
        void AddDeviceButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem.ToString() == "Devices")
            {
                
            }
            else if (e.SelectedItem.ToString() == "History")
            {
            
            }
            else if (e.SelectedItem.ToString() == "Settings")
            {

            }
            else if (e.SelectedItem.ToString() == "Set Up a Device")
            {

            }
            else if (e.SelectedItem.ToString() == "Account")
            {

            }
            else if (e.SelectedItem.ToString() == "Control Center")
            {
                Application.Current.MainPage.Navigation.PushAsync(new ControlPage());

            }
            navigationDrawer.ToggleDrawer();
        }
        /// <summary>
        /// Invokes when search box Animation completed.
        /// </summary>
        private void SearchBoxAnimationCompleted()
        {
          //  this.Search.IsVisible = false;
       //     this.Title.IsVisible = true;
        }

        /// <summary>
        /// Invokes when search expand Animation completed.
        /// </summary>
        private void SearchExpandAnimationCompleted()
        {
         //   this.SearchEntry.Focus();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Forms.FillInDetailsPage());
        }

        private void ArticlesCard_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (e != null)
            {
                var ICCID = (e.ItemData as Model).Description;
                var sensor = (e.ItemData as Model).DeviceType;
                var gardenName = (e.ItemData as Model).Name;
                Application.Current.MainPage.Navigation.PushAsync(new MyWalletPage(ICCID, sensor, gardenName));
            }
        }
        
        private void DeleteButton_Clicked(Syncfusion.XForms.Buttons.SfButton sender, EventArgs e)
        {
            
            var SelectedDevice = sender.CommandParameter as Model;

            //  App.DevicesDatabase.DeleteTelemetryAsync(SelectedDevice);


        }
    }
}

