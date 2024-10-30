using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SMS.Application.Interfaces.Identity
{
    public interface IUserClaimService
    {
        Task<IdentityResult> AddClaimAsync(string userId, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(string userId);
    }
}
