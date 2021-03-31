using smiqs.DataService;
using smiqs.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace smiqs.Views.LoginForms
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        public LoginPage(IOAuth2Service oAuth2Service)
        {
            InitializeComponent();
            this.BindingContext = new LoginVM(oAuth2Service);
        }
    }
}