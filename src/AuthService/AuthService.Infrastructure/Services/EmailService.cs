namespace AuthService.Infrastructure.Services;

using AuthService.Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

public class EmailService(IOptions<SmtpOptions> smtpOptions) : IEmailSender
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var body = new TextPart(TextFormat.Html);

        body.Text = htmlMessage;

        using var message = new MimeMessage();

        message.From.Add(new MailboxAddress("FriendsAppEmailService", _smtpOptions.UserName));
        message.To.Add(new MailboxAddress(null, email));
        message.Subject = subject;
        message.Body = body;

        using var client = new SmtpClient();

        await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, true);

        await client.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }
}