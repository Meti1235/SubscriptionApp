using MassEmailSender.Domain.Core.Entities;
using System.Collections.Generic;

namespace MassEmailSender.Services
{
    public interface IUiService
    {
        int RoleMenu();
        int LogInMenu();
        int RegisterMenu();
        int ChooseMenu<T>(List<T> items);
        List<string> MainMenuItems { get; set; }
        List<string> AccountMenuItems { get; set; }
        void WelcomeMenu(Subscriber user);
        int MainMenu(SubscriptionType role);
        int AccountMenu(SubscriptionType role);
        int PromotionMenu();
        void MyAccountMenu(Subscriber currentUser);
        void SubscribeMenu(UserSubscriber currentUser);
        void UnSubscribeMenu(UserSubscriber currentUser);
        void CompanySubscriberListMenue(CompanySubscriber currentCompany);


    }
}
