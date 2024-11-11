using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SMS.Application.Interfaces.Identity;
using SMS.Common.Constants;
using SMS.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Models;
using SMS.Common.Responses;
using SMS.Common.ViewModels;

namespace SMS.IdentityService.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRoleService _userRole;

        public TokenService(IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicationUser> userManager,
            IUserRoleService userRole)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _userRole = userRole;
        }

        public async Task<ResponseModel<AuthenticationResponse>> GenerateUserTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Expiry),
                SigningCredentials =
                   new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new ResponseModel<AuthenticationResponse>
            {
                Successful = true,
                Message = IdentityMessageConstants.UserAuthenticatedSuccessfully,
                Result = new AuthenticationResponse
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Token = tokenHandler.WriteToken(token),
                   
                }
            };
        }
    }
}
