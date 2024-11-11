using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Accounts;
using SMS.Application.Interfaces.Identity;
using SMS.Common.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IAccountService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<RoleController> _logger;

        // User Role Operations

        [HttpPost("users/{userId}/roles/{roleId}")]
        public async Task<ActionResult<ResponseModel>> AssignRole(string userId, string roleId)
        {
            await _userRoleService.AssignRoleAsync(userId, roleId);
            return Ok(new ResponseModel { Message = "Role assigned successfully." });
        }

        [HttpDelete("users/{userId}/roles/{roleId}")]
        public async Task<ActionResult<ResponseModel>> RemoveRole(string userId, string roleId)
        {
            await _userRoleService.RemoveRoleAsync(userId, roleId);
            return Ok(new ResponseModel { Message = "Role removed successfully." });
        }
    }
   
}
