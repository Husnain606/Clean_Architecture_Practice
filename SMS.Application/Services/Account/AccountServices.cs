using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Accounts;
using SMS.Application.Services.Account.Dto;
using SMS.Domain.Entities;
using SMS.Common.Constants;
using SMS.Common.Responses;
using SMS.Application.Interfaces.Identity;
using FluentValidation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace SMS.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public AccountService(IMapper mapper, IIdentityService identityService, ITokenService tokenService, ILogger<AccountService> logger, ICurrentUserService currentUserService,IApplicationDbContext dbContext)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _logger = logger;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordDto model)
        {
            try
            {
               
                string email = model.Email;
                var user = await _identityService.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogError(IdentityMessageConstants.UserNotFound);
                    throw new ValidationException(IdentityMessageConstants.UserNotFound);
                }

                var passwordCheck = await _identityService.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheck)
                {
                    _logger.LogError(IdentityMessageConstants.InvalidPassword);
                    throw new ValidationException(IdentityMessageConstants.InvalidPassword);
                }

                var result = await _identityService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    _logger.LogError(IdentityMessageConstants.UnableToChangePassword);
                    throw new ValidationException(IdentityMessageConstants.UnableToChangePassword);
                }

                return new Response
                {
                    Successful = true,
                    Message = IdentityMessageConstants.PasswordChangedSuccessfully
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the password.");
                throw; // Optionally rethrow or handle the error
            }
        }

        public async Task<Response<AuthenticationResponse>> CreateUserAsync(CreateUserDto model)
        {
            try
            {
                 using var transaction = await _dbContext.Database.BeginTransactionAsync();     
                var user = _mapper.Map<ApplicationUser>(model);
                var result = await _identityService.CreateUserAsync(user, model.Password);
                await _identityService.AddToRoleAsync(user, new List<string> { "ADMINISTRATION", "USER" });

                if (!result.Succeeded)
                {
                    _logger.LogError(IdentityMessageConstants.UserCreationFailed);
                    throw new ValidationException(IdentityMessageConstants.UserCreationFailed);
                }
                var tokenResponse = await _tokenService.GenerateUserTokenAsync(user);
                await transaction.CommitAsync(); // Commit the transaction
                return tokenResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user.");
                throw; // Optionally rethrow or handle the error
            }
        }

        public async Task<Response<AuthenticationResponse>> LoginUserAsync(LoginDto model)
        {
            try
            {
                var user = await _identityService.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogError(IdentityMessageConstants.InvalidUserNamePassword);
                    throw new ValidationException(IdentityMessageConstants.InvalidUserNamePassword);
                }

                var passwordCheck = await _identityService.CheckPasswordAsync(user, model.Password);
                if (!passwordCheck)
                    throw new ValidationException(IdentityMessageConstants.InvalidUserNamePassword);

                return await _tokenService.GenerateUserTokenAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                throw; // Optionally rethrow or handle the error
            }
        }

        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            try
            {
                var user = await _identityService.FindByEmailAsync(refreshToken.Email);
                if (user == null)
                {
                    _logger.LogError(IdentityMessageConstants.InvalidUserNamePassword);
                    throw new ValidationException(IdentityMessageConstants.InvalidUserNamePassword);
                }

                return await _tokenService.GenerateUserTokenAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing the token.");
                throw; // Optionally rethrow or handle the error
            }
        }
    }
}
