using Melanin.Application.Interfaces.Utils;
using Melanin.Domain.BusinessExecptions;
using Stripe;
using Stripe.Checkout;

namespace Melanin.Infrastructure.Stripe;

public class StripeUtil : IStripeUtil
{
    // Devise unique du shop (Belgique → euro)
    private const string Currency = "eur";

    // Configuration (état de la classe, rempli par le constructeur)
    private readonly string _secretKey;
    private readonly string _webhookSecret;
    private readonly string _successUrl;
    private readonly string _cancelUrl;

    public StripeUtil(string secretKey, string webhookSecret, string successUrl, string cancelUrl)
    {
        _secretKey = secretKey;
        _webhookSecret = webhookSecret;
        _successUrl = successUrl;
        _cancelUrl = cancelUrl;
    }

    public async Task<(string sessionId, string url)> CreateCheckoutSessionAsync(decimal amount, int orderId)
    {
        // On fournit la clé secrète à la librairie Stripe
        StripeConfiguration.ApiKey = _secretKey;

        // Description de la session de paiement à créer
        SessionCreateOptions options = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = _successUrl,
            CancelUrl = _cancelUrl,
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = Currency,
                        UnitAmount = (long)(amount * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Melanin Shop — Commande n°{orderId}"
                        }
                    }
                }
            },
            Metadata = new Dictionary<string, string>
            {
                { "orderId", orderId.ToString() }
            }
        };

        // Appel à l'API Stripe pour créer la session
        SessionService service = new SessionService();
        Session session = await service.CreateAsync(options);

        return (sessionId: session.Id, url: session.Url);
    }

    public (bool isPaymentCompleted, string sessionId) VerifyAndParseWebhook(string payload, string signature)
    {
        Event stripeEvent;

        // Vérification de la signature : garantit que le webhook vient bien de Stripe
        try
        {
            stripeEvent = EventUtility.ConstructEvent(payload, signature, _webhookSecret);
        }
        catch (StripeException)
        {
            throw new PaymentWebhookException("Signature du webhook invalide");
        }

        // On ne traite qu'un seul type d'event : paiement réussi
        if (stripeEvent.Type != "checkout.session.completed")
            return (isPaymentCompleted: false, sessionId: string.Empty);

        // L'event contient une Session : on en extrait l'identifiant
        Session? session = stripeEvent.Data.Object as Session;

        if (session is null)
            throw new PaymentWebhookException("Impossible de lire la session du webhook");

        return (isPaymentCompleted: true, sessionId: session.Id);
    }
}