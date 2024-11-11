using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Services.Account.Dto;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")] // Adjust roles as necessary
    [AllowAnonymous]
    public class RoleClaimController : ControllerBase
    {
        private readonly IRoleClaimService _roleClaimService;

        public RoleClaimController(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }

        // POST: api/RoleClaim
        [HttpPost]
        public async Task<IActionResult> AddClaim([FromBody] RoleClaimDto claimDto, string roleId)
        {
            if (claimDto == null || string.IsNullOrEmpty(roleId))
            {
                return BadRequest("Invalid input.");
            }

            var claim = await _roleClaimService.AddClaimAsync(roleId, claimDto);
            return CreatedAtAction(nameof(GetClaims), new { roleId = roleId }, claim);
        }

        // GET: api/RoleClaim/{roleId}
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetClaims(string roleId)
        {
            var claims = await _roleClaimService.GetClaimsAsync(roleId);
            return Ok(claims);
        }

        // PUT: api/RoleClaim/{roleId}
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateClaim(string roleId, [FromBody] UpdateRoleClaimDto updateDto)
        {
            if (updateDto == null || string.IsNullOrEmpty(roleId))
            {
                return BadRequest("Invalid input.");
            }

            await _roleClaimService.UpdateClaimAsync(roleId, updateDto.OldClaim, updateDto.NewClaim);
            return NoContent();
        }

        // DELETE: api/RoleClaim/{roleId}
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> RemoveClaim(string roleId, [FromBody] RoleClaimDto claimDto)
        {
            if (claimDto == null || string.IsNullOrEmpty(roleId))
            {
                return BadRequest("Invalid input.");
            }

            await _roleClaimService.RemoveClaimAsync(roleId, claimDto);
            return NoContent();
        }
    }
}
