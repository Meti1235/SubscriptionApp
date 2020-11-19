using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;

namespace MassEmailSender.Domain.Core.Services
{

    public static class Subscribe
    {
        public static void SubscribesForPromotion(this UserSubscriber user, CompanySubscriber company)
        {
            Console.WriteLine("A person susbscribed");
            FileSystemDb<CompanySubscriber> companyDB = new FileSystemDb<CompanySubscriber>();
            FileSystemDb<UserSubscriber> userDB = new FileSystemDb<UserSubscriber>();

            company.PromotionUserBase += user.ReadPromotion;

            if (!company.IdSubscribers.Contains(user.Id))
            {
                company.IdSubscribers.Add(user.Id);
                companyDB.Update(company);
            }
            if (!user.IdCompanies.Contains(company.Id))
            {
                user.IdCompanies.Add(company.Id);
                userDB.Update(user);
            }

            Console.ReadLine();
            company.SubscribedEmails.Add(user.Email);
        }
        public static void AddSubscribers(this CompanySubscriber currentCompany)    //create 1 method for both
        {
            FileSystemDb<UserSubscriber> userDB = new FileSystemDb<UserSubscriber>();


            foreach (int subscriberID in currentCompany.IdSubscribers)
            {
                UserSubscriber subscriber = userDB.GetById(subscriberID);

                subscriber.SubscribesForPromotion(currentCompany);

            }
            Console.WriteLine("Subscriptions were added again for the Company");
            Console.ReadLine();
        }
        public static void AddCompanies(this UserSubscriber currentUser)     //create 1 method for both
        {
            FileSystemDb<CompanySubscriber> userDB = new FileSystemDb<CompanySubscriber>();


            foreach (int companiesID in currentUser.IdCompanies)
            {
                CompanySubscriber company = userDB.GetById(companiesID);
                currentUser.SubscribesForPromotion(company);
            }
            Console.WriteLine("Subscriptions were added again for the User");
            Console.ReadLine();
        }
        public static void EditDiscription(this CompanySubscriber currentCompany)
        {
            FileSystemDb<CompanySubscriber> userDB = new FileSystemDb<CompanySubscriber>();

            Console.WriteLine("Edit your Discription");
            string discription = Console.ReadLine();

            currentCompany.PromotionText = $"( {discription} )";
            userDB.Update(currentCompany);
            Console.WriteLine("Discription Updated.");
        }

        public static void UserSubscriptionList(this UserSubscriber currentUser)
        {
            FileSystemDb<CompanySubscriber> userDB = new FileSystemDb<CompanySubscriber>();
            var companyList = new List<CompanySubscriber>();

            foreach (int companiesID in currentUser.IdCompanies)
            {
                CompanySubscriber company = userDB.GetById(companiesID);
                companyList.Add(company);
                Console.WriteLine(company.CompanyName);
            }
       
            Console.ReadLine();
        }
        public static void UnsubscribesForPromotion(this UserSubscriber user, CompanySubscriber company, string reason)
        {
            Console.WriteLine("A person unsubscribed");
            FileSystemDb<CompanySubscriber> companyDB = new FileSystemDb<CompanySubscriber>();
            FileSystemDb<UserSubscriber> userDB = new FileSystemDb<UserSubscriber>();

            company.PromotionUserBase -= user.ReadPromotion;

            if (company.IdSubscribers.Contains(user.Id))
            {
                company.IdSubscribers.Remove(user.Id);
                companyDB.Update(company);
            }
            if (user.IdCompanies.Contains(company.Id))
            {
                user.IdCompanies.Remove(company.Id);
                userDB.Update(user);
            }
            company.SuggestionBox.Add(reason);
            Console.ReadLine();
        }

    }
}
