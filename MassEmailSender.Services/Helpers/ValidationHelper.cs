using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Services.Helpers
{
    public static class ValidationHelper
    {

        public static int ValidateNumber(string number, int range)
        {
            int num = 0;
            bool isNumber = int.TryParse(number, out num);
            if (!isNumber) return -1;
            if (num <= 0 || num > range) return -1;
            return num;
        }

        public static string ValidateString(string str)
        {
            str = str.Trim();
            if (str.Length < 2) return null;
            int number;
            bool hasNumber = false;
            foreach (char character in str)
            {
                if (int.TryParse(character.ToString(), out number))
                {
                    hasNumber = true;
                }
            }
            if (hasNumber) return null;
            return str;
        }

        public static string ValidateUsername(string username)
        {
            username = username.Trim();
            if (username.Length < 6) return null;
            return username;
        }

        public static string ValidatePassword(string password)
        {
            password = password.Trim();
            if (password.Length < 6) return null;
            int number;
            bool hasNumber = false;
            foreach (char character in password)
            {
                if (int.TryParse(character.ToString(), out number))
                {
                    hasNumber = true;
                }
            }
            if (!hasNumber) return null;
            return password;
        }
    }
}
