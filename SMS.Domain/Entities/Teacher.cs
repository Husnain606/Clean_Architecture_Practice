namespace SMS.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string CNIC { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;

        // Foreign key for ApplicationUser 
        public string UserId { get; set; } = string.Empty; // Foreign key to ApplicationUser 
        public virtual ApplicationUser ApplicationUser { get; set; } = null!; // Navigation property

        public DateTime HiringDate { get; set; } = DateTime.Now;
        public Guid SchoolId { get; set; }
        public Guid BranchId { get; set; }
    }
}