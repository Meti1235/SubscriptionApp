using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Services
{
	public interface ISubscriberService<T> where T : Subscriber
	{
		T LogIn(string username, string password);
		T Register(T user);
		T GetUserById(int id);
		IDb<T> GetAll();

		void ChangePassword(int userId, string oldPassword, string newPassword);
		void ChangeInfo(int userId, string firstName, string lastName);
		bool IsDbEmpty();
	}
}
