using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services.Helpers;
using System;

namespace MassEmailSender.Services
{
    public class AccountService<T> : IAccountService<T> where T : Subscriber
    {

        private IDb<T> _db;
        public AccountService()
        {
            _db = new FileSystemDb<T>();
        }

        public void EditDiscription(T entity)
        {
            Console.WriteLine("Edit your Discription:");
            string discription = Console.ReadLine();
            entity.ProfileDiscription = $"({discription})";

            _db.Update(entity);
            Console.Clear();
            Console.WriteLine("Discription Updated.");
            Console.ReadLine();
        }

        public void ChangeInfo(int userId, string firstName, string lastName)
        {
            T user = _db.GetById(userId);
            if (ValidationHelper.ValidateString(firstName) == null || ValidationHelper.ValidateString(lastName) == null)
            {
                MessageHelper.PrintMessage("[Error] strings were not valid. Please try again!", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            user.FirstName = firstName;
            user.LastName = lastName;
            _db.Update(user);
            MessageHelper.PrintMessage("Data successfuly changed!", ConsoleColor.Green);
            Console.ReadLine();
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            T user = _db.GetById(userId);

            if (user.Password != oldPassword)
            {
                MessageHelper.PrintMessage("[Error] Old password did not match", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            if (ValidationHelper.ValidateString(newPassword) == null)
            {
                MessageHelper.PrintMessage("[Error] New password is not valid", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            user.Password = newPassword;
            _db.Update(user);
            MessageHelper.PrintMessage("Password successfuly changed!", ConsoleColor.Green);
            Console.ReadLine();
        }

    }
    public class AccountService
    {
        public static IUiService _uiSrvc = new UiService();
        public static EmailService _emailSrvc = new EmailService();
        public static ISubscriberService<UserSubscriber> _userSubscribeSrvc = new SubscriberService<UserSubscriber>();
        public static ISubscriberService<CompanySubscriber> _companySubscribeSrvc = new SubscriberService<CompanySubscriber>();
 
        public Subscriber UserRegister()
        {
            int registerChoice = _uiSrvc.RegisterMenu();
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
     
        public Subscriber UserLogIn()
        {
            int roleChoice = _uiSrvc.RoleMenu();

            SubscriptionType role = (SubscriptionType)roleChoice;

            Console.Clear();
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            switch (role)
            {
                case SubscriptionType.User:
                    UserSubscriber _currentUser = _userSubscribeSrvc.LogIn(username, password);
                    if (_currentUser != null) _currentUser.AddSubscriptionsAgain();
                    return _currentUser;

                case SubscriptionType.Company:
                    CompanySubscriber _currentCompany = _companySubscribeSrvc.LogIn(username, password);
                    if (_currentCompany != null) _currentCompany.AddSubscriptionsAgain();
                    return _currentCompany;
            }
            throw new ApplicationException("This error is not possible");
        }
  
        public void UpgradeToPremium()
        {
            Console.Clear();
            Console.WriteLine("Upgrade to premium to get these features:");
            Console.WriteLine("* Newsletter in your mail");
            Console.ReadLine();
        }

        public void SendPromotion(Subscriber currentCompany) //move this method
        {
            int promotionChoice = _uiSrvc.PromotionMenue();

            var compamy = (CompanySubscriber)currentCompany;

            if (promotionChoice == 1)
            {
                compamy.SendInAppPromotions();
            }
            else
            {
                _emailSrvc.SendEmailPromotion();
            }
        }

    }

}

