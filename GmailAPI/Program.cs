using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace GmailQuickstart
{
    class Program
    {

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json

        static string[] Scope = { GmailService.Scope.GmailSend };
        //static string[] Scope = { GmailService.Scope.GmailModify };

        static string ApplicationName = "Gmail API .NET Quickstart";

        static void Main(string[] args)
        {
          
            //Create Message
            MailMessage mail = new MailMessage();
            mail.Subject = "Message Test 01!";
            mail.Body = "This is <b><i>Nana pe qon ni mesazh djalit tvet mat mirit!</i></b> of message";
            mail.From = new MailAddress("proshqipe@gmail.com");
            mail.IsBodyHtml = true;
            string attImg = @"C:\Users\LaunchD\Desktop\Meti Portfolio Code\SubscriptionApp\meti.jpg";
            mail.Attachments.Add(new Attachment(attImg));
            mail.To.Add(new MailAddress("sakipi.m@outlook.com"));
            MimeKit.MimeMessage mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mail);

            Message message = new Message();
            message.Raw = Base64UrlEncode(mimeMessage.ToString());

            //Gmail API credentials
            UserCredential credential;
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scope,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var result = service.Users.Messages.Send(message, "me").Execute();
            Console.WriteLine("-------------------");
            Console.WriteLine(result.Snippet);
            Console.WriteLine(result.Payload);
            Console.WriteLine(result.Raw);
            Console.WriteLine(result.SizeEstimate);
            Console.ReadLine();
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