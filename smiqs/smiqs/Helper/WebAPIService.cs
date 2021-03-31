using smiqs.Models;
using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace smiqs.Helper
{
    public class WebAPIService
    {
        #region Fields 

        System.Net.Http.HttpClient client;

        #endregion

        #region Properties 

        public ObservableCollection<tblEvents> Items
        {
            get;  set;
        }
        public ObservableCollection<tblSmiqsTelemetry> SmiqsItems
        {
            get;  set;
        }
        public string WebAPIUrl
        {
            get; private set;
        }

        #endregion

        #region Constructor
        public WebAPIService()
        {
            client = new System.Net.Http.HttpClient();
        }

        #endregion

        #region Methods
        public async System.Threading.Tasks.Task<ObservableCollection<tblEvents>> RefreshDataAsync(string iccid,string sensor,string status)
        {
            WebAPIUrl = "https://smiqsdbcrud.azurewebsites.net/api/tblevents/"+ iccid+"/"+sensor + "/" + status; // Set your REST API url here
            var uri = new Uri(WebAPIUrl);
            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<ObservableCollection<tblEvents>>(content);
                    return Items;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async System.Threading.Tasks.Task<ObservableCollection<tblSmiqsTelemetry>> GetTelemetriesSmiqs(string iccid)
        {
            WebAPIUrl = "https://smiqsdbcrud.azurewebsites.net/api/tblSmiqsTelemetries/" + iccid; // Set your REST API url here
            var uri = new Uri(WebAPIUrl);
            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    SmiqsItems = JsonConvert.DeserializeObject<ObservableCollection<tblSmiqsTelemetry>>(content);
                    return SmiqsItems;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        #endregion
    }
}

