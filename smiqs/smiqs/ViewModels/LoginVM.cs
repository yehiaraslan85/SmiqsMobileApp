using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Refit;
using smiqs.DataService;
using smiqs.Models;
using smiqs.ViewModels.Helpers;
using smiqs.Views;
using smiqs.Views.Catalog;
using smiqs.Views.Dashboard;
using smiqs.Views.Forms;
using smiqs.Views.Navigation;
using smiqs.Views.Onboarding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace smiqs.ViewModels
{
   public class LoginVM : INotifyPropertyChanged
    {
        const string InstagramApiUrl = "https://api.instagram.com";
        const string InstagramScope = "basic";
        const string InstagramAuthorizationUrl = "https://api.instagram.com/oauth/authorize/";
        const string InstagramRedirectUrl = "https://xamboy.com/default.html";
        const string InstagramClientId = "77567512de424a528ba61715d389a644";
      

        private string imagePath;

        private string header;

        private string content;
        IFacebookClient _facebookService = CrossFacebookClient.Current;
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IOAuth2Service _oAuth2Service;

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("CanRegister");
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged("Email");
                OnPropertyChanged("CanLogin");
                OnPropertyChanged("CanRegister");
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                OnPropertyChanged("CanLogin");
                OnPropertyChanged("CanRegister");
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
                OnPropertyChanged("CanRegister");
            }
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
            }
        }

        public bool CanRegister
        {
            get
            {
                return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrEmpty(Name);
            }
        }

       
        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
            }
        }
      
        public string Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
            }
        }
        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                this.content = value;
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public ICommand OnLoginCommand { get; set; }
        public Command GmailLoginCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<AuthNetwork> AuthenticationNetworks { get; set; } = new ObservableCollection<AuthNetwork>()
        {
           
              new AuthNetwork()
            {
                Name = "Google",
                Icon = "Google",
                Foreground = "#000000",
                Background ="#F8F8F8"
            }
        };

        public LoginVM(IOAuth2Service oAuth2Service)
        {
            _oAuth2Service = oAuth2Service;

            OnLoginCommand = new Command<AuthNetwork>(async (data) => await LoginAsync(data));
           
            GmailLoginCommand = new Command<AuthNetwork>(async (data) => await LoginAsync(data));
            LoginCommand = new Command(Login, LoginCanExecute);
            RegisterCommand = new Command(Register, RegisterCanExecute);
            this.ImagePath = "SmiqsLogo.svg";
            this.Header = "NO INTERNET";
            this.Content = "You must be connected to the internet to complete this action";

        }
        private bool RegisterCanExecute(object parameter)
        {
            return CanRegister;
        }
        private async void Register(object parameter)
        {
            if (ConfirmPassword != Password)
                await App.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "Ok");
            else
            {
                bool result = await Auth.RegisterUser(Name, Email, Password);
                if (result)
                    await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        
        private async void Login(object parameter)
        {
            bool result = await Auth.AuthenticateUser(Email, Password);           
            var Devices = App.DevicesDatabase.getAllDevices();
            if (result && Devices.Result.Count != 0) 
             {
                Application.Current.Properties["UserEmail "] = Email;
                Application.Current.Properties["UserName "] = Name;
                await Application.Current.SavePropertiesAsync();
                await Application.Current.MainPage.Navigation.PushAsync(new ArticleCardPage(Email, Name));
             }
             else if(result)
             {
                Application.Current.Properties["UserEmail "] = Email;
                Application.Current.Properties["UserName "] = Name;
                await Application.Current.SavePropertiesAsync();
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Forms.FillInDetailsPage(Email, Name));
             }
            
            
        }
        private bool LoginCanExecute(object parameter)
        {
            return CanLogin;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        async Task LoginAsync(AuthNetwork authNetwork)
        {
            switch (authNetwork.Name)
            {
                case "Facebook":
                    await LoginFacebookAsync(authNetwork);
                    break;
                case "Instagram":
                    await LoginInstagramAsync(authNetwork);
                    break;
                case "Google":
                    await LoginGoogleAsync(authNetwork);
                    break;
            }
        }
        async Task LoginInstagramAsync(AuthNetwork authNetwork)
        {
            EventHandler<string> onSuccessDelegate = null;
            EventHandler<string> onErrorDelegate = null;
            EventHandler onCancelDelegate = null;

            onSuccessDelegate = async (s, a) =>
            {

                UserDialogs.Instance.ShowLoading("Loading");

                var userResponse = await RestService.For<IInstagramApi>(InstagramApiUrl).GetUser(a);

                if (userResponse.IsSuccessStatusCode)
                {
                    var userDataString = await userResponse.Content.ReadAsStringAsync();
                    //Handling Encoding
                    var userDataStringFixed = System.Text.RegularExpressions.Regex.Unescape(userDataString);

                    var instagramUser = JsonConvert.DeserializeObject<InstagramUser>(userDataStringFixed);
                    var socialLoginData = new NetworkAuthData
                    {
                        Logo = authNetwork.Icon,
                        Picture = instagramUser.Data.ProfilePicture,
                        Foreground = authNetwork.Foreground,
                        Background = authNetwork.Background,
                        Name = instagramUser.Data.FullName,
                        Id = instagramUser.Data.Id
                    };

                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.Navigation.PushModalAsync(new HomePage(socialLoginData));
                }
                else
                {
                    //TODO: Handle instagram user info error
                    UserDialogs.Instance.HideLoading();

                    await UserDialogs.Instance.AlertAsync("Error", "Houston we have a problem", "Ok");
                }

                _oAuth2Service.OnSuccess -= onSuccessDelegate;
                _oAuth2Service.OnCancel -= onCancelDelegate;
                _oAuth2Service.OnError -= onErrorDelegate;
            };
            onErrorDelegate = (s, a) =>
            {
                _oAuth2Service.OnSuccess -= onSuccessDelegate;
                _oAuth2Service.OnCancel -= onCancelDelegate;
                _oAuth2Service.OnError -= onErrorDelegate;
                Debug.WriteLine($"ERROR: Instagram, MESSAGE: {a}");
            };
            onCancelDelegate = (s, a) =>
            {
                _oAuth2Service.OnSuccess -= onSuccessDelegate;
                _oAuth2Service.OnCancel -= onCancelDelegate;
                _oAuth2Service.OnError -= onErrorDelegate;
            };

            _oAuth2Service.OnSuccess += onSuccessDelegate;
            _oAuth2Service.OnCancel += onCancelDelegate;
            _oAuth2Service.OnError += onErrorDelegate;
            _oAuth2Service.Authenticate(InstagramClientId, InstagramScope, new Uri(InstagramAuthorizationUrl), new Uri(InstagramRedirectUrl));


        }

        async Task LoginFacebookAsync(AuthNetwork authNetwork)
        {
            try
            {

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            var socialLoginData = new NetworkAuthData
                            {
                                Id = facebookProfile.Id,
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = facebookProfile.Picture.Data.Url,
                                Name = $"{facebookProfile.FirstName} {facebookProfile.LastName}",
                            };
                            await App.Current.MainPage.Navigation.PushModalAsync(new HomePage(socialLoginData));
                            break;
                        case FacebookActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Facebook Auth", "Canceled", "Ok");
                            break;
                        case FacebookActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Facebook Auth", "Error", "Ok");
                            break;
                        case FacebookActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Facebook Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "picture", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task LoginGoogleAsync(AuthNetwork authNetwork)
        {
            try
            {
                if (!string.IsNullOrEmpty(_googleService.AccessToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
#if DEBUG
                            var googleUserString = JsonConvert.SerializeObject(e.Data);
                            Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
#endif

                            var socialLoginData = new NetworkAuthData
                            {
                                Id = e.Data.Id,
                                Email = e.Data.Email,                               
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = e.Data.Picture.AbsoluteUri,
                                Name = e.Data.Name,
                            };
                         

                            Application.Current.Properties["UserEmail "] = socialLoginData.Email;
                            await Application.Current.MainPage.Navigation.PushAsync(new FillInDetailsPage(socialLoginData));
                            break;
                        case GoogleActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _googleService.OnLogin -= userLoginDelegate;
                };

                _googleService.OnLogin += userLoginDelegate;

                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}