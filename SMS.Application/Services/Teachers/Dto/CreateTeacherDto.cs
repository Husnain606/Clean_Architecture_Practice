namespace SMS.Application.Services.Teachers.Dto
{
    public class CreateTeacherDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string CNIC { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Pasword { get; set; } = string.Empty;
        public string ConfirmPasword { get; set; } = string.Empty;
        public Guid SchoolId { get; set; }
        public Guid BranchId { get; set; }
    }
}
