using Microsoft.AspNetCore.Identity;
using SMS.Application.Interfaces.Identity;
using SMS.Domain.Entities;

namespace SMS.Application.Services.Account
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Assign a role to a user
        public async Task AssignRoleAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null)
            {
                throw new Exception("User  not found.");
            }

            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        
        // Remove a role from a user
        public async Task RemoveRoleAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null)
            {
                throw new Exception("User  not found.");
            }

            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
       
    }
}
