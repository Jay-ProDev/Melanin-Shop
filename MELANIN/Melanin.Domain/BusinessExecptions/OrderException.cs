using System;

namespace Melanin.Domain.BusinessExecptions
{
    public class OrderException : Exception
    {
        public OrderException(string? message) : base(message) { }
    }

    public class OrderNotFoundException : OrderException
    {
        public OrderNotFoundException()
            : base("Commande introuvable") { }

        public OrderNotFoundException(int id)
            : base($"Commande avec l'id {id} introuvable") { }
    }

    public class EmptyCartException : OrderException
    {
        public EmptyCartException()
            : base("Impossible de créer une commande : le panier est vide") { }
    }

    public class InvalidOrderStatusException : OrderException
    {
        public InvalidOrderStatusException(string action, string currentStatus)
            : base($"Action '{action}' impossible sur cette commande (statut actuel : {currentStatus})") { }
    }
}