using smiqs.Models;
using smiqs.ViewModels.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using smiqs.Views.Catalog;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using static smiqs.Views.MainMenu;
using Newtonsoft.Json;
using System.Net;
using smiqs.Helper;
using System.Collections.ObjectModel;
using Azure.Storage.Files.Shares;
using Azure;
using Azure.Storage.Blobs;

namespace smiqs.Views.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FillInDetailsPage : ContentPage, INotifyPropertyChanged
    {
        public string userEmail = "";
        public string queryReadICCID = "SELECT * FROM dbo.tblICCID where iccid LIKE @DevID";
        public MediaFile selectedImageFile;
        public string selectedImagePath;
        public byte[] selectedImageBytes;
        public Stream imageStream;
        private WebAPIService webAPIService;
        public ObservableCollection<tblEvents> Items { get; set; }

        public ImageSource PictureImageSource;
        public FillInDetailsPage()
        {
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;

            IsLoading = false;
            try { 
                
            userEmail = Application.Current.Properties["UserEmail "].ToString();
            }
            catch(Exception e)
            {

            }
            if (Auth.IsAuthenticated())
            {
                ICCIDHolder.IsVisible = true;
                ConfirmICCIDBtn.IsVisible = true;
                stackICCIDEntry.IsVisible = true;
            }
            else
            {

                ICCIDHolder.IsVisible = false;
                ConfirmICCIDBtn.IsVisible = false;
                stackICCIDEntry.IsVisible = false;
            }

        }

        public FillInDetailsPage(NetworkAuthData networkAuthData)
        {
            InitializeComponent();
            BindingContext = networkAuthData;
            userEmail = networkAuthData.Email.ToString();
            ICCIDHolder.IsVisible = true;
            ConfirmICCIDBtn.IsVisible = true;
            stackICCIDEntry.IsVisible = true;
        }

        public FillInDetailsPage(string email, string name)
        {
            InitializeComponent();
            IsLoading = false;
            //       lblUserName.Text = "Welcome " + name;
            //       PasswordEntryHolder.IsVisible = false;
            //       ConfirmPasswordEntryHolder.IsVisible = false;
            ICCIDHolder.IsVisible = true;
            //       AddressEntryHolder.IsVisible = false;
            userEmail = email;
            //       ConfirmPasswordBtn.IsVisible = false;
            ConfirmICCIDBtn.IsVisible = true;
            //        lblUserEmail.IsVisible = false;
            //       stackAddressEntry.IsVisible = false;
            stackICCIDEntry.IsVisible = true;
            //        stackPasswordEntry.IsVisible = false;
            //        stackConfirmPasswordEntry.IsVisible = false;
        }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
        async Task<bool> ConfirmICCIDAsync(string ICCID)
        {
            var httpClient = new HttpClient();
            var data = new Post { request = true };
            var content = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://smiqsdbcrud.azurewebsites.net/api/tblICCIDs/" + ICCID)
            };
            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<string> uploadanImageAsync(byte[] ms)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=functionappsmartiqsstora;AccountKey=mtzL9CjuMPB5nglH57hoUAJ7dhgI42eAGFmdGldLTsXXs3DuqBQ/GrLGfSzqmWjCfGi/7x3YByze7itymNafiQ==;EndpointSuffix=core.windows.net";

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            string containerName = "smiqsimages";

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Create a local file in the ./data/ directory for uploading and downloading
            string fileName = ICCIDEntry.Text + GNameEntry.Text + GTypeEntry.Text + ".jpg";
            string localFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), ICCIDEntry.Text + GNameEntry.Text + GTypeEntry.Text + ".jpg");

            // Write text to the file
            File.WriteAllBytes(localFilePath, ms);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Open the file and upload its data
            FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            
            uploadFileStream.Close();
            return "https://functionappsmartiqsstora.blob.core.windows.net/smiqsimages/"+ fileName;
        }
        async Task<bool> AddDeviceToDB(Models.Device data)
        {
            var httpClient = new HttpClient();
            var contentType = "application/json"; //May vary based on your app
            var content = JsonConvert.SerializeObject(data);
            var httpMethod = HttpMethod.Post;
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://smiqsdbcrud.azurewebsites.net/api/tblDevices/"),
                Method = httpMethod,
                Content = new StringContent(content, System.Text.Encoding.UTF8, contentType),

            };

            var httpResponse = await httpClient.SendAsync(request);
            var s = httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

      
        async void ConfirmICCID_ClickedAsync(object sender, EventArgs e)
        {
            //continue check on fields
            if (
                ICCIDEntry.Text == null ||
                GNameEntry.Text == null ||
                GTypeEntry.Text == null ||
                selectedImageBytes == null
                )
            {
                loadingIndicator.IsRunning = true;

                await App.Current.MainPage.DisplayAlert("Error", "Please fill in all details", "Ok");
                loadingIndicator.IsRunning = false;

            }
            else
            {
                IsLoading = true;
                loadingIndicator.IsRunning = true;
                bool Valid = await ConfirmICCIDAsync(ICCIDEntry.Text);
                if (Valid)
                {
                    //Register Device in Database
                    //Both Internal and External db
                    Models.Device dev = new Models.Device();
                    dev.deviceICCID = ICCIDEntry.Text;
                    dev.deviceName = GNameEntry.Text;
                    dev.deviceType = GTypeEntry.Text;
                    //dev.deviceImageBytes = selectedImageBytes;           
                    dev.deviceImagePath = await uploadanImageAsync(selectedImageBytes);
                    dev.deviceMode = "Smart";
                    dev.deviceInDate = DateTime.Now.ToString();
                    dev.OwnerEmail = userEmail;
                    await App.DevicesDatabase.SaveTelemetryAsync(dev);

                    await AddDeviceToDB(dev);
                   // await InitiateDBAsync(ICCIDEntry.Text, GTypeEntry.Text);

                    await Application.Current.MainPage.Navigation.PushAsync(new LoadingPage());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Wrong ICCID number", "Ok");
                    loadingIndicator.IsRunning = false;

                }
                loadingIndicator.IsRunning = false;
                IsLoading = false;
            }
        }
        private async Task InitiateDBAsync(string SmiqsICCID, string sensor)
        {
            webAPIService = new WebAPIService();
            //Item source which needs to be displayed on the list view.

            await GetDataFromWebAPI(SmiqsICCID, sensor);


            //Get Telemetries
            var EventHighestTelemetry = App.EventsDatabase.getLastReadingDate(SmiqsICCID, sensor);
            if (EventHighestTelemetry.Result == null)
            {
                var FullEvents_Results = from mytable in Items
                                         where
                                         mytable.DeviceID == SmiqsICCID && mytable.valveName.Contains(sensor)
                                         select mytable;
                List<tblEvents> FullEvents = FullEvents_Results.ToList();

                foreach (var T in Items)
                    await App.EventsDatabase.SaveTelemetryAsync(T);
            }
            else
            {
                var DateRange_result = from mytable in Items
                                       where
                                       mytable.eventDate > EventHighestTelemetry.Result.eventDate
                                       &&
                                       mytable.DeviceID == SmiqsICCID
                                       && mytable.valveName.Contains(sensor)
                                       select mytable;
                List<tblEvents> NewEvents = DateRange_result.ToList();
                foreach (var T in NewEvents)
                    await App.EventsDatabase.SaveTelemetryAsync(T);
            }
        }
        private byte[] GetImageBytes(Stream stream)
        {
            byte[] ImageBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
                selectedImageBytes = memoryStream.ToArray();
            }

            return ImageBytes;
        }
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        async Task GetDataFromWebAPI(string iccid, string sensor)
        {
            Items = await webAPIService.RefreshDataAsync(iccid, sensor, "Finish");


        }
        private async void BackButton_Clicked(object sender, System.EventArgs e)
        {

            Application.Current.MainPage = new NavigationPage(new ArticleCardPage());
        }
        async void Handle_Clicked(object sender, System.EventArgs e)
         {
             //! added using Plugin.Media;
             await CrossMedia.Current.Initialize();

             //// if you want to take a picture use this
             // if(!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
             /// if you want to select from the gallery use this
             if (!CrossMedia.Current.IsPickPhotoSupported)
             {
                 await DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                 return;
             }

            //! added using Plugin.Media.Abstractions;
            // if you want to take a picture use StoreCameraMediaOptions instead of PickMediaOptions
            selectedImageFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,

            });


            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");

                return;
            }
            selectedImagePath = selectedImageFile.Path;
            selectedImage.Source = ImageSource.FromStream(() =>
            {
                imageStream = selectedImageFile.GetStream();
                selectedImageFile.Dispose();
                return imageStream;
            });

    



            // UploadImage(selectedImageFile.GetStream());
        }
        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                PictureImageSource = await TakePhoto();
                 if (PictureImageSource != null)
                  selectedImage.Source = PictureImageSource;
                else
                {
                    await DisplayAlert("Unknown Error", "Unknown Error, image not captured", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Unknown Error", "Unknown Error, image not captured", "OK");

                // handle your exception
            }
        }
        private async void PickPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await SelectPhoto();
                if (result != null)
                    selectedImage.Source = result;
            }
            catch (Exception ex)
            {
                // handle your exception
            }
        }

        [Obsolete]
        public async Task<ImageSource> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable ||
                    !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "Sorry! No camera available.", "OK");
                return null;
            }

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();
            if (!isPermissionGranted)
                return null;

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "SmiqsPhotoFolder",
                SaveToAlbum = true,
                CompressionQuality = 100,
                CustomPhotoSize = 100,
                PhotoSize = PhotoSize.Medium,
                AllowCropping = true,
                ModalPresentationStyle = MediaPickerModalPresentationStyle.FullScreen,
                Name = "yehia",
                // MaxWidthHeight = 1000,
            }) ;

            if (file == null)
                return null;

           var imagesrc = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                GetImageBytes(stream);
                return stream;
            });
           
            return imagesrc;
        }

        public async Task<ImageSource> SelectPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", "Sorry! Permission not granted to photos.", "OK");
                return null;
            }

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();
            if (!isPermissionGranted)
                return null;

            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });

            if (file == null)
                return null;

            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            return imageSource;
        }

        [Obsolete]
        private async Task<bool> RequestCameraAndGalleryPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var permissionRequestResult = await CrossPermissions.Current.RequestPermissionsAsync(
                    new Permission[] { Permission.Camera, Permission.Storage, Permission.Photos });

                var cameraResult = permissionRequestResult[Plugin.Permissions.Abstractions.Permission.Camera];
                var storageResult = permissionRequestResult[Plugin.Permissions.Abstractions.Permission.Storage];
                var photosResult = permissionRequestResult[Plugin.Permissions.Abstractions.Permission.Photos];

                return (
                    cameraResult != PermissionStatus.Denied &&
                    storageResult != PermissionStatus.Denied &&
                    photosResult != PermissionStatus.Denied);
            }

            return true;
        }

        [Obsolete]
        private async Task<bool> RequestPermissions(List<Permission> permissionList)
        {
            List<PermissionStatus> permissionStatuses = new List<PermissionStatus>();
            foreach (var permission in permissionList)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                permissionStatuses.Add(status);
            }

            var requiresRequesst = permissionStatuses.Any(x => x != PermissionStatus.Granted);

            if (requiresRequesst)
            {
                var permissionRequestResult = await CrossPermissions.Current.RequestPermissionsAsync(permissionList.ToArray());

                return permissionRequestResult.All(x => x.Value != PermissionStatus.Denied);
            }

            return true;
        }
        /*
           async void Handle_Clicked(object sender, System.EventArgs e)
           {
               if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
               {
                   await DisplayAlert("No Camera", ":( No camera available.", "OK");
                   return;
               }

               var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
               {
                   Directory = "Test",
                   SaveToAlbum = true,
                   CompressionQuality = 75,
                   CustomPhotoSize = 50,
                   PhotoSize = PhotoSize.MaxWidthHeight,
                   MaxWidthHeight = 2000,
                   DefaultCamera = CameraDevice.Front
               });

               if (file == null)
                   return;

               await DisplayAlert("File Location", file.Path, "OK");

               selectedImage.Source = ImageSource.FromStream(() =>
               {
                   var stream = file.GetStream();
                   file.Dispose();
                   return stream;
               });
           }
            */

    }
}