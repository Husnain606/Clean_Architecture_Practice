using Microsoft.AspNetCore.Identity;
using SMS.Application.Services.Account.Dto;

namespace SMS.Application.Interfaces.Identity
{
    public interface IRoleClaimService
    {
        Task<IdentityRoleClaim<string>> AddClaimAsync(string roleId, RoleClaimDto claimDto);
        Task<IEnumerable<IdentityRoleClaim<string>>> GetClaimsAsync(string roleId);
        Task RemoveClaimAsync(string roleId, RoleClaimDto claimDto);
        Task UpdateClaimAsync(string roleId, RoleClaimDto oldClaimDto, RoleClaimDto newClaimDto);
    }
}