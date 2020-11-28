using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Services.Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(Message message);
        Message WriteEmail();
        void SendEmailPromotion();

    }
}
