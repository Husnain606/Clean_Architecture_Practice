using Microsoft.AspNetCore.Identity;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Services.Account.Dto;

namespace SMS.Application.Services.Account
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRole> CreateRoleAsync(CreateRoleDto roleDto)
        {
            var role = new IdentityRole { Name = roleDto.Name };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return role;
            }

            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task UpdateRoleAsync(string roleId, UpdateRoleDto roleDto)
        {
            var role = await GetRoleByIdAsync(roleId);
            if (role != null)
            {
                role.Name = roleDto.Name;
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await GetRoleByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }
    }
}
