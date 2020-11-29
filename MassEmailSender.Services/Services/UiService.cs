using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MassEmailSender.Services
{
    public class UiService : IUiService
    {
        private static FileSystemDb<CompanySubscriber> _companyDb = new FileSystemDb<CompanySubscriber>();
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

        public int ChooseMenu(List<CompanySubscriber> items)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter a number to choose one of the following:");

                MessageHelper.PrintCompanyList(items);

                int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), items.Count);
                if (choice == -1)
                {
                    MessageHelper.PrintMessage("[Error] Input incorrect. Please try again.", ConsoleColor.Red);
                    continue;
                }
                return choice;
            }
        }

        public void MyAccountMenu(Subscriber currentUser)
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
                        _userAccountSrvc.EditDescription((UserSubscriber)currentUser);
                        break;
                    case SubscriptionType.Company:
                        _companyAccountSrvc.EditDescription((CompanySubscriber)currentUser);
                        break;
                }
            }
            else if (accountChoice == 4)
            {
                Console.WriteLine($"Your subscription is: {currentUser.Role}");
                Console.ReadLine();
            }
        }

        public int PromotionMenu()
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

        public int AccountMenu(SubscriptionType role)
        {
            AccountMenuItems = new List<string>() { "Change Name and Surname", "Change Password", "Edit Description", "Check Subscription" };
            return ChooseMenu(AccountMenuItems);
        }

        public int MainMenu(SubscriptionType role)
        {
            MainMenuItems = new List<string>() { "Upgrade to Premium", "Account", "Log Out" };
            switch (role)
            {
                case SubscriptionType.Company:

                    MainMenuItems.Insert(0, "Send Promotions");
                    MainMenuItems.Insert(0, "Your Subscribers");
                    break;
                case SubscriptionType.User:

                    MainMenuItems.Insert(0, "Market Offers");
                    MainMenuItems.Insert(0, "Your Subscriptions");
                    break;
            }
            return ChooseMenu(MainMenuItems);
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

        public void SubscribeMenu(UserSubscriber currentUser)
        {
            while (true)
            {
                Console.Clear();
                List<CompanySubscriber> allCompanies = _companyDb.GetAll().ToList();
                List<int> myCompanyIds = currentUser.UsersSusbscriptionList().Select(x => x.Id).ToList();

                List<CompanySubscriber> availableCompanies = new List<CompanySubscriber>();
                foreach (var company in allCompanies)
                {
                    if (!myCompanyIds.Contains(company.Id))
                    {
                        availableCompanies.Add(company);
                    }
                }

                if (availableCompanies.Count == 0)
                {
                    Console.WriteLine("There are no more subscriptions available");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("Company subcriptions available:");
                MessageHelper.PrintCompanyList(availableCompanies);

                Console.WriteLine("Would you like to Subscribe? ( Y/N )");
                string question = Console.ReadLine();

                if (question.ToLower() == "y")
                {
                    Console.Clear();

                    int choice = ChooseMenu(availableCompanies);

                    CompanySubscriber currentCompany = availableCompanies[choice - 1];

                    currentUser.SubscribesForPromotion(currentCompany);
                    Console.Clear();
                    Console.WriteLine("You succesfully Subscribed.");
                    Console.ReadLine();
                }
                if (question.ToLower() == "n") break;
            }
        }

        public void UnSubscribeMenu(UserSubscriber currentUser)
        {
            while (true)
            {
                Console.Clear();
                List<CompanySubscriber> companySubscriptions = currentUser.UsersSusbscriptionList();

                if (companySubscriptions.Count == 0)
                {
                    Console.WriteLine("There are no more subscriptions available");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("Your company subcriptions: ");
                MessageHelper.PrintCompanyList(companySubscriptions);

                Console.WriteLine("Would you like to Unsubscribe? ( Y/N )");
                string question = Console.ReadLine();
                if (question.ToLower() == "y")
                {
                    Console.Clear();
                    Console.WriteLine("Please choose the company you would like to unsubscribe from");

                    int choice = ChooseMenu(companySubscriptions);

                    CompanySubscriber currentCompany = companySubscriptions[choice - 1];

                    Console.WriteLine("Please let us know why you are unsubscribing:");
                    string reason = Console.ReadLine();

                    currentUser.UnsubscribesForPromotion(currentCompany, reason);
                    Console.Clear();
                    Console.WriteLine("You succesfully Unsubscribed.");
                }
                if (question.ToLower() == "n") break;
            }
        }

        public void CompanySubscriberListMenue(CompanySubscriber currentCompany)
        {
            var userList = currentCompany.CompanysSubscriptionList();
            Console.Clear();
            if (userList.Count == 0)
            {
                Console.WriteLine("You have no Subscribers");
            }
            else
            {
                Console.WriteLine("Your current subscribed Users: ");
                for (int i = 0; i < userList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {userList[i].Username} ({userList[i].ShowProfileDescription()})");
                    Console.WriteLine(); //space
                }
            }
            Console.ReadLine();
        }

    }
}
