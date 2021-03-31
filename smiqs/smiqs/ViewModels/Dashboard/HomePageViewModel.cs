using Xamarin.Forms.Internals;
using smiqs.Models;

namespace smiqs.ViewModels.Dashboard
{
    /// <summary>
    /// ViewModel for Home page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class HomePageViewModel : BaseViewModel
    {
        public string SSA = "";
        public HomePageViewModel()
        {
           var TM = new Telemetry("8997103101017701450");
            SSA = TM.SSA.ToString();
        } 
    }
}
