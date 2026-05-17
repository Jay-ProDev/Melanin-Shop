using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Enums;
using System;

namespace Melanin.Domain.Entities
{
    public class Payment
    {
        // Propriétés
        public int Id { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public string StripeSessionId { get; private set; } = default!;

        // Relations
        public Order Order { get; private set; } = default!;

        // Clés étrangères
        public int OrderId { get; private set; }

        // Constructeur EF Core
        private Payment() { }

        // Constructeur principal
        public Payment(int orderId, decimal amount, string stripeSessionId)
        {
            if (orderId <= 0)
                throw new ArgumentException("La commande est obligatoire", nameof(orderId));

            if (amount <= 0)
                throw new ArgumentException("Le montant doit être supérieur à 0", nameof(amount));

            if (string.IsNullOrWhiteSpace(stripeSessionId))
                throw new ArgumentException("Le sessionId Stripe est obligatoire", nameof(stripeSessionId));

            OrderId = orderId;
            Amount = amount;
            StripeSessionId = stripeSessionId;
            Status = PaymentStatus.Pending;
            PaidAt = null;
        }

        // Méthodes métier
        public void MarkAsCompleted()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidPaymentStatusException("marquer comme complété", Status.ToString());

            Status = PaymentStatus.Completed;
            PaidAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidPaymentStatusException("marquer comme échoué", Status.ToString());

            Status = PaymentStatus.Failed;
        }

        public void MarkAsRefunded()
        {
            if (Status != PaymentStatus.Completed)
                throw new InvalidPaymentStatusException("rembourser", Status.ToString());

            Status = PaymentStatus.Refunded;
        }
    }
}