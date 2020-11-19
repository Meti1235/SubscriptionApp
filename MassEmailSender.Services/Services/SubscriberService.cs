using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services.Helpers;
using System;
using System.Linq;

namespace MassEmailSender.Services
{
    public class SubscriberService<T> : ISubscriberService<T> where T : Subscriber
    {
        private IDb<T> _db;
        public SubscriberService()
        {
            _db = new FileSystemDb<T>();
        }

        public T GetUserById(int id)
        {
            // Security question, Check token, Check authentication, Check if it's logged in
            return _db.GetById(id);
        }

        public IDb<T> GetAll()
        {
            return _db;
        }

        public T LogIn(string username, string password)
        {

            T userFound = _db.GetAll().SingleOrDefault(x => x.Username == username && x.Password == password);
            if (userFound == null)
            {
                MessageHelper.PrintMessage("[Error] Username or Password did not match! Please try again!", ConsoleColor.Red);
                return null;
            }
            return userFound;
        }

        public T Register(T entity)
        {

            if (ValidationHelper.ValidateString(entity.FirstName) == null
                || ValidationHelper.ValidateString(entity.LastName) == null
                || ValidationHelper.ValidateUsername(entity.Username) == null
                || ValidationHelper.ValidatePassword(entity.Password) == null)
            {
                MessageHelper.PrintMessage("[Error] Invalid information!", ConsoleColor.Red);
                return null;
            }
            int id = _db.Insert(entity);
            return _db.GetById(id);
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

        public bool IsDbEmpty()
        {
            return _db.GetAll() == null || _db.GetAll().Count == 0;
        }
    }
}
