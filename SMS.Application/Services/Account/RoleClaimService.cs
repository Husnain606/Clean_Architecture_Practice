using Microsoft.AspNetCore.Identity;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Services.Account.Dto;

namespace SMS.Application.Services.Account
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleClaimService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRoleClaim<string>> AddClaimAsync(string roleId, RoleClaimDto claimDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var claim = new IdentityRoleClaim<string>
            {
                RoleId = roleId,
                ClaimType = claimDto.ClaimType,
                ClaimValue = claimDto.ClaimValue
            };

            var result = await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(claim.ClaimType, claim.ClaimValue));
            if (result.Succeeded)
            {
                return claim;
            }

            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IEnumerable<IdentityRoleClaim<string>>> GetClaimsAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            return (IEnumerable<IdentityRoleClaim<string>>)await _roleManager.GetClaimsAsync(role);
        }

        public async Task RemoveClaimAsync(string roleId, RoleClaimDto claimDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var claim = new System.Security.Claims.Claim(claimDto.ClaimType, claimDto.ClaimValue);
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task UpdateClaimAsync(string roleId, RoleClaimDto oldClaimDto, RoleClaimDto newClaimDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var oldClaim = new System.Security.Claims.Claim(oldClaimDto.ClaimType, oldClaimDto.ClaimValue);
            var newClaim = new System.Security.Claims.Claim(newClaimDto.ClaimType, newClaimDto.ClaimValue);

            var result = await _roleManager.RemoveClaimAsync(role, oldClaim);
            if (result.Succeeded)
            {
                await _roleManager.AddClaimAsync(role, newClaim);
            }
            else
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}

