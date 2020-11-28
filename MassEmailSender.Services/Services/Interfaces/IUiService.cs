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
        void MyAccountMenue(Subscriber currentUser);
        void SubscribeMenue(Subscriber currentUser);
        int PromotionMenue();

    }
}
