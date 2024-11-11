using SMS.Application.Services.Account.Dto;
using SMS.Common.Responses;
using SMS.Common.ViewModels;

namespace SMS.Application.Interfaces.Accounts
{
    public interface IAccountService
    {
        Task<ResponseModel<AuthenticationResponse>> CreateUserAsync(CreateUserDto model);

        Task<ResponseModel<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenDto refreshToken);

        Task<ResponseModel<AuthenticationResponse>> LoginUserAsync(LoginDto model);

        Task<ResponseModel> ChangePasswordAsync(ChangePasswordDto model);
    }
}
