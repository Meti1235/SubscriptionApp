﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Domain.Core.Entities
{
    public abstract class Subscriber : BaseEntity, ISubscriber
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public SubscriptionType Role { get; set; }
        public ProductType CurrentProduct { get; set; }
    }
}
