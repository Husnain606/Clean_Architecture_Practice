using Microsoft.AspNetCore.Identity;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser applicationUser, string password);

        Task<ApplicationUser> FindByEmailAsync(string email);

        Task<ApplicationUser> FindByIdAsync(string id);

        Task<ApplicationUser> FindByUserNameAsync(string userName);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, List<string> roles);

        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        Task<List<ApplicationUser>> GetAllUsersAsync();
    }
}
