namespace SMS.Application.Services.Teachers.Dto
{
    public class CreateTeacherDto
    {
        public Guid Id { get; set; }
        public string TeacherFirstName { get; set; } = string.Empty;
        public string TeacherLastName { get; set; } = string.Empty;
        public string TeacherFatherName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string CNIC { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Pasword { get; set; } = string.Empty;
        public string ConfirmPasword { get; set; } = string.Empty;
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public Guid SchoolId { get; set; }
        public Guid BranchId { get; set; }
    }
}
