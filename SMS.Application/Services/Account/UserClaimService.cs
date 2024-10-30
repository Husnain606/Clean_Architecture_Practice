using Microsoft.AspNetCore.Identity;
using SMS.Application.Interfaces.Identity;
using SMS.Domain.Entities;
using System.Security.Claims;

namespace SMS.Application.Services.Account
{
    public class UserClaimService : IUserClaimService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserClaimService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddClaimAsync(string userId, Claim claim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User  not found.");
            }
            return await _userManager.AddClaimAsync(user, claim);
        }

        public async Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User  not found.");
            }
            return await _userManager.RemoveClaimAsync(user, claim);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User  not found.");
            }
            return await _userManager.GetClaimsAsync(user);
        }
    }
}
