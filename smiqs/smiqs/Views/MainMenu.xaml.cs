using Newtonsoft.Json;
using smiqs.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace smiqs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : TabbedPage
    {
        public class Post
        {
           // public int Id { get; set; }
            public bool request { get; set; }
           // public string request { get; set; }
        }

        private const string URL = "https://jsonplaceholder.typicode.com/posts";
        private HttpClient _client = new HttpClient();
        public IList<Telemetry> Monkeys { get; private set; }
        public MainMenu()
        {
            InitializeComponent();
            Monkeys = new List<Telemetry>();
            
            AzureDB InitializeAzureDB = new AzureDB();
            BindingContext = this;
            // PostRequest();
        }
      
        public async void PostRequest()
        {
            var httpClient = new HttpClient();
            //var data = "{'request': true}"; //May not need this, based on your app needs
            var contentType = "application/json"; //May vary based on your app
            var httpMethod = HttpMethod.Post; //or Get, or whatever HTTP verb your API endpoint needs
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/8997103101017701450/components/SmartiqsGSMV1_6j7/commands/COMMANDVALVESTATUSA"),
                Method = httpMethod,
                Content = new StringContent(content, System.Text.Encoding.UTF8, contentType),
                
            };
           
            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);

            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {
                
            }
            else
            {
                //It didn't work, so do something else
            }
        }
        public async void GetRequest(string Telemetry,string DeviceID)
        {
            var httpClient = new HttpClient();
            //var data = "{'request': true}"; //May not need this, based on your app needs
            //var contentType = "application/json"; //May vary based on your app
            var httpMethod = HttpMethod.Get; //or Get, or whatever HTTP verb your API endpoint needs
            
           // var content = "";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://water-consumption-monitoring-dtu32ekris.azureiotcentral.com/api/preview/devices/"+ DeviceID + "/components/SmartiqsGSMV1_6j7/telemetry/"+ Telemetry),
            
            };

            request.Headers.Add("Authorization", "SharedAccessSignature sr=e779b45b-ae87-4104-b486-d3a968644e7a&sig=pxJeUenkoGd3rhlufpxFt3b4HDRdHDLxW3PBFnc5hcE%3D&skn=InvokeCommand&se=1624113888563");

            var httpResponse = await httpClient.SendAsync(request);

            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {

            }
            else
            {
                //It didn't work, so do something else
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
           
           
        }
        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Telemetry selectedItem = e.SelectedItem as Telemetry;
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Telemetry tappedItem = e.Item as Telemetry;
        }
    }
}