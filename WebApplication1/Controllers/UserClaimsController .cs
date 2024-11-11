using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Services.Account.Dto;
using System.Security.Claims;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserClaimsController : ControllerBase
    {
        private readonly IUserClaimService _userClaimService;

        public UserClaimsController(IUserClaimService userClaimService)
        {
            _userClaimService = userClaimService;
        }

        [HttpPost("{userId}/claims")]
        public async Task<ActionResult> AddClaim(string userId, [FromBody] UserClaimDto claimDto)
        {
            if (claimDto == null)
            {
                return BadRequest("Claim Result is required.");
            }

            var result = await _userClaimService.AddClaimAsync(userId, claimDto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Claim added successfully." });
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{userId}/claims")]
        public async Task<ActionResult> RemoveClaim(string userId, [FromBody] Claim claim)
        {
            var result = await _userClaimService.RemoveClaimAsync(userId, claim);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Claim removed successfully." });
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{userId}/claims")]
        public async Task<ActionResult> GetClaims(string userId)
        {
            var claims = await _userClaimService.GetClaimsAsync(userId);
            return Ok(claims);
        }
    }
}
