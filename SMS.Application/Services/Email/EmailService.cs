using SMS.Application.Interfaces.Email;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using SMS.Application.Interfaces.Identity;
using SMS.Common.ViewModels;

namespace SMS.Application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config, IIdentityService identityService)
        {
            _config = config;
            _identityService = identityService;
        }

        public async Task<ResponseModel> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            ResponseModel model = new ResponseModel();
            var smtpSettings = _config.GetSection("SmtpSettings");

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = smtpSettings["Server"];
                smtpClient.Port = int.Parse(smtpSettings["Port"]);
                smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]);
                smtpClient.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    model.IsSuccess = true;
                    model.StatusCode = HttpStatusCode.OK;
                    model.Messsage = "Email sent successfully.";
                }
                catch (SmtpException smtpEx)
                {
                    model.IsSuccess = false;
                    model.StatusCode = HttpStatusCode.InternalServerError;
                    model.Messsage = $"SMTP error: {smtpEx.Message}";
                }
                catch (Exception ex)
                {
                    model.IsSuccess = false;
                    model.StatusCode = HttpStatusCode.InternalServerError;
                    model.Messsage = $"Error sending email: {ex.Message}";
                }

                return model;
            }
        }

        public async Task<ResponseModel> SendEmailToUserAsync(string userName, string subject, string body)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var user = await _identityService.FindByUserNameAsync(userName);
                if (user != null)
                {
                    var emailResult = await SendEmailAsync(user.Email, subject, body);
                    model.IsSuccess = emailResult.IsSuccess;
                    model.Messsage = emailResult.Messsage;
                    model.StatusCode = emailResult.StatusCode;
                }
                else
                {
                    model.IsSuccess = false;
                    model.StatusCode = HttpStatusCode.NotFound;
                    model.Messsage = "User not found.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.StatusCode = HttpStatusCode.InternalServerError;
                model.Messsage = $"Error retrieving user: {ex.Message}";
            }

            return model;
        }

        public async Task<ResponseModel> SendEmailToAllUsersAsync(string subject, string body)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var users = await _identityService.GetAllUsersAsync();
                if (users != null && users.Any())
                {
                    foreach (var user in users)
                    {
                        try
                        {
                            var emailResult = await SendEmailAsync(user.Email, subject, body);
                            if (!emailResult.IsSuccess)
                            {
                                // Log failure for this user if necessary
                                model.Messsage += $"Failed to send email to {user.Email}: {emailResult.Messsage}\n";
                            }
                        }
                        catch (Exception emailEx)
                        {
                            model.Messsage += $"Error sending email to {user.Email}: {emailEx.Message}\n";
                        }
                    }

                    model.IsSuccess = true;
                    model.StatusCode = HttpStatusCode.OK;
                    model.Messsage += "Emails sent to all users.";
                }
                else
                {
                    model.IsSuccess = false;
                    model.StatusCode = HttpStatusCode.NotFound;
                    model.Messsage = "No users found.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.StatusCode = HttpStatusCode.InternalServerError;
                model.Messsage = $"Error retrieving users: {ex.Message}";
            }

            return model;
        }
    }
}
