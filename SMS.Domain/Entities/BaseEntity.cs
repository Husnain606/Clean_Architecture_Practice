namespace SMS.Domain.Entities
{
    public class BaseEntity : BaseAuditEntity
    {
        public bool IsDeleted { get; set; } = false;
    }
}
