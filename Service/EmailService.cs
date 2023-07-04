using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

using MimeKit.Text;
using ShoeShop.Helper;
using ShoeShop.Interfaces;

namespace ShoeShop.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string email, string username, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Cửa hàng giày Huy Sport", _smtpSettings.Username));
            mail.To.Add(new MailboxAddress(username, email));
            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Plain) { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }
        }
    }

}
