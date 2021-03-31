using smiqs.Models;
using smiqs.Views.Navigation;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace smiqs.ViewModels.Calendar
{
    public class CalendarPageViewModel : BaseViewModel
    {
        private ObservableCollection<tblEvents> listItems;

        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();
        public SelectionRange SelectedRange { get; set; }

        private Command<object> tapCommand;
        private INavigation navigation;

        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }

        public INavigation Navigation
        {
            get { return navigation; }
            set { navigation = value; }
        }

        public ObservableCollection<tblEvents> ListItems
        {
            get
            {
                return this.listItems;
            }

            set
            {
                this.listItems = value;
                this.NotifyPropertyChanged();
            }
        }
        public CalendarPageViewModel(string iccid,string sensor)
        {
            getAllIrrigations(iccid, sensor);
        }
        public CalendarPageViewModel(INavigation _navigation)
        {
            navigation = _navigation;
            tapCommand = new Command<object>(OnTapped);
        }
        private void OnTapped(object obj)
        {
          //  var newPage = new SettingsPage();
          //  newPage.BindingContext = obj;
          //  Navigation.PushAsync(newPage);
        }
        private void getAllIrrigations(string iccid, string sensor)
        {
            var result = App.EventsDatabase.getAllActiveTelemetry(iccid, sensor);
            var AllTelemetry = new ObservableCollection<tblEvents>();
            foreach (var r in result.Result)
            {
                CalendarInlineEvent event1 = new CalendarInlineEvent();
                r.Duration = (int.Parse(r.elapsedtime) / 60).ToString() + " Minutes.";
                int liters = int.Parse(r.flowml) / 5;
                r.flowL = liters;
                r.strflowL = liters + " Liters";
                float galons = (float)(liters * 0.2641);
                double tarrif = ((galons * 4) / 100) + ((galons * 0.6) / 100);
                r.Cost = (float)tarrif;
                r.Cost = (float)Math.Round(r.Cost, 2);
                AllTelemetry.Add(r);
                event1.StartTime = r.eventDate;
                event1.EndTime = r.eventDate.AddSeconds(-int.Parse(r.elapsedtime));
                event1.Subject = "Irrigation";
                event1.Color = Color.Green;
                CalendarInlineEvents.Add(event1);
            }
            ListItems = AllTelemetry;
            
           

           

            
            
        }
    }
}
