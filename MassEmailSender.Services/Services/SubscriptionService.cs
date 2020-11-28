using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;

namespace MassEmailSender.Services
{

    public static class SubscriptionService
    {
        private static FileSystemDb<CompanySubscriber> _DbCompany = new FileSystemDb<CompanySubscriber>();
        private static FileSystemDb<UserSubscriber> _userDB = new FileSystemDb<UserSubscriber>();

        public static void AddSubscriptionsAgain(this CompanySubscriber currentCompany)  
        {

            foreach (int subscriberID in currentCompany.IdSubscriptionList)
            {
                UserSubscriber subscriber = _userDB.GetById(subscriberID);
                subscriber.SubscribesForPromotion(currentCompany);

            }
        }
        public static void AddSubscriptionsAgain(this UserSubscriber currentUser)   
        {
            foreach (int companiesID in currentUser.IdSubscriptionList) 
            {
                CompanySubscriber company = _DbCompany.GetById(companiesID);
                currentUser.SubscribesForPromotion(company);
            }
        }

        public static List<CompanySubscriber> MySusbscriptionList(this Subscriber currentUser)
        {
            var companyList = new List<CompanySubscriber>();
            
            int i = 1;
            foreach (int companiesID in currentUser.IdSubscriptionList)
            {
                CompanySubscriber company = _DbCompany.GetById(companiesID);
                companyList.Add(company);
                Console.WriteLine($"{i} {company.CompanyName} {company.ShowProfileDiscription()}");
                i++;
            }
            return companyList;

        }
        public static void SubscribesForPromotion(this UserSubscriber user, CompanySubscriber company)
        {

            company.PromotionUserBase += user.ReadPromotion;
            Console.WriteLine("A person susbscribed");


            if (!company.IdSubscriptionList.Contains(user.Id))
            {
                company.IdSubscriptionList.Add(user.Id);
                _DbCompany.Update(company);
            }
            if (!user.IdSubscriptionList.Contains(company.Id))
            {
                user.IdSubscriptionList.Add(company.Id);
                _userDB.Update(user);
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
                _DbCompany.Update(company);
            }
            if (user.IdSubscriptionList.Contains(company.Id))
            {
                user.IdSubscriptionList.Remove(company.Id);
                _userDB.Update(user);
            }
            company.SuggestionBox.Add(reason);
            Console.ReadLine();
        }
    }
}
