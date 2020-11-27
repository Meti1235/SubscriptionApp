using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MassEmailSender.Services
{
    public class UiService : IUiService
    {
        public static ISubscriberService<UserSubscriber> _userSubscribeSrvc = new SubscriberService<UserSubscriber>();
        public static ISubscriberService<CompanySubscriber> _companySubscribeSrvc = new SubscriberService<CompanySubscriber>();
        public FileSystemDb<CompanySubscriber> _DbCompany = new FileSystemDb<CompanySubscriber>();
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
                // Change Info
                Console.WriteLine("Enter new First Name:");
                string firstName = Console.ReadLine();
                Console.WriteLine("Enter new Last Name:");
                string lastName = Console.ReadLine();
                switch (currentUser.Role)
                {
                    case SubscriptionType.User:
                        _userSubscribeSrvc.ChangeInfo(currentUser.Id, firstName, lastName);
                        break;
                    case SubscriptionType.Company:
                        _companySubscribeSrvc.ChangeInfo(currentUser.Id, firstName, lastName);
                        break;
                }
            }
            else if (accountChoice == 2)
            {
                // Check Subscription  
                Console.WriteLine($"Your subscription is: {currentUser.Role}");
                Console.ReadLine();
            }
            else if (accountChoice == 3)
            {
                // Change Password
                Console.WriteLine("Enter old password:");
                string oldPass = Console.ReadLine();
                Console.WriteLine("Enter new password:");
                string newPass = Console.ReadLine();
                switch (currentUser.Role)
                {
                    case SubscriptionType.User:
                        _userSubscribeSrvc.ChangePassword(currentUser.Id, oldPass, newPass);
                        break;
                    case SubscriptionType.Company:
                        _companySubscribeSrvc.ChangePassword(currentUser.Id, oldPass, newPass);
                        break;
                }
            }
        }
        public Subscriber UserRegister() //move this method
        {
            int registerChoice = RegisterMenu();
            SubscriptionType role = (SubscriptionType)registerChoice;
            switch (role)
            {
                case SubscriptionType.User:
                    var newUser = _userSubscribeSrvc.CreateEntity(new UserSubscriber());
                    var registeredUser = _userSubscribeSrvc.Register(newUser);
                    return registeredUser;

                case SubscriptionType.Company:
                    var newCompany = _companySubscribeSrvc.CreateEntity(new CompanySubscriber());
                    var registeredCompany = _companySubscribeSrvc.Register(newCompany);
                    return registeredCompany;
            }
            throw new ApplicationException("This error is not possible");
        }

        public int PromotionMenue()
        {
            List<string> menuItems = new List<string>() { "Send InApp Promotion", "Send Email Promotion" };
            return ChooseMenu(menuItems);
        }

        public void SendPromotion(Subscriber currentCompany) //move this method
        {
            int promotionChoice = PromotionMenue();

            var compamy = (CompanySubscriber)currentCompany;

            if (promotionChoice == 1)
            {
                compamy.SendInAppPromotions();
            }
            else
            {
                _companySubscribeSrvc.SendEmailPromotion();
            }
        }

        public Subscriber UserLogIn()  //move this method
        {
            int roleChoice = RoleMenu();

            SubscriptionType role = (SubscriptionType)roleChoice;

            Console.Clear();  //refactor this into a menue 
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            switch (role)
            {
                case SubscriptionType.User:
                    UserSubscriber _currentUser = _userSubscribeSrvc.LogIn(username, password);
                    if (_currentUser != null) _currentUser.AddCompaniesAgain();
                    return _currentUser;

                case SubscriptionType.Company:
                    CompanySubscriber _currentCompany = _companySubscribeSrvc.LogIn(username, password);
                    if (_currentCompany != null) _currentCompany.AddSubscribersAgain();
                    return _currentCompany;
            }
            throw new ApplicationException("This error is not possible");
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
                    MainMenuItems.Insert(0, "Promotion Discription");
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
            AccountMenuItems = new List<string>() { "Change UserName", "Check Subscription", "Change Password" };
            return ChooseMenu(AccountMenuItems);
        }

        public void UpgradeToPremium()
        {
            Console.Clear();
            Console.WriteLine("Upgrade to premium to get these features:");
            Console.WriteLine("* Newsletter in your mail");
            Console.ReadLine();
        }

        public void SubscribeMenue(Subscriber currentUser)
        {
            while (true)
            {
                Console.Clear();
                List<CompanySubscriber> allCompanies = _DbCompany.GetAll().ToList();
                for (int i = 0; i < allCompanies.Count; i++)
                {
                    Console.Write($"{i + 1}) {allCompanies[i].CompanyName} ");
                    allCompanies[i].Discription();
                }

                int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), allCompanies.Count);
                CompanySubscriber currentCompany = allCompanies[choice - 1];
                if (choice == -1)
                {
                    MessageHelper.PrintMessage("[Error] Input incorrect. Please try again.", ConsoleColor.Red);
                    continue;
                }

                var user = (UserSubscriber)currentUser;
                user.SubscribesForPromotion(currentCompany);
                Console.Clear();
                Console.WriteLine("You succesfully Subscribed.");
                Console.ReadLine();
            }
        }

        public int ChooseEntiiesMenu<T>(List<T> entities) where T : IBaseEntity
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Enter a number to choose one of the following:");
                for (int i = 0; i < entities.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {entities[i].Info()}");
                }
                int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), entities.Count);
                if (choice == -1)
                {
                    MessageHelper.PrintMessage("[Error] Input incorrect. Please try again", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                return choice;
            }
        }

        public void Welcome(Subscriber entity)
        {
            Console.Clear();
            Console.WriteLine($"Welcome our app {entity.Username}!");
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
