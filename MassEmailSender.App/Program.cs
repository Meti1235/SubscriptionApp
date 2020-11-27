using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services;
using MassEmailSender.Services.GmailAPI;
using System;
using System.Collections.Generic;

namespace MassEmailSender.App
{

    //1. Registraion for 2 accounts types Subscriber/Company DONE*****
    //2. Login to show 2 different layouts //DONE*****
    //3. Subscriber Layout should show all the companies and option for subscribing  //DONE*****
    //4. Company Layout should give them option to edit their product description and  //DONE*****
    //4.5 Multy Option for sending out advertising to all subscribers DONEish******
    //5. put users and company in a dictionary inside the comapny db so they can auto subscribe when logging in //DONE*****
    //6. add a list of companies which the user is following so they can see all their subscriptions Like 5. above //DONE*****
    //7. fix the broken subscription for new companies //DONE*****
    //8. create your subscription menue for Users
    //9. Ask the user if they are sure before sending Promotions(write yes/no)
    //10. Give the option to change products to the users and companies
    //11. Give multiple option for Products
    //12. Add the Unsubscribe option //HALFDONE*****
    //13. Complete the GmailAPI service //HALFDONE*****
    class Program
    {
        public static IUiService _uiSrvc = new UiService();
        public static ISubscriberService<UserSubscriber> _userSubscribeSrvc = new SubscriberService<UserSubscriber>();
        public static ISubscriberService<CompanySubscriber> _companySubscribeSrvc = new SubscriberService<CompanySubscriber>();
        public static Subscriber _currentlyLoggedIn;

        #region Seed method
        public static void Seed()
        {
            if (_userSubscribeSrvc.IsDbEmpty() && _companySubscribeSrvc.IsDbEmpty())
            {
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Muhamed", LastName = "Sakipi", Username = "meti20", Password = "meti123", CurrentProduct = ProductType.Food, Email = "sakipi.m@outlook.com", Age = 27, Id = 1, IdSubscriptionList = new List<int> { 4 } });
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Muh", LastName = "Sakipi", Username = "meti30", Password = "meti123", CurrentProduct = ProductType.Cosmetics, Email = "sakipi.m@outlook.com", Age = 27, Id = 2 });
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Muha", LastName = "Sakipi", Username = "meti40", Password = "meti123", CurrentProduct = ProductType.Electronics, Email = "sakipi.m@outlook.com", Age = 27, Id = 3 });
                _companySubscribeSrvc.Register(new CompanySubscriber() { CompanyName = "MetiCompany", FirstName = "TestUser", LastName = "Sakipi", Username = "meti22", Password = "meti123", Age = 27, Email = "sakipi.mu@gmail.com", CurrentProduct = ProductType.Food, Id = 4, IdSubscriptionList = new List<int> { 1, 2, 3 } }); ;
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
                        _currentlyLoggedIn = _uiSrvc.UserLogIn();
                    }
                    else
                    {
                        _currentlyLoggedIn = _uiSrvc.UserRegister();

                    }
                    if (_currentlyLoggedIn == null) continue;
                    _uiSrvc.Welcome(_currentlyLoggedIn);
                }

                int mainMenuChoice = _uiSrvc.MainMenu(_currentlyLoggedIn.Role);
                string mainMenuItem = _uiSrvc.MainMenuItems[mainMenuChoice - 1];
                switch (mainMenuItem)
                {
                    case "Promotion Discription":
                        _currentlyLoggedIn.EditDiscription();
                        break;
                    case "Market Offers":
                        _uiSrvc.SubscribeMenue(_currentlyLoggedIn);

                        break;
                    case "Send Promotions":
                        _uiSrvc.SendPromotion(_currentlyLoggedIn);
                        break;
                    case "Upgrade to Premium":
                        _uiSrvc.UpgradeToPremium();
                        break;
                    case "Your Subscriptions":
                        _currentlyLoggedIn.MySusbscriptionList();
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
                        Email.ClearCredential();
                        break;
                    default:
                        break;
                }


            }
        }
    }
}
