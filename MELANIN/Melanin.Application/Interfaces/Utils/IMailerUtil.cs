namespace Melanin.Application.Interfaces.Utils;

public interface IMailerUtil
{
    Task SendWelcomeEmailAsync(string toEmail, string firstName);
    Task SendOrderConfirmedEmailAsync(string toEmail, string firstName, int orderId, decimal totalPrice);
    Task SendOrderShippedEmailAsync(string toEmail, string firstName, int orderId);
    Task SendOrderDeliveredEmailAsync(string toEmail, string firstName, int orderId);
}