using Microsoft.AspNetCore.Identity;
using SMS.Application.Services.Account.Dto;
namespace SMS.Application.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<IdentityRole> CreateRoleAsync(CreateRoleDto roleDto);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task UpdateRoleAsync(string roleId, UpdateRoleDto roleDto);
        Task DeleteRoleAsync(string roleId);
    }
}
