using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Accounts;
using SMS.Application.Services.Account.Dto;
using SMS.Domain.Entities;
using SMS.Common.Constants;
using SMS.Common.Responses;
using SMS.Application.Interfaces.Identity;
using FluentValidation;
using AutoMapper;


namespace SMS.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserConfigurationService _userConfigurationService;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        //private readonly IHeaderService _headerService;

        public AccountService(IMapper mapper,IIdentityService identityService, ITokenService tokenService, ILogger<AccountService> logger, ICurrentUserService currentUserService, IUserConfigurationService userConfigurationService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _logger = logger;
            _currentUserService = currentUserService;
             //_headerService = headerService;
            _userConfigurationService = userConfigurationService;
            _mapper= mapper;
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordDto model)
        {
            string email = model.Email;
            var user = await _identityService.FindByEmailAsync(email);      //(_currentUserService.Email.ToString());
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

        public async Task<Response<AuthenticationResponse>> CreateUserAsync(CreateUserDto model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
                          
            var result = await _identityService.CreateUserAsync(user, model.Password);
            await _identityService.AddToRoleAsync(user, new List<string> { "ADMINISTRATION" });


            if (!result.Succeeded)
            {
                _logger.LogError(IdentityMessageConstants.UserCreationFailed);
                throw new ValidationException(IdentityMessageConstants.UserCreationFailed);
            }
            // await _userConfigurationService.SaveUserConfiguration(Guid.Parse(user.Id));
            return await _tokenService.GenerateUserTokenAsync(user);
        }

        public async Task<Response<AuthenticationResponse>> LoginUserAsync(LoginDto model)
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

        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            var user = await _identityService.FindByEmailAsync(refreshToken.Email);
            if (user == null)
            {
                _logger.LogError(IdentityMessageConstants.InvalidUserNamePassword);
                throw new ValidationException(IdentityMessageConstants.InvalidUserNamePassword);
            }

            return await _tokenService.GenerateUserTokenAsync(user);
        }
    }
}
//public class AccountServices : IAccountService
//{
//    private readonly IConfiguration _configuration;
//    private readonly ILogger<AccountServices> _logger;

//    public AccountServices(IConfiguration configuration, ILogger<AccountServices> logger)
//    {
//        _configuration = configuration;
//        _logger = logger;
//    }
//    public async Task<LoginResponseDto> LoginUser(LoginDto model)
//    {
//        try
//        {
//            if (model == null)
//            {
//                throw new ArgumentException("Invalid model", nameof(model));
//            }
//            LoginResponseDto response = new() { Username = model.Username };
//            if (model.Username == "Husnain" && model.Password == "nani@606")
//            {
//                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecret"));
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var tokenDescription = new SecurityTokenDescriptor()
//                {
//                    Subject = new ClaimsIdentity(new Claim[]
//                    { 
//                //Username
//                new Claim(ClaimTypes.Name, model.Username),
//                //Role
//                new Claim(ClaimTypes.Role,"Teacher")
//                    }),
//                    Expires = DateTime.Now.AddHours(5),
//                    NotBefore = DateTime.Now.AddSeconds(-1),
//                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
//                };
//                var token = tokenHandler.CreateToken(tokenDescription);
//                response.Token = tokenHandler.WriteToken(token);
//            }
//            else
//            {
//                throw new ArgumentException("Invalid model", nameof(model));
//            }
//            return  response ;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error occurred while creating student.");
//            throw ex;
//        }
//    }
//}

