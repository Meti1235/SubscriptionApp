using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Domain.Core.Entities
{
    public class UserSubscriber : Subscriber
    {
        public ProductType FavoriteType { get; set; }
        public List<int> IdCompanies { get; set; }
        public UserSubscriber()
        {
            Role = SubscriptionType.User;
            IdCompanies = new List<int>();
        }
        public override string Info()
        {
            return $"{FirstName} {LastName} is a subscriber!";
        }
        public override void ReadPromotion(ProductType product)
        {
            Console.WriteLine($"Mr/Mrs: {FirstName}, The product {product} is on sale!"); 
            if (product == FavoriteType) Console.WriteLine($"Special Promotion with Coupon: ILove{FavoriteType}");
        }
    }
}
