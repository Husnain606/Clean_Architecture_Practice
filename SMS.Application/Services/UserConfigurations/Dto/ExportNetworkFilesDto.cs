namespace SMS.Application.Services.UserConfigurations.Dto
{
    public class ExportNetworkFilesDto
    {
        public string Name { get; set; } = default!;
        public MemoryStream MemoryStream { get; set; } = default!;
    }
}
