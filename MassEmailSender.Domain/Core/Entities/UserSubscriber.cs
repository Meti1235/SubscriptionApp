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
            ProfileDiscription = null;
        }
      
        public override string ShowProfileDiscription() //future implementation
        {
            if (ProfileDiscription != null)
            {
                return ProfileDiscription; 
            }
            else
            {
               return $"(Please edit your discription)";
            }
        }
        public void ReadPromotion(ProductType product)
        {
            Console.WriteLine($"Mr/Mrs: {FirstName}, The product {product} is on sale!");
            if (product == CurrentProduct) Console.WriteLine($"Special Promotion with Coupon: ILove{CurrentProduct}");
        }
    }
}
