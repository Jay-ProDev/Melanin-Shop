using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities;

public class OrderItem
{
    // Propriétés
    public int Id { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    // Relations
    public Order Order { get; private set; } = default!;
    public Product Product { get; private set; } = default!;

    // Clés étrangères
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }

    // Constructeur EF Core
    private OrderItem() { }

    // Constructeur principal
    public OrderItem(int quantity, decimal unitPrice, int orderId, int productId)
    {
        if (quantity <= 0)
            throw new ArgumentException("La quantité doit être supérieure à 0", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("Le prix doit être supérieur à 0", nameof(unitPrice));

        if (orderId <= 0)
            throw new ArgumentException("La commande est obligatoire", nameof(orderId));

        if (productId <= 0)
            throw new ArgumentException("Le produit est obligatoire", nameof(productId));

        Quantity = quantity;
        UnitPrice = unitPrice;
        OrderId = orderId;
        ProductId = productId;
    }
}