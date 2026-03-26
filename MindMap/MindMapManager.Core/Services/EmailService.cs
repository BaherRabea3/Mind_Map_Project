using Microsoft.Extensions.Options;
using MimeKit;
using MindMapManager.Core.Configuration;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;
using MailKit.Net.Smtp;

namespace MindMapManager.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailConfiguration> _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig) 
        {
            _emailConfig = emailConfig;
        }
        public async Task SendPaswordResetEmailAsync(Message message)
        {
            var mailMessage = CreateMessage(message);

            await SendMessageAsync(mailMessage);
        }
        private MimeMessage CreateMessage(Message message)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_emailConfig.Value.UserName, _emailConfig.Value.From));
            mailMessage.To.AddRange(message.To);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Body };

            return mailMessage;
        }
        private async Task SendMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.Value.MailServer, _emailConfig.Value.Port,true);
                     client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.Value.From,_emailConfig.Value.Password);
                    await client.SendAsync(message);
                }
                catch
                {
                    throw;
                }
                finally
                {
                   await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
               
        }
    }
}
