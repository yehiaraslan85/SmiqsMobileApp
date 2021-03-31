﻿using Plugin.FacebookClient;
using Plugin.GoogleClient;
using smiqs.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace smiqs.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomNavigationPage : TabbedPage
    {
        
        public BottomNavigationPage(NetworkAuthData networkAuthData)
        {
            BindingContext = networkAuthData;
            InitializeComponent();
        }
        public BottomNavigationPage()
        {
            InitializeComponent();
        }
        async void OnLogout(object sender, System.EventArgs e)
        {
            if (BindingContext is NetworkAuthData data)
            {
                switch (data.Name)
                {
                    case "Facebook":
                        CrossFacebookClient.Current.Logout();
                        break;
                    case "Google":
                        CrossGoogleClient.Current.Logout();
                        break;
                }

                await Navigation.PopModalAsync();
            }
        }
       
    }
}