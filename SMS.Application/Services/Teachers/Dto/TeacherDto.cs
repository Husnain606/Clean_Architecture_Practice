using SMS.Common.ViewModels;

namespace SMS.Application.Services.Teachers.Dto
{
    public class TeacherDto
    {
        public string Name { get; set; } = string.Empty;
        public string TeacherFatherName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string CNIC { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public DateTime HiringDate { get; set; }
        public string School { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string timespann { get; set; }

        public static implicit operator TeacherDto(ResponseModel v)
        {
            throw new NotImplementedException();
        }
    }
}
