using System.ComponentModel.DataAnnotations;

namespace SMS.Application.Services.Account.Dto
{
    public class UpdateRoleDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; } = string.Empty; // New role name

        public string ConcurrencyStamp { get; set; } = string.Empty;
    }
}
