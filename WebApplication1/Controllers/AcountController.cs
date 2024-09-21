using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SMS.Application.Services.Account.Dto;
using SMS.Application.Interfaces.Accounts;
using SMS.Common.Responses;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [EnableCors("CorsPolicy")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController>? _logger;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, ILogger<AccountController>? logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost(nameof(CreateUser))]
        public async Task<ActionResult<Response<AuthenticationResponse>>> CreateUser([FromBody] CreateUserDto model)
        {
            var result = await _accountService.CreateUserAsync(model);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<ActionResult<Response<AuthenticationResponse>>> Login([FromBody] LoginDto model)
        {
            var result = await _accountService.LoginUserAsync(model);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost(nameof(RefreshToken))]
        public async Task<ActionResult<Response<AuthenticationResponse>>> RefreshToken([FromBody] RefreshTokenDto refreshToken)
        {
            var result = await _accountService.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }

        [HttpPost(nameof(ChangePassword))]
        public async Task<ActionResult<Response>> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _accountService.ChangePasswordAsync(model);
            return Ok(result);
        }
    }
}
