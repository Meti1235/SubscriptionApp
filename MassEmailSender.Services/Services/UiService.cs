using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Domain.Core.Services;
using MassEmailSender.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MassEmailSender.Services
{
    public class UiService : IUiService
    {
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
            AccountMenuItems = new List<string>() { "Change Info", "Check Subscription", "Change Password" };
            return ChooseMenu(AccountMenuItems);
        }

        public void SendPromotions(CompanySubscriber company)
        {
            Console.Clear();
            Console.WriteLine("promotions!!!");
            company.SendPromotions();

            Console.ReadLine();
        }

        public void UpgradeToPremium()
        {
            Console.Clear();
            Console.WriteLine("Upgrade to premium to get these features:");
            Console.WriteLine("* Live trainings");
            Console.WriteLine("* Newsletter in your mail");
            Console.WriteLine("* Discounts at sports equipment stores");
            Console.ReadLine();
        }

        public int SubscribeMenue(UserSubscriber currentUser)
        {
            while (true)
            {
                Console.Clear();
                List<CompanySubscriber> allCompanies = _DbCompany.GetAll().ToList();
                for (int i = 0; i < allCompanies.Count; i++)
                {
                    Console.Write($"{i + 1}) {allCompanies[i].CompanyName} ");
                    allCompanies[i].ReadPromotion(allCompanies[i].CurrentPromotion);
                }

                int choice = ValidationHelper.ValidateNumber(Console.ReadLine(), allCompanies.Count);
                CompanySubscriber currentCompany = allCompanies[choice - 1];
                if (choice == -1)
                {
                    MessageHelper.PrintMessage("[Error] Input incorrect. Please try again.", ConsoleColor.Red);
                    continue;
                }
                currentUser.SubscribesForPromotion(currentCompany);
                return choice;
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
