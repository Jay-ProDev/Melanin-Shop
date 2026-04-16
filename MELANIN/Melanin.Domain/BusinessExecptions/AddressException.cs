using System;

namespace Melanin.Domain.BusinessExecptions
{
    public class AddressException : Exception
    {
        public AddressException(string? message) : base(message) { }
    }

    public class AddressNotFoundException : AddressException
    {
        public AddressNotFoundException()
            : base("Adresse introuvable") { }

        public AddressNotFoundException(int id)
            : base($"Adresse avec l'id {id} introuvable") { }
    }
}