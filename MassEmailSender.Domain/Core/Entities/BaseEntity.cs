
namespace MassEmailSender.Domain.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public abstract string ShowProfileDescription();
    }
}
