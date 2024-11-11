using Microsoft.AspNetCore.Identity;
using SMS.Application.Services.Account.Dto;
using System.Security.Claims;

namespace SMS.Application.Interfaces.Identity
{
    public interface IUserClaimService
    {
        Task<IdentityResult> AddClaimAsync(string userId, UserClaimDto claim);
        Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(string userId);
    }
}
