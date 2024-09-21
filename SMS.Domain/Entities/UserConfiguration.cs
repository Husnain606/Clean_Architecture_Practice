using SMS.Domain.Common;

namespace SMS.Domain.Entities
{
    public class UserConfiguration : BaseEntity, ICreationAudited, IModificationAudited, ISoftDelete
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public bool IsShown { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSortable { get; set; }
        public int DisplayOrder { get; set; }
        public Guid UserId { get; set; }
        public Guid ConfigurationSchemaId { get; set; }
        public virtual ConfigurationSchema ConfigurationSchema { get; set; } = null!;
    }
}
