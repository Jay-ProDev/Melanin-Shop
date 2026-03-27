using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities
{
    public class CartItem
    {
        // Propriétés
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // Relations
        public Product Product { get; private set; } = default!;
        public Member Member { get; private set; } = default!;

        // Clés étrangères
        public int ProductId { get; private set; }
        public int MemberId { get; private set; }

        // Constructeur EF Core
        private CartItem() { }

        // Constructeur principal
        public CartItem(int quantity, decimal unitPrice, int productId, int memberId)
        {
            if (quantity <= 0)
                throw new ArgumentException("La quantité doit être supérieure à 0", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("Le prix doit être supérieur à 0", nameof(unitPrice));

            if (productId <= 0)
                throw new ArgumentException("Le produit est obligatoire", nameof(productId));

            if (memberId <= 0)
                throw new ArgumentException("Le membre est obligatoire", nameof(memberId));

            Quantity = quantity;
            UnitPrice = unitPrice;
            ProductId = productId;
            MemberId = memberId;
        }
    }
}
