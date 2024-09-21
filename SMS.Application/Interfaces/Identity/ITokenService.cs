using SMS.Common.Responses;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Identity
{
    public interface ITokenService
    {
        Task<Response<AuthenticationResponse>> GenerateUserTokenAsync(ApplicationUser applicationUser);
    }
}
