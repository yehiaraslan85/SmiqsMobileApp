using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace smiqs.Views.Templates
{
    /// <summary>
    /// Navigation tile template.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseChartTemplate : Grid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseChartTemplate"/> class.
        /// </summary>
        public ExpenseChartTemplate()
        {
            InitializeComponent();
     
        } 

        private void rotator_SelectedIndexChanged(object sender, Syncfusion.SfRotator.XForms.SelectedIndexChangedEventArgs e)
        {
           // var x = e.Index;
           
        }
    }
}