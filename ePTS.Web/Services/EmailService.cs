using ePTS.Models.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ePTS.Web.Services
{
    public class EmailService : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = htmlMessage };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                throw;
            }
        }

        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    var emailMessage = new MimeMessage();

        //    emailMessage.From.Add(new MailboxAddress("Your application name", "yourEmail@yourDomain.com"));
        //    emailMessage.To.Add(new MailboxAddress("", email));
        //    emailMessage.Subject = subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = message
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync("smtp.yourServer.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        //        await client.AuthenticateAsync("yourEmail@yourDomain.com", "yourPassword");
        //        await client.SendAsync(emailMessage);

        //        await client.DisconnectAsync(true);
        //    }
        //}
    }
}
