using Xamarin.Forms;
using smiqs.Models.Detail;
using System.Collections.ObjectModel;
using Xamarin.Forms.Internals;
using smiqs.Models;

namespace smiqs.ViewModels.Detail
{
    /// <summary>
    /// ViewModel for my address page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MyAddressViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<TelemetryEventsModel> AddressDetails { get; set; }
        #endregion

        #region Constructor
        public MyAddressViewModel(TelemetryEventsModel T)
        {
            this.BackCommand = new Command(this.BackButtonClicked);
            this.EditCommand = new Command(this.EditButtonClicked);
            this.DeleteCommand = new Command(this.DeleteButtonClicked);
            this.AddCardCommand = new Command(this.AddCardButtonClicked);
            ObservableCollection<TelemetryEventsModel> AllT = new ObservableCollection<TelemetryEventsModel>();
            AllT.Add(T);
            this.AddressDetails = AllT;
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the back button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void BackButtonClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the edit button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void EditButtonClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the delete button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void DeleteButtonClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the add card button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void AddCardButtonClicked(object obj)
        {
            // Do something
        }
        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public Command BackCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the edit button is clicked.
        /// </summary>
        public Command EditCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the delete button is clicked.
        /// </summary>
        public Command DeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the add card button is clicked.
        /// </summary>
        public Command AddCardCommand { get; set; }

        #endregion
    }
}
