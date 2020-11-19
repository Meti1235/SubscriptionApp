using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Domain.Core.Services;
using MassEmailSender.Services;
using System;
using System.Collections.Generic;

namespace MassEmailSender.App
{

    //1. Registraion for 2 accounts types Subscriber/Company DONE*********
    //2. Login to show 2 different layouts //DONE*****
    //3. Subscriber Layout should show all the companies and option for subscribing  //DONE*****
    //4. Company Layout should give them option to edit their product description and  //DONE*****
    //4.5 Multy Option for sending out advertising to all subscribers DONEish******
    //5. put users and company in a dictionary inside the comapny db so they can auto subscribe when logging in //DONE******
    //6. add a list of companies which the user is following so they can see all their subscriptions Like 5. above //DONE*****
    //7. fix the broken subscription for new companies
    //8. create your subscription menue for Users
    //9. Ask the user if they are sure before sending Promotions(write yes/no)
    //10. Give the option to change the user and company products
    //11. Give multiple option for Products
    //12. Add the Unsubscribe option //HALFDONE****
    //
    class Program
    {
        public static IUiService _uiSrvc = new UiService();
        public static ISubscriberService<UserSubscriber> _userSubscribeSrvc = new SubscriberService<UserSubscriber>();
        public static ISubscriberService<CompanySubscriber> _companySubscribeSrvc = new SubscriberService<CompanySubscriber>();
        public static Subscriber _currentlyLoggedIn;
        public static UserSubscriber _currentUser;
        public static CompanySubscriber _currentCompany;
        #region
        public static void Seed()
        {
            if (_userSubscribeSrvc.IsDbEmpty() && _companySubscribeSrvc.IsDbEmpty())
            {
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Muhamed", LastName = "Sakipi", Username = "meti20", Password = "meti123", FavoriteType = ProductType.Food, Email = "sakipi.m@outlook.com", Age = 27, Id = 1, IdCompanies = new List<int> { 4 } });
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Muh", LastName = "Sakipi", Username = "meti30", Password = "meti123", FavoriteType = ProductType.Cosmetics, Email = "sakipi.m@outlook.com", Age = 27, Id = 2 });
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Muha", LastName = "Sakipi", Username = "meti40", Password = "meti123", FavoriteType = ProductType.Electronics, Email = "sakipi.m@outlook.com", Age = 27, Id = 3 });
                _companySubscribeSrvc.Register(new CompanySubscriber() { CompanyName = "MetiCompany", FirstName = "TestUser", LastName = "Sakipi", Username = "meti22", Password = "meti123", Age = 27, Email = "sakipi.mu@gmail.com", CurrentPromotion = ProductType.Food, Id = 4, IdSubscribers = new List<int> { 1, 2, 3 } }); ;
            }
        }
        #endregion
        static void Main(string[] args)
        {
            Seed();
            while (true)
            {

                if (_currentlyLoggedIn == null)
                {
                    int loginChoice = _uiSrvc.LogInMenu();

                    if (loginChoice == 1)
                    {
                        int roleChoice = _uiSrvc.RoleMenu();

                        SubscriptionType role = (SubscriptionType)roleChoice;

                        Console.Clear();  //refactor this into a menue 
                        Console.WriteLine("Enter username:");
                        string username = Console.ReadLine();
                        Console.WriteLine("Enter password:");
                        string password = Console.ReadLine();

                        switch (role)
                        {
                            case SubscriptionType.User:
                                _currentUser = _userSubscribeSrvc.LogIn(username, password);
                                _currentlyLoggedIn = _currentUser;
                                _currentUser.AddCompanies();
                                break;
                            case SubscriptionType.Company:

                                _currentCompany = _companySubscribeSrvc.LogIn(username, password);
                                _currentCompany.AddSubscribers();
                                _currentlyLoggedIn = _currentCompany;

                                break;
                        }

                        if (_currentlyLoggedIn == null) continue;
                    }
                    else
                    {
                        int registerChoice = _uiSrvc.RegisterMenu();
                        if (registerChoice == 1)
                        {
                            Console.Clear(); //refactor this into a menue 
                            UserSubscriber user = new UserSubscriber();
                            Console.WriteLine("Enter first name:");
                            user.FirstName = Console.ReadLine();
                            Console.WriteLine("Enter last name:");
                            user.LastName = Console.ReadLine();
                            Console.WriteLine("Enter age:");
                            user.Age = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter email:");
                            user.Email = Console.ReadLine();
                            Console.WriteLine("Enter username:");
                            user.Username = Console.ReadLine();
                            Console.WriteLine("Enter password:");
                            user.Password = Console.ReadLine();
                            Console.Clear();
                            Console.WriteLine("Enter your favorite product:");
                            string[] productChoice = Enum.GetNames(typeof(ProductType));
                            for (int i = 0; i < productChoice.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}) {productChoice[i]}");
                            }
                            user.FavoriteType = (ProductType)int.Parse(Console.ReadLine()) - 1;
                            _currentUser = _userSubscribeSrvc.Register(user);
                            if (_currentUser == null) continue;
                            _currentlyLoggedIn = _currentUser;
                        }
                        else
                        {
                            Console.Clear(); //refactor this into a menue 
                            CompanySubscriber company = new CompanySubscriber();
                            Console.WriteLine("Enter first name:");
                            company.FirstName = Console.ReadLine();
                            Console.WriteLine("Enter last name:");
                            company.LastName = Console.ReadLine();
                            Console.WriteLine("Enter age:");
                            company.Age = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter company name:");
                            company.CompanyName = Console.ReadLine();
                            Console.WriteLine("Enter email:");
                            company.Email = Console.ReadLine();
                            Console.WriteLine("Enter username:");
                            company.Username = Console.ReadLine();
                            Console.WriteLine("Enter password:");
                            company.Password = Console.ReadLine();
                            Console.Clear();
                            Console.WriteLine("Enter your favorite product:");
                            string[] productChoice = Enum.GetNames(typeof(ProductType));
                            for (int i = 0; i < productChoice.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}) {productChoice[i]}");
                            }
                            company.CurrentPromotion = (ProductType)int.Parse(Console.ReadLine()) - 1;
                            _currentCompany = _companySubscribeSrvc.Register(company);
                            if (_currentCompany == null) continue;
                            _currentlyLoggedIn = _currentCompany;
                        }
                    }
                    _uiSrvc.Welcome(_currentlyLoggedIn);
                }

                int mainMenuChoice = _uiSrvc.MainMenu(_currentlyLoggedIn.Role);
                string mainMenuItem = _uiSrvc.MainMenuItems[mainMenuChoice - 1];
                switch (mainMenuItem)
                {
                    case "Promotion Discription":
                        _currentCompany.EditDiscription();
                        break;
                    case "Market Offers":
                        _uiSrvc.SubscribeMenue(_currentUser);
                        Console.WriteLine("You succesfully Subscribed");
                        Console.ReadLine();
                        break;
                    case "Send Promotions":
                        _currentCompany.SendPromotions();

                        break;
                    case "Upgrade to Premium":
                        _uiSrvc.UpgradeToPremium();
                        break;
                    case "Your Subscriptions":  //refactor this into a menue 
                        _currentUser.UserSubscriptionList();
                        break;
                    case "Account":
                        int accountChoice = _uiSrvc.AccountMenu(_currentlyLoggedIn.Role);
                        Console.Clear();
                        if (accountChoice == 1)
                        {
                            // Change Info
                            Console.WriteLine("Enter new First Name:");
                            string firstName = Console.ReadLine();
                            Console.WriteLine("Enter new Last Name:");
                            string lastName = Console.ReadLine();
                            switch (_currentlyLoggedIn.Role)
                            {
                                case SubscriptionType.User:
                                    _userSubscribeSrvc.ChangeInfo(_currentlyLoggedIn.Id, firstName, lastName);
                                    break;
                                case SubscriptionType.Company:
                                    _companySubscribeSrvc.ChangeInfo(_currentlyLoggedIn.Id, firstName, lastName);
                                    break;
                            }
                        }
                        else if (accountChoice == 2)
                        {
                            // Check Subscription  
                            Console.WriteLine($"Your subscription is: {_currentlyLoggedIn.Role}");
                            Console.ReadLine();
                        }
                        else if (accountChoice == 3)
                        {
                            // Change Password
                            Console.WriteLine("Enter old password:");
                            string oldPass = Console.ReadLine();
                            Console.WriteLine("Enter new password:");
                            string newPass = Console.ReadLine();
                            switch (_currentlyLoggedIn.Role)
                            {
                                case SubscriptionType.User:
                                    _userSubscribeSrvc.ChangePassword(_currentlyLoggedIn.Id, oldPass, newPass);
                                    break;
                                case SubscriptionType.Company:
                                    _companySubscribeSrvc.ChangePassword(_currentlyLoggedIn.Id, oldPass, newPass);
                                    break;
                            }
                        }
                        break;
                    case "Log Out":
                        _currentlyLoggedIn = null;
                        break;
                    default:
                        break;
                }


            }
        }
    }
}
