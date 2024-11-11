using SMS.Common.Responses;
using SMS.Common.ViewModels;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces.Identity
{
    public interface ITokenService
    {
        Task<ResponseModel<AuthenticationResponse>> GenerateUserTokenAsync(ApplicationUser applicationUser);
    }
}
