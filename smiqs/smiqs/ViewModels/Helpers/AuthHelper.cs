using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace smiqs.ViewModels.Helpers
{
    public interface IAuth
    {
        Task<bool> RegisterUser(string name, string email, string password);
        Task<bool> AuthenticateUser(string email, string password);

        Task<bool> IsEmailExists(string email);
        bool IsAuthenticated();
        string GetCurrentUserId();

    }

    public class Auth
    {
        private static IAuth auth = DependencyService.Get<IAuth>();
       
        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
           
            try { 
            return await auth.RegisterUser(name,email,password);
            }
            catch(Exception ex)
            {
               await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }
        }
       
        public static async Task<bool> AuthenticateUser(string email, string password)
        {
            try { 
            return await auth.AuthenticateUser(email, password);
               
            }
            catch(Exception ex)
            {
                
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }
        }
    public static bool IsAuthenticated()
        {
            return auth.IsAuthenticated();
        }
        public static async Task<bool> IsEmailExists(string email)
        {
           

            try
            {
                return await auth.IsEmailExists(email);
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }
        }
        public static string GetCurrentUserId()
        {
            
            return auth.GetCurrentUserId();
        }
        
    }
}
