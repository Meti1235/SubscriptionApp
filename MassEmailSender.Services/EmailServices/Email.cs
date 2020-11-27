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
    public static class Email
    {

        static string[] Scope = { GmailService.Scope.GmailSend };
        //static string[] Scope = { GmailService.Scope.GmailModify }; //later add multiple options

        static string ApplicationName = "Gmail API .NET Quickstart";
        static UserCredential Credential;


        public static Message CreateMessage(string subject, string body, string emailFrom, string emailTo)
        {
            //Create Message
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(emailFrom);
            mail.IsBodyHtml = false; // turn true Later when we use html
            //string attImg = @"C:\Users\LaunchD\Desktop\Meti Portfolio Code\SubscriptionApp\ImageImplementationTest.jpg";  //Option to add Images
            //mail.Attachments.Add(new Attachment(attImg));  // at ASP.Net MVC project create a File Option
            mail.To.Add(new MailAddress(emailTo));

            MimeMessage mimeMessage = MimeMessage.CreateFromMailMessage(mail);
            Message message = new Message();
            message.Raw = Base64UrlEncode(mimeMessage.ToString());
            return message;
        }
        public static bool SendEmail(Message message)
        {

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });

            var result = service.Users.Messages.Send(message, "me").Execute();
            if (result == null)
            {
                return false;
            }
            return true;
        }
        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
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
                    Console.WriteLine("Credential file saved to: " + credPath);
                }
            }
        }
    }
}
