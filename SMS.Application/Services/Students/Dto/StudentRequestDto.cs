using SMS.Application.Services.Common;

namespace SMS.Application.Services.Students.Dto
{
    public class StudentRequestDto : PageSortDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? StudentFatherName { get; set; } = string.Empty;
        public string? Class { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string? Mail { get; set; } = string.Empty;
        public string? Contact { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? timespann { get; set; } = null;
    }
}