namespace SMS.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Class { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Foreign key for ApplicationUser 
        public string UserId { get; set; } = string.Empty; // Foreign key to ApplicationUser 
        public virtual ApplicationUser ApplicationUser { get; set; } = null!; // Navigation property

        // Foreign key for Department
        public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
    }
}
