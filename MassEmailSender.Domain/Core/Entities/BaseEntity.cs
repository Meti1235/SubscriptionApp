namespace MassEmailSender.Domain.Core.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public abstract string Info();
    }
}
