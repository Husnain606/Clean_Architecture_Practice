using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SMS.Domain.Common;

namespace SMS.Domain.Entities
{
    public class ConfigurationSchema :  ICreationAudited, IModificationAudited, ISoftDelete
    {
        public Guid Id { get; set; }
        public int EntityTypeId { get; set; }
        public string SchemaName { get; set; } = null!;
        public string DataType { get; set; } = null!;
        public bool IsOptional { get; set; }
        public bool ReadOnly { get; set; }
        public virtual EntityType EntityType { get; set; } = null!;
        public virtual ICollection<UserConfiguration> UserConfigurations { get; set; } = null!;
        public string? CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? ModifiedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? ModifiedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
