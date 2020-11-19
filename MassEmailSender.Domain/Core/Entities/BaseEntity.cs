using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Domain.Core.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public abstract string Info();
        public abstract void ReadPromotion(ProductType product);
    }
}
