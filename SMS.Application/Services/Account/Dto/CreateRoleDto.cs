using System.ComponentModel.DataAnnotations;

namespace SMS.Application.Services.Account.Dto
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; } = string.Empty; // Role name
    }
}
