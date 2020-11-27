using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;

namespace MassEmailSender.Services
{

    public static class Subscribe
    {
        public static IUiService _uiSrvc = new UiService();
        private static FileSystemDb<CompanySubscriber> companyDB = new FileSystemDb<CompanySubscriber>();
        private static FileSystemDb<UserSubscriber> userDB = new FileSystemDb<UserSubscriber>();

        public static void AddSubscribersAgain(this CompanySubscriber currentCompany)    //create 1 method for both
        {

            foreach (int subscriberID in currentCompany.IdSubscriptionList)
            {
                UserSubscriber subscriber = userDB.GetById(subscriberID);
                subscriber.SubscribesForPromotion(currentCompany);

            }
        }
        public static void AddCompaniesAgain(this UserSubscriber currentUser)     //create 1 method for both
        {
            foreach (int companiesID in currentUser.IdSubscriptionList) //map or third method
            {
                CompanySubscriber company = companyDB.GetById(companiesID);
                currentUser.SubscribesForPromotion(company);
            }
        }

        public static void EditDiscription(this Subscriber currentCompany)
        {
            Console.WriteLine("Edit your Discription:");
            var company = (CompanySubscriber)currentCompany;
            string discription = Console.ReadLine();

            company.PromotionText = $"({discription})";
            companyDB.Update(company);
            Console.Clear();
            Console.WriteLine("Discription Updated.");
            Console.ReadLine();
        }


        public static void MySusbscriptionList(this Subscriber currentUser)
        {
            var companyList = new List<CompanySubscriber>();
            Console.Clear();
            Console.WriteLine("Your company subcriptions: ");
            foreach (int companiesID in currentUser.IdSubscriptionList)
            {
                CompanySubscriber company = companyDB.GetById(companiesID);
                companyList.Add(company);
                Console.Write($"{company.CompanyName}");
                company.Discription();
            }
            Console.ReadLine();

        }
        public static void SubscribesForPromotion(this UserSubscriber user, CompanySubscriber company)
        {

            company.PromotionUserBase += user.ReadPromotion;
            Console.WriteLine("A person susbscribed");


            if (!company.IdSubscriptionList.Contains(user.Id))
            {
                company.IdSubscriptionList.Add(user.Id);
                companyDB.Update(company);
            }
            if (!user.IdSubscriptionList.Contains(company.Id))
            {
                user.IdSubscriptionList.Add(company.Id);
                userDB.Update(user);
            }
            company.SubscribedEmails.Add(user.Email);
            Console.ReadLine();
        }
        public static void UnsubscribesForPromotion(this UserSubscriber user, CompanySubscriber company, string reason)
        {
            Console.WriteLine("A person unsubscribed");

            company.PromotionUserBase -= user.ReadPromotion;

            if (company.IdSubscriptionList.Contains(user.Id))
            {
                company.IdSubscriptionList.Remove(user.Id);
                companyDB.Update(company);
            }
            if (user.IdSubscriptionList.Contains(company.Id))
            {
                user.IdSubscriptionList.Remove(company.Id);
                userDB.Update(user);
            }
            company.SuggestionBox.Add(reason);
            Console.ReadLine();
        }

    }
}
