using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace MassEmailSender.Services.GmailAPI
{
    public class Email
    {
        public static Message CreateMessage(string subject, string body, string emailFrom, string emailTo)
        {
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(emailFrom);
            mail.IsBodyHtml = false; // turn true Later when we use html
            mail.To.Add(new MailAddress(emailTo));

            //string attImg = @"C:\Users\LaunchD\Desktop\Meti Portfolio Code\SubscriptionApp\ImageImplementationTest.jpg";  //Option to add Images
            //mail.Attachments.Add(new Attachment(attImg));  // also at ASP.Net MVC project create a File Option

            MimeMessage mimeMessage = MimeMessage.CreateFromMailMessage(mail);
            Message message = new Message();
            message.Raw = Base64UrlEncode(mimeMessage.ToString());
            return message;
        }
        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
      
    }
}
