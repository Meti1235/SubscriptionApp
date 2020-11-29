using MassEmailSender.Domain.Core.Entities;
using System;
using System.Collections.Generic;

namespace MassEmailSender.Services.Helpers
{
    public static class MessageHelper
    {
        public static void PrintMessage(string errorMsg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(errorMsg);
            Console.ResetColor();
            Console.ReadLine();
        }

        public static void PrintCompanyList(List<CompanySubscriber> companyList)
        {
            for (int i = 0; i < companyList.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {companyList[i].CompanyName} {companyList[i].ShowProfileDescription()}");
                Console.WriteLine(); //space
            }
        }
    }
}
