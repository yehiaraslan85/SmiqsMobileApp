using smiqs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace smiqs.ViewModels.Helpers
{
    public interface IFirestore
    {
        bool InsertSubscription(Subscription subscription);
        Task<bool> DeleteSubscription(Subscription subscription);
        Task<bool> UpdateSubscription(Subscription subscription);
        Task<IList<Subscription>> ReadSubscriptions();

    }
    public class DatabaseHelper 
    {
        private static IFirestore firestore = DependencyService.Get<IFirestore>();

        public Task<bool> DeleteSubscription(Subscription subscription)
        {
            return firestore.DeleteSubscription(subscription);
        }

        public bool InsertSubscription(Subscription subscription)
        {
            return firestore.InsertSubscription(subscription);

        }

        public Task<IList<Subscription>> ReadSubscriptions()
        {
            return firestore.ReadSubscriptions();

        }

        public Task<bool> UpdateSubscription(Subscription subscription)
        {
            return firestore.UpdateSubscription(subscription);

        }
    }
}
