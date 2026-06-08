using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Application.Interfaces.Utils;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Melanin.Domain.Enums;

namespace Melanin.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderService _orderService;
        private readonly IStripeUtil _stripeUtil;
        private readonly IMailerUtil _mailerUtil;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderService orderService,
            IStripeUtil stripeUtil,
            IMailerUtil mailerUtil)
        {
            _paymentRepository = paymentRepository;
            _orderService = orderService;
            _stripeUtil = stripeUtil;
            _mailerUtil = mailerUtil;
        }

        public async Task<string> CreateCheckoutSessionAsync(int orderId, int memberId)
        {
            // 1. Récupérer l'Order + vérification d'existence et IDOR (déléguée à OrderService)
            Order order = await _orderService.GetByIdForMemberAsync(orderId, memberId);

            // 2. On ne paie qu'une commande en attente de paiement
            if (order.Status != OrderStatus.Pending)
                throw new InvalidOrderStatusException("payer", order.Status.ToString());

            // 3. Créer la session de paiement chez Stripe
            (string sessionId, string url) = await _stripeUtil.CreateCheckoutSessionAsync(order.TotalPrice, order.Id);

            // 4. Créer le Payment en base avec le statut Pending
            Payment payment = new Payment(order.Id, order.TotalPrice, sessionId);
            await _paymentRepository.AddAsync(payment);

            // 5. Retourner l'URL Stripe pour la redirection du client
            return url;
        }

        public async Task HandleWebhookAsync(string payload, string signature)
        {
            // 1. Vérifier la signature + parser le webhook (délégué à StripeUtil)
            (bool isPaymentCompleted, string sessionId) = _stripeUtil.VerifyAndParseWebhook(payload, signature);

            // 2. On ne traite que les paiements réussis ; tout le reste est ignoré
            if (!isPaymentCompleted)
                return;

            // 3. Retrouver le Payment correspondant à cette session Stripe
            Payment? payment = await _paymentRepository.GetByStripeSessionIdAsync(sessionId);
            if (payment is null)
                throw new PaymentNotFoundException(sessionId);

            // 4. Idempotence : si le webhook arrive une 2e fois, le Payment est déjà Completed → on ne refait rien
            if (payment.Status == PaymentStatus.Completed)
                return;

            // 5. Marquer le Payment comme complété
            payment.MarkAsCompleted();
            await _paymentRepository.UpdateAsync(payment);

            // 6. Confirmer la commande associée (Pending → Confirmed)
            await _orderService.ConfirmAsync(payment.OrderId);

            // 7. Envoyer l'email de confirmation au membre
            Order order = await _orderService.GetByIdAsync(payment.OrderId);
            await _mailerUtil.SendOrderConfirmedEmailAsync(order.Member.Email, order.Member.FirstName, order.Id, order.TotalPrice);
        }
    }
}