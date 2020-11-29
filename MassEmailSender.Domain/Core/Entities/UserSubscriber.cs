using System;
using System.Collections.Generic;

namespace MassEmailSender.Domain.Core.Entities
{
    public class UserSubscriber : Subscriber
    {
        public UserSubscriber()
        {
            Role = SubscriptionType.User;
            IdSubscriptionList = new List<int>();
            ProfileDescription = null;
        }
      
        public override string ShowProfileDescription() 
        {
            if (ProfileDescription != null)
            {
                return ProfileDescription; 
            }
            else
            {
               return $"(No description)";
            }
        }
        public void ReadPromotion(ProductType product)
        {
            Console.WriteLine($"Mr/Mrs: {FirstName}, The product {product} is on sale!");
            if (product == CurrentProduct) Console.WriteLine($"Special Promotion with Coupon: ILove{CurrentProduct}");
        }
    }
}
