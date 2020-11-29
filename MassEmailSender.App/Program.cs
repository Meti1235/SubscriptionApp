using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services;
using System.Collections.Generic;

namespace MassEmailSender.App
{
    class Program
    {
        #region To Do List

        //1. Registraion for 2 accounts types Subscriber/Company DONE*****
        //2. Login to show 2 different layouts //DONE*****
        //3. Subscriber Layout should show all the companies and option for subscribing  //DONE*****
        //4. Company Layout should give them option to edit their product description and  //DONE*****
        //4.5 Multy Option for sending out advertising to all subscribers DONE******
        //5. put users and company in a dictionary inside the comapny db so they can auto subscribe when logging in //DONE*****
        //6. add a list of companies which the user is following so they can see all their subscriptions Like 5. above //DONE*****
        //7. fix the broken subscription for new companies //DONE*****
        //8. create your subscription menu for Users //DONE*****
        //9. Ask the user if they are sure before sending Promotions(write yes/no) //DONE*****
        //10. Give the option to change products to the users and companies
        //11. Give multiple option for Products
        //12. Add the Unsubscribe option //DONE*****
        //13. Complete the GmailAPI service //DONE*****
        #endregion

        public static IUiService _uiSrvc = new UiService();
        public static ISubscriberService<UserSubscriber> _userSubscribeSrvc = new SubscriberService<UserSubscriber>();
        public static ISubscriberService<CompanySubscriber> _companySubscribeSrvc = new SubscriberService<CompanySubscriber>();
        public static AccountService _accountSrvc = new AccountService();
        public static Subscriber _currentlyLoggedIn;

        #region Seed method
        public static void Seed()
        {
            if (_userSubscribeSrvc.IsDbEmpty() && _companySubscribeSrvc.IsDbEmpty())
            {
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Meti", LastName = "Sakipi", Username = "meti20", Password = "meti123", CurrentProduct = ProductType.Food, Email = "sakipi.m@outlook.com", Age = 27, Id = 1, IdSubscriptionList = new List<int> { 4, 5 }, ProfileDescription = "I am here to eat delicious food." });
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Denis", LastName = "Sakipi", Username = "denis20", Password = "denis123", CurrentProduct = ProductType.Cosmetics, Email = "aurora55@live.com", Age = 27, Id = 2, IdSubscriptionList = new List<int> { 4, 5 }, ProfileDescription = "I am just looking around." });
                _userSubscribeSrvc.Register(new UserSubscriber() { FirstName = "Artan", LastName = "Zumberi", Username = "artan20", Password = "artan123", CurrentProduct = ProductType.Electronics, Email = "albanian20@live.com", Age = 27, Id = 3, IdSubscriptionList = new List<int> { 4, 5 }, ProfileDescription = "I love shopping." });
                _companySubscribeSrvc.Register(new CompanySubscriber() { CompanyName = "MetiCompany", FirstName = "Muhamed", LastName = "Sakipi", Username = "meti22", Password = "meti123", Age = 27, Email = "proshqipe@gmail.com", CurrentProduct = ProductType.Food, Id = 4, IdSubscriptionList = new List<int> { 1, 2, 3 } });
                _companySubscribeSrvc.Register(new CompanySubscriber() { CompanyName = "MiraxhCorp", FirstName = "Miraxh", LastName = "Sakipi", Username = "miraxh22", Password = "miraxh123", Age = 27, Email = "sakipi.mu@gmail.com", CurrentProduct = ProductType.Electronics, Id = 5, IdSubscriptionList = new List<int> { 1, 2, 3 } });
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
                        _currentlyLoggedIn = _accountSrvc.UserLogIn();
                    }
                    else
                    {
                        _currentlyLoggedIn = _accountSrvc.UserRegister();

                    }
                    if (_currentlyLoggedIn == null) continue;
                    _uiSrvc.WelcomeMenu(_currentlyLoggedIn);
                }

                int mainMenuChoice = _uiSrvc.MainMenu(_currentlyLoggedIn.Role);
                string mainMenuItem = _uiSrvc.MainMenuItems[mainMenuChoice - 1];
                switch (mainMenuItem)
                {
                    case "Your Subscribers":
                        _uiSrvc.CompanySubscriberListMenue((CompanySubscriber)_currentlyLoggedIn);
                        break;
                    case "Send Promotions":
                        _accountSrvc.SendPromotion((CompanySubscriber)_currentlyLoggedIn);
                        break;
                    case "Market Offers":
                        _uiSrvc.SubscribeMenu((UserSubscriber)_currentlyLoggedIn);
                        break;
                    case "Your Subscriptions":
                        _uiSrvc.UnSubscribeMenu((UserSubscriber)_currentlyLoggedIn);
                        break;
                    case "Upgrade to Premium":
                        _accountSrvc.UpgradeToPremium();
                        break;
                    case "Account":
                        _uiSrvc.MyAccountMenu(_currentlyLoggedIn);
                        break;
                    case "Log Out":
                        _currentlyLoggedIn = null;
                        EmailService.ClearCredential();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
