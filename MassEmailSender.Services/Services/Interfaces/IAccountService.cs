using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Services
{
    public interface IAccountService<T> where T : Subscriber
    {
        void EditDescription(T entity);
        void ChangeInfo(int userId, string firstName, string lastName);
        void ChangePassword(int userId, string oldPassword, string newPassword);

    }
}
