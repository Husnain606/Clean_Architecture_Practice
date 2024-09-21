namespace SMS.Application.Services.UserConfigurations.Dto
{
    public class UserConfigurationDto
    {
        public Guid Id { get; set; }
        public string SchemaName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string DataType { get; set; } = null!;
        public bool IsShown { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSortable { get; set; }
        public bool ReadOnly { get; set; }
        public int DisplayOrder { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityTypeName { get; set; } = null!;
    }
}
