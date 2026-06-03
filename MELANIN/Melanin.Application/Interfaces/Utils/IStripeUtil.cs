namespace Melanin.Application.Interfaces.Utils;

public interface IStripeUtil
{
    Task<(string sessionId, string url)> CreateCheckoutSessionAsync(decimal amount, int orderId);
    (bool isPaymentCompleted, string sessionId) VerifyAndParseWebhook(string payload, string signature);
}