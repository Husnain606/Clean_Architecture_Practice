using SMS.Common.ViewModels;

namespace SMS.Application.Interfaces.Email
{
    public interface IEmailService
    {
        Task<ResponseModel> SendEmailAsync(string recipientEmail, string subject, string body);
        Task<ResponseModel> SendEmailToUserAsync(string userName, string subject, string body);
        Task<ResponseModel> SendEmailToAllUsersAsync(string subject, string body);
    }
}
