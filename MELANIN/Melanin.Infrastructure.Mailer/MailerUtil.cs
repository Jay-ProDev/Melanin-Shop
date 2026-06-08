using Melanin.Application.Interfaces.Utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Melanin.Infrastructure.Mailer;

public class MailerUtil : IMailerUtil
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _appEmail;
    private readonly string _appName;

    public MailerUtil(string host, int port, string username, string password, string appEmail, string appName)
    {
        _host = host;
        _port = port;
        _username = username;
        _password = password;
        _appEmail = appEmail;
        _appName = appName;
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string firstName)
    {
        MimeMessage message = new MimeMessage();
        message.Subject = "Bienvenue sur Melanin Shop !";
        message.From.Add(new MailboxAddress(_appName, _appEmail));
        message.To.Add(new MailboxAddress(firstName, toEmail));

        string html = await LoadTemplateAsync("WelcomeTemplate.html");
        html = html.Replace("{{FirstName}}", firstName);

        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Bienvenue sur Melanin Shop !";
        bodyBuilder.HtmlBody = html;
        message.Body = bodyBuilder.ToMessageBody();

        await SendMailAsync(message);
    }

    public async Task SendOrderConfirmedEmailAsync(string toEmail, string firstName, int orderId, decimal totalPrice)
    {
        MimeMessage message = new MimeMessage();
        message.Subject = $"Commande #{orderId} confirmée";
        message.From.Add(new MailboxAddress(_appName, _appEmail));
        message.To.Add(new MailboxAddress(firstName, toEmail));

        string html = await LoadTemplateAsync("OrderConfirmedTemplate.html");
        html = html.Replace("{{FirstName}}", firstName)
                   .Replace("{{OrderId}}", orderId.ToString())
                   .Replace("{{TotalPrice}}", totalPrice.ToString("C"));

        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = $"Ta commande #{orderId} a été confirmée.";
        bodyBuilder.HtmlBody = html;
        message.Body = bodyBuilder.ToMessageBody();

        await SendMailAsync(message);
    }

    public async Task SendOrderShippedEmailAsync(string toEmail, string firstName, int orderId)
    {
        MimeMessage message = new MimeMessage();
        message.Subject = $"Commande #{orderId} expédiée";
        message.From.Add(new MailboxAddress(_appName, _appEmail));
        message.To.Add(new MailboxAddress(firstName, toEmail));

        string html = await LoadTemplateAsync("OrderShippedTemplate.html");
        html = html.Replace("{{FirstName}}", firstName)
                   .Replace("{{OrderId}}", orderId.ToString());

        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = $"Ta commande #{orderId} est en route !";
        bodyBuilder.HtmlBody = html;
        message.Body = bodyBuilder.ToMessageBody();

        await SendMailAsync(message);
    }

    public async Task SendOrderDeliveredEmailAsync(string toEmail, string firstName, int orderId)
    {
        MimeMessage message = new MimeMessage();
        message.Subject = $"Commande #{orderId} livrée";
        message.From.Add(new MailboxAddress(_appName, _appEmail));
        message.To.Add(new MailboxAddress(firstName, toEmail));

        string html = await LoadTemplateAsync("OrderDeliveredTemplate.html");
        html = html.Replace("{{FirstName}}", firstName)
                   .Replace("{{OrderId}}", orderId.ToString());

        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = $"Ta commande #{orderId} a été livrée.";
        bodyBuilder.HtmlBody = html;
        message.Body = bodyBuilder.ToMessageBody();

        await SendMailAsync(message);
    }

    private async Task SendMailAsync(MimeMessage message)
    {
        using SmtpClient smtpClient = new SmtpClient();
        try
        {
            await smtpClient.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_username, _password);
            await smtpClient.SendAsync(message);
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }

    private async Task<string> LoadTemplateAsync(string fileName)
    {
        string basePath = AppContext.BaseDirectory;
        string fullPath = Path.Combine(basePath, "Templates", fileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Template not found: {fullPath}", fullPath);

        return await File.ReadAllTextAsync(fullPath);
    }
}