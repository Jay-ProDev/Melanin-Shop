using Melanin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities
{
    public class Payment
    {
        // Propriétés
        public string Id { get; private set; } = default!;
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public string? StripeSessionId { get; private set; }

        // Relations
        public Order Order { get; private set; } = default!;

        // Clés étrangères
        public int OrderId { get; private set; }

        // Constructeur EF Core
        private Payment() { }

        // Constructeur principal
        public Payment(string id, decimal amount, int orderId, string? stripeSessionId = null)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("L'identifiant est obligatoire", nameof(id));

            if (amount <= 0)
                throw new ArgumentException("Le montant doit être supérieur à 0", nameof(amount));

            if (orderId <= 0)
                throw new ArgumentException("La commande est obligatoire", nameof(orderId));

            Id = id;
            Amount = amount;
            OrderId = orderId;
            StripeSessionId = stripeSessionId;
            Status = PaymentStatus.Pending;
            PaidAt = null;
        }
    }
}
