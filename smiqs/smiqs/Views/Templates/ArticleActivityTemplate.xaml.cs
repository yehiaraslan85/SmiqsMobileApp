using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace smiqs.Views.Templates
{
    /// <summary>
    /// article activity template.
    /// </summary>
    [Preserve(AllMembers = true)]

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleActivityTemplate : StackLayout
    {
        /// <summary>
        /// Bindable property to set the parent bindingcontext.
        /// </summary>
        public static BindableProperty ParentBindingContextProperty =
         BindableProperty.Create(nameof(ParentBindingContext), typeof(object),
         typeof(ArticleActivityTemplate), null);

        /// <summary>
        /// Gets or sets the parent bindingcontext.
        /// </summary>
        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set { SetValue(ParentBindingContextProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleActivityTemplate"/> class.
        /// </summary>
        public ArticleActivityTemplate()
        {
            InitializeComponent();
        }
        private async void State_StateChangingA(object sender, SwitchStateChangingEventArgs e)
        {
            if (e.NewValue == true)
            {
              //  this.sfSwitchA.IsBusy = true;
            //    await PostRequestA(true, "COMMANDVALVESTATUSA", true);
            }
            else
            {
            //    this.sfSwitchA.IsBusy = true;
            //    await PostRequestA(false, "COMMANDVALVESTATUSA", false);
            }
           // this.sfSwitchA.IsBusy = false;
        }
    }
}