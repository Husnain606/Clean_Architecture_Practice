namespace SMS.Application.Services.Students.Dto
{
    public class CreateStudentDto
    {
        public Guid Id { get; set; }
        public string StudentFirstName { get; set; } = string.Empty;

        public string StudentLastName { get; set; } = string.Empty;

        public string StudentFatherName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Class { get; set; } = string.Empty;

        public string Contact { get; set; }

        public string Mail { get; set; } = string.Empty;

        public string Pasword { get; set; } = string.Empty;

        public string ConfirmPasword { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Foreign key for Department
        public Guid DepartmentId { get; set; }

    }
}