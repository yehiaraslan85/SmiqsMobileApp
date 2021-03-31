using smiqs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace smiqs.ViewModels.Dashboard
{
    public class DataTemplateViewModel : DataTemplateSelector
    {
        private DataTemplate defaulttemplate;

        public DataTemplate DefaultTemplate
        {
            get { return defaulttemplate; }
            set { defaulttemplate = value; }
        }

        private DataTemplate specifictemplate;

        public DataTemplate SpecificTemplate
        {
            get { return specifictemplate; }
            set { specifictemplate = value; }
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as RotatorModel;
            if (message == null)
                return null;
            return message.Image.ToString() == "movie2.png" ? SpecificTemplate : DefaultTemplate;

        }
    }
}