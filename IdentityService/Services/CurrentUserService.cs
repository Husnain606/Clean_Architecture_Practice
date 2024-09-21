using Microsoft.AspNetCore.Http;
using SMS.Application.Interfaces.Identity;
using System.Security.Claims;

namespace SMS.IdentityService.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string UserIdentifier => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name)!;

        public string UserId => string.IsNullOrWhiteSpace(UserIdentifier) ? string.Empty : (UserIdentifier);
    }
}
