using System;
using System.Collections.Generic;
using System.Threading;

namespace MassEmailSender.Domain.Core.Entities
{
    public class CompanySubscriber : Subscriber
    {
        public string CompanyName { get; set; }
        public List<string> SuggestionBox { get; set; }
        public List<string> SubscribedEmails { get; set; }

        public CompanySubscriber()
        {
            Role = SubscriptionType.Company;
            SuggestionBox = new List<string>();
            SubscribedEmails = new List<string>();
            IdSubscriptionList = new List<int>();
            ProfileDiscription = null;
        }

        public override string ShowProfileDiscription()
        {
            if (ProfileDiscription != null)
            {
                return ProfileDiscription;
            }
            else
            {
                return $"(Here at {CompanyName} we provide the latest and best {CurrentProduct}!)";
            }
        }
        public void ReadSuggestions() //future implementation
        {
            Console.WriteLine($"Suggestions for {CompanyName}: ");
            foreach (string suggestion in SuggestionBox)
            {
                Console.WriteLine(suggestion);
            }
        }

        public delegate void PromotionSender(ProductType product);
        public event PromotionSender PromotionUserBase;
        public void SendInAppPromotions()
        {
            Console.Clear();
            Console.WriteLine("---------------");
            Console.WriteLine($"{CompanyName} is sending a promotion");
            Console.WriteLine("Sending....");
            Thread.Sleep(3000);
            if (PromotionUserBase == null)
            {
                Console.WriteLine("You have no subscribers");
            }
            else
            {
                PromotionUserBase(CurrentProduct);
            }
            Console.ReadLine();
        }
    }
}
