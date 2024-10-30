using Microsoft.AspNetCore.Identity;

namespace SMS.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public virtual Student? Student { get; set; } // Optional: if you want to access Student from ApplicationUser 

        public virtual Teacher? Teacher { get; set; } // Optional: if you want to access Teacher from ApplicationUser 
    }
}