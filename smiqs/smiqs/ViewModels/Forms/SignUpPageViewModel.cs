using smiqs.ViewModels.Helpers;
using smiqs.Views.Forms;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace smiqs.ViewModels.Forms
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string name;

        private string password;

        private string confirmPassword;
       
        private string email;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.Register, this.RegisterCanExecute);          
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("CanRegister");
            }
        }

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

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                OnPropertyChanged("Password");
                OnPropertyChanged("CanLogin");
                OnPropertyChanged("CanRegister");
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password confirmation from users in the Sign Up page.
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                if (this.confirmPassword == value)
                {
                    return;
                }

                this.confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
                OnPropertyChanged("CanRegister");
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        public bool CanRegister
        {
            get
            {
                return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrEmpty(Name);
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            //  await Application.Current.MainPage.Navigation.PopAsync();
            if (ConfirmPassword != Password)
                await App.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "Ok");
            else
            {
                bool result = await Auth.RegisterUser(Name, Email, Password);
                if (result)
                    await App.Current.MainPage.DisplayAlert("Info", "Account created successfully, please login with your new email and password", "Ok");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
            }

        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            // Do something
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
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion
    }
}