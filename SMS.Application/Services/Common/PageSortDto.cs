namespace SMS.Application.Services.Common
{
    public class PageSortDto : PageDto
    {
        public string? SortBy { get; set; }
        public bool SortOrder { get; set; } = true;
    }
}
