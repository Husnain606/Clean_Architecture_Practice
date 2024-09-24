using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using SMS.Application.Interfaces.Email;
using SMS.Application.Interfaces.Identity;
using SMS.Common.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit;

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
            var model = new ResponseModel();
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
            var model = new ResponseModel();
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
            var model = new ResponseModel();
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

        public async Task<List<EmailResponseModel>> ReadInboxEmailsAsync()
        {
            return await ReadEmailsFromFolderAsync("INBOX");
        }

        public async Task<List<EmailResponseModel>> ReadSentEmailsAsync()
        {
            return await ReadEmailsFromFolderAsync("[Gmail]/Sent Mail"); // Adjust for your email provider
        }

        public async Task<List<EmailResponseModel>> ReadDraftEmailsAsync()
        {
            return await ReadEmailsFromFolderAsync("[Gmail]/Drafts"); // Adjust for your email provider
        }

        private async Task<List<EmailResponseModel>> ReadEmailsFromFolderAsync(string folderName)
        {
            var emailList = new List<EmailResponseModel>();
            using (var client = new ImapClient())
            {
                var imapServer = _config["EmailSettings:ImapServer"];
                var imapPort = int.Parse(_config["EmailSettings:ImapPort"]);
                var email = _config["EmailSettings:Email"];
                var password = _config["EmailSettings:Password"];

                await client.ConnectAsync(imapServer, imapPort, true);
                await client.AuthenticateAsync(email, password);

                var folder = client.GetFolder(folderName);
                await folder.OpenAsync(FolderAccess.ReadOnly);
                var uids = await folder.SearchAsync(SearchQuery.All);

                int count = 0;
                foreach (var uid in uids)
                {
                    var message = await folder.GetMessageAsync(uid);

                    // Check if the body is of type TextPart to get the encoding
                    string body = null;
                    string encodingName = "Unknown";

                    if (message.Body is TextPart textPart)
                    {
                         body = textPart.Text;
                        encodingName = textPart.ContentType.Charset ?? "Unknown"; // Get encoding if available
                    }
                    else if (message.Body is Multipart multipart) // Handle multipart messages (e.g., HTML + plain text)
                    {
                        foreach (var part in multipart)
                        {
                            if (part is TextPart partText)
                            {
                                 body = partText.Text;
                                encodingName = partText.ContentType.Charset ?? "Unknown";
                                break; // Prefer the first text part
                            }
                        }
                    }

                    // Map MimeMessage to EmailResponseModel
                    var emailResponse = new EmailResponseModel
                    {
                        Sender = message.From.Mailboxes.FirstOrDefault()?.Address ?? "Unknown",
                        Recipient = message.To.Mailboxes.FirstOrDefault()?.Address ?? "Unknown",
                        Subject = message.Subject,
                        Body = body ?? "No body", // Handle null body
                        EncodingName = encodingName
                    };

                    emailList.Add(emailResponse);
                    count++;
                    if (count == 8) break; // Limit to the first 4 emails
                }

                await client.DisconnectAsync(true);
            }

            return emailList;
        }
    }
}
