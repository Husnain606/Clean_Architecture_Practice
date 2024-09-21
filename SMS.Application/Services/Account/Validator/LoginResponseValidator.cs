using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using SMS.Application.Services.Account.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace SMS.Application.Services.Account.Validator
{
    public class LoginResponseValidator : AbstractValidator<LoginResponseDto>
    {
        public LoginResponseValidator()
        {
            RuleFor(s => s.Username)
                .NotEmpty().WithMessage("Username is required");
            RuleFor(s => s.Token)
                .NotEmpty()
                .Must(BeAValidJwtToken).WithMessage("Token must be a valid JWT token");
        }

        private bool BeAValidJwtToken(string token)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                jwtHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false
                }, out SecurityToken validatedToken);
                return validatedToken != null;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
    }
}
