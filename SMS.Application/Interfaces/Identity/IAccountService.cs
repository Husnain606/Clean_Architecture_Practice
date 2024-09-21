using SMS.Application.Services.Account.Dto;
using SMS.Common.Responses;

namespace SMS.Application.Interfaces.Accounts
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> CreateUserAsync(CreateUserDto model);

        Task<Response<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenDto refreshToken);

        Task<Response<AuthenticationResponse>> LoginUserAsync(LoginDto model);

        Task<Response> ChangePasswordAsync(ChangePasswordDto model);
    }
}
