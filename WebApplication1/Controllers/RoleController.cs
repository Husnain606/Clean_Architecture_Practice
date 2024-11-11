using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Accounts;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Services.Account.Dto;
using SMS.Common.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IAccountService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IAccountService  userService, IRoleService roleService, IUserRoleService userRoleService, ILogger<RoleController> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService;
            _logger = logger;
        }
        // Role CRUD Operations

        [HttpPost("roles")]
        public async Task<ActionResult<ResponseModel<IdentityRole>>> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            var result = await _roleService.CreateRoleAsync(roleDto);
            return Ok(new ResponseModel<IdentityRole> { Result = result });
        }

        [HttpGet("roles/{roleId}")]
        public async Task<ActionResult<ResponseModel<IdentityRole>>> GetRoleById(string roleId)
        {
            var result = await _roleService.GetRoleByIdAsync(roleId);
            return Ok(new ResponseModel<IdentityRole> { Result = result });
        }

        [HttpGet("roles")]
        public async Task<ActionResult<ResponseModel<IEnumerable<IdentityRole>>>> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return Ok(new ResponseModel<IEnumerable<IdentityRole>> { Result = result });
        }

        [HttpPut("roles/{roleId}")]
        public async Task<ActionResult<ResponseModel>> UpdateRole(string roleId, [FromBody] UpdateRoleDto roleDto)
        {
            await _roleService.UpdateRoleAsync(roleId, roleDto);
            return Ok(new ResponseModel { Message = "Role updated successfully." });
        }

        [HttpDelete("roles/{roleId}")]
        public async Task<ActionResult<ResponseModel>> DeleteRole(string roleId)
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Ok(new ResponseModel { Message = "Role deleted successfully." });
        }

       
    }
}

