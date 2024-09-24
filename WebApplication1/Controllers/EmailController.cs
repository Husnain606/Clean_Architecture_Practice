using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Email;
using SMS.Common.ViewModels;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // 1. Send email directly to a provided email address
        [HttpPost("send-to-email")]
        public async Task<IActionResult> SendToEmail([FromQuery] string recipientEmail, [FromQuery] string subject, [FromQuery] string body)
        {
            var  model= await _emailService.SendEmailAsync(recipientEmail, subject, body);
            return Ok(model);
        }

        // 2. Send email to a user based on UserName from AspNetUsers
        [HttpPost("send-to-user")]
        public async Task<IActionResult> SendToUser([FromQuery] string userName, [FromQuery] string subject, [FromQuery] string body)
        {
           var model = await _emailService.SendEmailToUserAsync(userName, subject, body);
            return Ok(model);
        }

        // 3. Send email to all users in AspNetUsers
        [HttpPost("send-to-all-users")]
        public async Task<IActionResult> SendToAllUsers([FromQuery] string subject, [FromQuery] string body)
        {
             var model =await _emailService.SendEmailToAllUsersAsync(subject, body);
            return Ok(model);
        }

        // Endpoint to read emails from the inbox
        [HttpGet("inbox")]
        public async Task<IActionResult> ReadInboxEmails()
        {
            var emails = await _emailService.ReadInboxEmailsAsync();
            return Ok(emails);
        }

        // Endpoint to read sent emails
        [HttpGet("sent")]
        public async Task<IActionResult> ReadSentEmails()
        {
            var emails = await _emailService.ReadSentEmailsAsync();
            return Ok(emails);
        }

        // Endpoint to read draft emails
        [HttpGet("drafts")]
        public async Task<IActionResult> ReadDraftEmails()
        {
            var emails = await _emailService.ReadDraftEmailsAsync();
            return Ok(emails);
        }
    }
}
