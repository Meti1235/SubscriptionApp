using Google.Apis.Gmail.v1.Data;
using MassEmailSender.Domain.Core.Entities;

namespace MassEmailSender.Services
{
    public interface ISubscriberService<T> where T : Subscriber
    {
        T LogIn(string username, string password);
        T Register(T user);
        T GetUserById(int id);
        IDb<T> GetAll();
        T CreateEntity(T entity);
        bool IsDbEmpty();
    }
}
