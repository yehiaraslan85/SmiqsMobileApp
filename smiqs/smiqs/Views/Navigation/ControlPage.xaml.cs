
using Newtonsoft.Json;
using smiqs.Models;
using smiqs.Views.Catalog;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static smiqs.Views.MainMenu;

namespace smiqs.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : ContentPage
    {

        SfPopupLayout popupLayout;
        SfComboBox combobox;
        public ControlPage()
        {
            InitializeComponent();
            popupLayout = new SfPopupLayout();

            List<string> list = new List<string>();
            list.Add("Dashboard");
            list.Add("Devices");
            list.Add("History");
            list.Add("Settings");
            list.Add("Set Up a Device");
            list.Add("Account");
            list.Add("Control Center");
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

            //   GetRequestA("VA", Application.Current.Properties["SmiqsICCID "].ToString());
            combobox = new SfComboBox();
            var devices= App.DevicesDatabase.getAllDevices();
            comboBox.BindingContext = devices;
            comboBox.DataSource = devices.Result;

            comboBox.DisplayMemberPath = "deviceName";
            comboBox.SelectedValuePath = "deviceICCID";
           // comboBox.SelectionChanged += ComboBox_SelectionChanged;

            comboBox.SelectedIndex = 0;
        }
       
        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            await Application.Current.MainPage.Navigation.PushAsync(new Forms.FillInDetailsPage());
        }
        
        private async void State_StateChangingA(object sender, SwitchStateChangingEventArgs e)
        {
            if (e.NewValue == true)
            {
                this.sfSwitchA.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;
                await PostRequest(selectedItem.deviceICCID, true, "COMMANDVALVESTATUSA", true, sfSwitchA);
            }
            else
            {
                this.sfSwitchA.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;

                await PostRequest(selectedItem.deviceICCID, false, "COMMANDVALVESTATUSA", false, sfSwitchA);
            }
            this.sfSwitchA.IsBusy = false;
        }
        private async void State_StateChangingB(object sender, SwitchStateChangingEventArgs e)
        {
            if (e.NewValue == true)
            {
                this.sfSwitchB.IsBusy = true;
               smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;
                //  iccid = combobox.SelectedItem.ToString();

                await PostRequest(selectedItem.deviceICCID, true, "COMMANDVALVESTATUSB", true, sfSwitchB);
            }
            else
            {
                this.sfSwitchB.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;

                await PostRequest(selectedItem.deviceICCID, false, "COMMANDVALVESTATUSB", false, sfSwitchB);
            }
            this.sfSwitchB.IsBusy = false;
        }
        private async void State_StateChangingC(object sender, SwitchStateChangingEventArgs e)
        {
            if (e.NewValue == true)
            {
                this.sfSwitchC.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;

                await PostRequest(selectedItem.deviceICCID, true, "COMMANDVALVESTATUSC", true, sfSwitchC);
            }
            else
            {
                this.sfSwitchC.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;

                await PostRequest(selectedItem.deviceICCID, false, "COMMANDVALVESTATUSC", false, sfSwitchC);
            }
            this.sfSwitchC.IsBusy = false;
        }
        private async void State_StateChangingD(object sender, SwitchStateChangingEventArgs e)
        {
            if (e.NewValue == true)
            {
                this.sfSwitchD.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;

                await PostRequest(selectedItem.deviceICCID, true, "COMMANDVALVESTATUSD", true, sfSwitchD);
            }
            else
            {
                this.sfSwitchD.IsBusy = true;
                smiqs.Models.Device selectedItem = (Models.Device)comboBox.SelectedItem;

                await PostRequest(selectedItem.deviceICCID, false, "COMMANDVALVESTATUSD", false, sfSwitchD);
            }
            this.sfSwitchD.IsBusy = false;
        }
        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
        void AddDeviceButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
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
        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem.ToString() == "Dashboard")
            {
                Application.Current.MainPage.Navigation.PushAsync(new ArticleCardPage());

            }
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
        public async Task PostRequest(string iccid, Boolean flag, string commandName, Boolean busyStatus,SfSwitch sw)
        {
            var httpClient = new HttpClient();
            //var data = "{'request': true}"; //May not need this, based on your app needs
            var contentType = "application/json"; //May vary based on your app
            var httpMethod = HttpMethod.Post; //or Get, or whatever HTTP verb your API endpoint needs
            var data = new Post { request = flag };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + iccid + "/components/SmartiqsGSMV1_6j7/commands/" + commandName),
                Method = httpMethod,
                Content = new StringContent(content, System.Text.Encoding.UTF8, contentType),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);

            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {
                sw.IsOn = busyStatus;

            }
            else
            {
                sw.IsOn = false;
            }
        }
      
        public class TelemetryValue
        {
            public string value { get; set; }
            public DateTime timestamp { get; set; }
        }
        public async void GetRequestA(string Telemetry, string DeviceID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/" + Telemetry),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            TelemetryValue IoTCentralResult = JsonConvert.DeserializeObject<TelemetryValue>(s.Result);



            // SSA = float.Parse(xx.ToString());
            string RealTimeValue = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {

                RealTimeValue = IoTCentralResult.value.ToString();
                if (RealTimeValue.Equals("0"))
                    this.sfSwitchA.IsBusy= false;
                else
                    this.sfSwitchA.IsBusy = true;


            }
        }
        public async void GetRequest(string Telemetry, string DeviceID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/" + Telemetry),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            TelemetryValue IoTCentralResult = JsonConvert.DeserializeObject<TelemetryValue>(s.Result);

            // SSA = float.Parse(xx.ToString());
            string RealTimeValue = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                RealTimeValue = IoTCentralResult.value.ToString();
                if (RealTimeValue.Equals("0"))
                    this.sfSwitchA.IsBusy = false;
                else
                    this.sfSwitchA.IsBusy = true;
            }
        }
        public async void GetRequestB(string Telemetry, string DeviceID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/" + Telemetry),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            TelemetryValue IoTCentralResult = JsonConvert.DeserializeObject<TelemetryValue>(s.Result);



            // SSA = float.Parse(xx.ToString());
            string RealTimeValue = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {

                RealTimeValue = IoTCentralResult.value.ToString();
                if (RealTimeValue.Equals("0"))
                    this.sfSwitchB.IsBusy = false;
                else
                    this.sfSwitchB.IsBusy = true;


            }
        }

        public async void GetRequestC(string Telemetry, string DeviceID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/" + Telemetry),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            TelemetryValue IoTCentralResult = JsonConvert.DeserializeObject<TelemetryValue>(s.Result);



            // SSA = float.Parse(xx.ToString());
            string RealTimeValue = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {

                RealTimeValue = IoTCentralResult.value.ToString();
                if (RealTimeValue.Equals("0"))
                    this.sfSwitchC.IsBusy = false;
                else
                    this.sfSwitchC.IsBusy = true;


            }
        }

        public async void GetRequestD(string Telemetry, string DeviceID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/" + DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/" + Telemetry),

            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();

            TelemetryValue IoTCentralResult = JsonConvert.DeserializeObject<TelemetryValue>(s.Result);



            // SSA = float.Parse(xx.ToString());
            string RealTimeValue = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {

                RealTimeValue = IoTCentralResult.value.ToString();
                if (RealTimeValue.Equals("0"))
                    this.sfSwitchD.IsBusy = false;
                else
                    this.sfSwitchD.IsBusy = true;


            }
        }
    }
}