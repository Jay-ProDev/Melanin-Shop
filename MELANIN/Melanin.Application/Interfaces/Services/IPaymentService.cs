namespace Melanin.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<string> CreateCheckoutSessionAsync(int orderId, int memberId);
    Task HandleWebhookAsync(string payload, string signature);
}

