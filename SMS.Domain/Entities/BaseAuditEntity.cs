using System.ComponentModel.DataAnnotations;

namespace SMS.Domain.Entities
{
    public class BaseAuditEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [StringLength(450)]
        public string? ModifiedBy { get; set; }
    }
}
