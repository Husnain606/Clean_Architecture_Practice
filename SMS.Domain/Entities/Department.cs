using System.ComponentModel.DataAnnotations;

namespace SMS.Domain.Entities
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmenrDescription { get; set; }

        public virtual ICollection<Student> Student { get; set; } = null!;
    }
}
