﻿using MassEmailSender.Domain.Core.Entities;
using MassEmailSender.Services.Helpers;
using System;
using System.Linq;

namespace MassEmailSender.Services
{
    public class SubscriberService<T> : ISubscriberService<T> where T : Subscriber
    {
        public static IUiService _uiSrvc = new UiService();
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

        public T CreateEntity(T entity)
        {
            Console.Clear();
            Console.WriteLine("Enter first name:");
            entity.FirstName = Console.ReadLine();

            Console.WriteLine("Enter last name:");
            entity.LastName = Console.ReadLine();

            if (entity.GetType().ToString().Contains("CompanySubscriber"))
            {
                Console.WriteLine("Enter company name");
                CompanySubscriber companySubscriber = (CompanySubscriber)(Subscriber)entity;
                companySubscriber.CompanyName = Console.ReadLine();
            }

            Console.WriteLine("Enter age:");
            entity.Age = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter email:");
            entity.Email = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter username:");
            entity.Username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            entity.Password = Console.ReadLine();

            Console.WriteLine("Enter a profile description:");
            entity.ProfileDescription = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter your favorite product:");
            string[] productChoice = Enum.GetNames(typeof(ProductType));
            for (int i = 0; i < productChoice.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {productChoice[i]}");
            }
            entity.CurrentProduct = (ProductType)int.Parse(Console.ReadLine()) - 1;

            return entity;
        }

        public T Register(T entity)  //add more validations
        {
            if (ValidationHelper.ValidateString(entity.FirstName) == null
            || ValidationHelper.ValidateString(entity.LastName) == null
            || ValidationHelper.ValidateUsername(entity.Username) == null
            || ValidationHelper.ValidatePassword(entity.Password) == null
            || ValidationHelper.ValidateEmail(entity.Email) == null)
            {
                MessageHelper.PrintMessage("[Error] Invalid information!", ConsoleColor.Red);
                return null;
            }

            int id = _db.Insert(entity);
            if (entity.GetType().ToString().Contains("CompanySubscriber"))
            {
                EmailService.CreateAPICredentials(id);
            }
            return _db.GetById(id);
        }

        public T LogIn(string username, string password)
        {

            T userFound = _db.GetAll().SingleOrDefault(x => x.Username == username && x.Password == password);
            if (userFound == null)
            {
                MessageHelper.PrintMessage("[Error] Username or Password did not match! Please try again!", ConsoleColor.Red);
                return null;
            }
            if (userFound.GetType().ToString().Contains("CompanySubscriber"))
            {
                EmailService.CreateAPICredentials(userFound.Id);
            }

            return userFound;
        }

        public bool IsDbEmpty()
        {
            return _db.GetAll() == null || _db.GetAll().Count == 0;
        }

    }
}
