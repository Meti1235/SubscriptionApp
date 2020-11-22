using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
		void Welcome(Subscriber user);
		int ChooseEntiiesMenu<T>(List<T> entities) where T : IBaseEntity;
		int MainMenu(SubscriptionType role);
		int AccountMenu(SubscriptionType role);
		int SubscribeMenue(Subscriber currentUser);
		void UpgradeToPremium();
		Subscriber UserLogIn();
		Subscriber UserRegister();

	}
}
