namespace SMS.Application.Services.UserConfigurations.Dto
{
    public class UpdateUserConfigurationDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public bool IsShown { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSortable { get; set; }
        public int DisplayOrder { get; set; }
    }
}
