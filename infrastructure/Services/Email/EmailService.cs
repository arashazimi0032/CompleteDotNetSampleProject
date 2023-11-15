using Application.Abstractions.Email;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private const string FromEmailAddress = "gunnar45@ethereal.email";
    private const string FromEmailPassword = "FZzARQJY7xqm5VY7sU";
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(MailboxAddress.Parse(FromEmailAddress));
        emailMessage.To.Add(MailboxAddress.Parse(email));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        emailMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(FromEmailAddress, FromEmailPassword);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}