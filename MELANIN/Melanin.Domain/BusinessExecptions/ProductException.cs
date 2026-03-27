using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.BusinessExecptions
{
    public class ProductException : Exception
    {
        public ProductException(string? message) : base(message) { }
    }

    public class ProductNotFoundException : ProductException
    {
        public ProductNotFoundException()
            : base("Produit introuvable") { }

        public ProductNotFoundException(int id)
            : base($"Produit avec l'id {id} introuvable") { }
    }

    public class ProductOutOfStockException : ProductException
    {
        public ProductOutOfStockException(string name)
            : base($"Stock insuffisant pour le produit '{name}'") { }
    }
}