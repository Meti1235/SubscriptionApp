using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MassEmailSender.Services.GmailAPI;
using MassEmailSender.Services.Helpers;
using MassEmailSender.Services.Services.Interfaces;
using System;
using System.IO;
using System.Threading;

namespace MassEmailSender.Services
{
    public class EmailService : IEmailService
    {
        private static string[] Scope = { GmailService.Scope.GmailSend /*, GmailService.Scope.GmailModify */ }; //later add multiple options

        private static string ApplicationName = "Gmail API .NET Quickstart";
        private static UserCredential Credential;
        public static GmailService MyGmailService;
        public static void CreateGmailService()
        {
            MyGmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });
        }

        public static void ClearCredential()
        {
            Credential = null;
        }

        public static void CreateAPICredentials(int id)
        {
            if (Credential == null)
            {
                using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {

                    string credPath = $".credentials/gmail-dotnet-quickstart{id}.json";

                    Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                  GoogleClientSecrets.Load(stream).Secrets,
                  Scope,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true)).Result;
                    CreateGmailService();
                    Console.WriteLine("Your Credentials have been successfully verified");
                }
            }
        }
        public Message WriteEmail()
        {
            Console.Clear();
            Console.WriteLine("Please enter the Subject:");
            string subject = Console.ReadLine();
            Console.WriteLine("Please write your Gmail");
            string emailFrom = ValidationHelper.ValidateEmail(Console.ReadLine());
            Console.WriteLine("Please write the recipient email:");
            string emailTo = ValidationHelper.ValidateEmail(Console.ReadLine());
            Console.WriteLine("Please write your message:");
            string body = Console.ReadLine();

            Message message = Email.CreateMessage(subject, body, emailFrom, emailTo);
            return message;

        }
        public bool SendEmail(Message message)
        {
            var result = MyGmailService.Users.Messages.Send(message, "me").Execute();
            if (result == null)
            {
                return false;
            }
            return true;
        }
        public void SendEmailPromotion() //write email and store email //would you like to send email Yes/No //Yes sends email //No goes back aka opens the saves string again
        {
            Message message = WriteEmail();
            bool wasEmailSent = SendEmail(message);

            Console.Clear();
            if (!wasEmailSent)
            {
                Console.WriteLine("Your email was not sent.");
            }
            Console.WriteLine("Your email was successfully sent.");
            Console.ReadLine();
        }
    }
}
