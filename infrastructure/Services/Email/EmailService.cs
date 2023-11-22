using Application.Abstractions.Email;
using Application.ConfigOptions;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly EtherealEmailOptions _emailOptions;

    public EmailService(IOptions<EtherealEmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(MailboxAddress.Parse(_emailOptions.EtherealEmail));
        emailMessage.To.Add(MailboxAddress.Parse(email));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        emailMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailOptions.EtherealEmail, _emailOptions.EtherealEmailPassword);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}