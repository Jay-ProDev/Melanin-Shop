using System;
using System;

namespace Melanin.Domain.BusinessExecptions
{
    public class PaymentException : Exception
    {
        public PaymentException(string? message) : base(message) { }
    }

    public class PaymentNotFoundException : PaymentException
    {
        public PaymentNotFoundException()
            : base("Paiement introuvable") { }

        public PaymentNotFoundException(int id)
            : base($"Paiement avec l'id {id} introuvable") { }

        public PaymentNotFoundException(string stripeSessionId)
            : base($"Paiement avec le sessionId '{stripeSessionId}' introuvable") { }
    }

    public class InvalidPaymentStatusException : PaymentException
    {
        public InvalidPaymentStatusException(string action, string currentStatus)
            : base($"Action '{action}' impossible sur ce paiement (statut actuel : {currentStatus})") { }
    }

    public class PaymentWebhookException : PaymentException
    {
        public PaymentWebhookException(string message)
            : base($"Erreur webhook paiement : {message}") { }
    }
}