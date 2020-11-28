using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MassEmailSender.Services
{
    public class UiService : IUiService
    {
        private static FileSystemDb<CompanySubscriber> _DbCompany = new FileSystemDb<CompanySubscriber>();
        private static IAccountService<UserSubscriber> _userAccountSrvc = new AccountService<UserSubscriber>();
        private static IAccountService<CompanySubscriber> _companyAccountSrvc = new AccountService<CompanySubscriber>();

        public List<string> MainMenuItems { get; set; }
        public List<string> AccountMenuItems { get; set; }

        public int ChooseMenu<T>(List<T> items)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter a number to choose one of the following:");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {items[i]}");
                }

                int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), items.Count);
                if (choice == -1)
                {
                    MessageHelper.PrintMessage("[Error] Input incorrect. Please try again.", ConsoleColor.Red);
                    continue;
                }
                return choice;
            }
        }

        public void MyAccountMenue(Subscriber currentUser)
        {
            int accountChoice = AccountMenu(currentUser.Role);
            Console.Clear();
            if (accountChoice == 1)
            {

                Console.WriteLine("Enter new First Name:");
                string firstName = Console.ReadLine();
                Console.WriteLine("Enter new Last Name:");
                string lastName = Console.ReadLine();
                switch (currentUser.Role)
                {
                    case SubscriptionType.User:
                        _userAccountSrvc.ChangeInfo(currentUser.Id, firstName, lastName);
                        break;
                    case SubscriptionType.Company:
                        _companyAccountSrvc.ChangeInfo(currentUser.Id, firstName, lastName);
                        break;
                }
            }
            else if (accountChoice == 2)
            {
                // Change Password
                Console.WriteLine("Enter old password:");
                string oldPass = Console.ReadLine();
                Console.WriteLine("Enter new password:");
                string newPass = Console.ReadLine();
                switch (currentUser.Role)
                {
                    case SubscriptionType.User:
                        _userAccountSrvc.ChangePassword(currentUser.Id, oldPass, newPass);
                        break;
                    case SubscriptionType.Company:
                        _companyAccountSrvc.ChangePassword(currentUser.Id, oldPass, newPass);
                        break;
                }
            }
            else if (accountChoice == 3)
            {
                switch (currentUser.Role)
                {
                    case SubscriptionType.User:
                        _userAccountSrvc.EditDiscription((UserSubscriber)currentUser);
                        break;
                    case SubscriptionType.Company:
                        _companyAccountSrvc.EditDiscription((CompanySubscriber)currentUser);
                        break;
                }
            }
            else if (accountChoice == 4)
            {
                Console.WriteLine($"Your subscription is: {currentUser.Role}");
                Console.WriteLine($"Our premium offer is $12.95 a month.");
                Console.ReadLine();
            }
        }

        public int PromotionMenue()
        {
            List<string> menuItems = new List<string>() { "Send InApp Promotion", "Send Email Promotion" };
            return ChooseMenu(menuItems);
        }

        public int LogInMenu()
        {
            List<string> menuItems = new List<string>() { "Log In", "Regsiter" };
            return ChooseMenu(menuItems);
        }

        public int RegisterMenu()
        {
            List<string> menuItems2 = new List<string>() { "User", "Company" };
            return ChooseMenu(menuItems2);
        }

        public int RoleMenu()
        {
            List<string> menuItems = Enum.GetNames(typeof(SubscriptionType)).ToList();
            return ChooseMenu(menuItems);
        }

        public int MainMenu(SubscriptionType role)
        {
            MainMenuItems = new List<string>() { "Upgrade to Premium", "Account", "Log Out" };
            switch (role)
            {
                case SubscriptionType.Company:

                    MainMenuItems.Insert(0, "Send Promotions");
                    break;
                case SubscriptionType.User:

                    MainMenuItems.Insert(0, "Market Offers");
                    MainMenuItems.Insert(0, "Your Subscriptions");
                    break;
            }
            return ChooseMenu(MainMenuItems);
        }

        public int AccountMenu(SubscriptionType role)
        {
            AccountMenuItems = new List<string>() { "Change UserName", "Change Password", "Edit Discription", "Check Subscription" };
            return ChooseMenu(AccountMenuItems);
        }

        public void SubscribeMenue(Subscriber currentUser)
        {
            while (true)
            {
                Console.Clear();
                List<CompanySubscriber> allCompanies = _DbCompany.GetAll().ToList();
                for (int i = 0; i < allCompanies.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {allCompanies[i].CompanyName} {allCompanies[i].ShowProfileDiscription()} ");
                }

                int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), allCompanies.Count);
                if (choice == -1)
                {
                    MessageHelper.PrintMessage("[Error] Input incorrect. Please try again.", ConsoleColor.Red);
                    continue;
                }

                CompanySubscriber currentCompany = allCompanies[choice - 1];

                var user = (UserSubscriber)currentUser;
                user.SubscribesForPromotion(currentCompany);
                Console.Clear();
                Console.WriteLine("You succesfully Subscribed.");
                Console.ReadLine();
                break;
            }
        }
        public void UnSubscribeMenue(Subscriber currentUser)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your company subcriptions: ");
                List<CompanySubscriber> companySubscriptions = currentUser.MySusbscriptionList();

               

                Console.WriteLine("Would you like to Unsubscribe? ( Y/N )");
                string question = Console.ReadLine();
                if (question.ToLower() == "y")
                {
                    Console.Clear();
                    Console.WriteLine("Please choose the company you would like to unsubscribe from");
                    currentUser.MySusbscriptionList();
                    int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), companySubscriptions.Count);
                    if (choice == -1)
                    {
                        MessageHelper.PrintMessage("[Error] Input incorrect. Please try again.", ConsoleColor.Red);
                        continue;
                    }

                    CompanySubscriber currentCompany = companySubscriptions[choice - 1];
                    var user = (UserSubscriber)currentUser;

                    Console.WriteLine("Please let us know why you are unsubscribing:");
                    string reason = Console.ReadLine();

                    user.UnsubscribesForPromotion(currentCompany, reason);
                    Console.Clear();
                    Console.WriteLine("You succesfully Unsubscribed.");
                }
                if (question.ToLower() == "n") break;
            }
        }


        public void WelcomeMenu(Subscriber entity)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to our app {entity.FirstName}!");
            switch (entity.Role)
            {
                case SubscriptionType.User:
                    Console.WriteLine($"We wish you happy subscribing.");
                    break;
                case SubscriptionType.Company:
                    Console.WriteLine("We are so glad you are part of our community.");
                    break;
            }
            Console.ReadLine();
        }

    }



}
