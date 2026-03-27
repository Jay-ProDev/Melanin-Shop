using Melanin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities
{
    public class Order
    {
        // Propriétés
        public int Id { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public string? CouponCode { get; private set; }

        // Relations
        public Member Member { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address? BillingAddress { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = default!;
        public Payment? Payment { get; private set; }

        // Clés étrangères
        public int MemberId { get; private set; }
        public int ShippingAddressId { get; private set; }
        public int? BillingAddressId { get; private set; }

        // Constructeur EF Core
        private Order() { }

        // Constructeur principal
        public Order(decimal totalPrice, int memberId, int shippingAddressId, int? billingAddressId = null, string? couponCode = null)
        {
            if (totalPrice <= 0)
                throw new ArgumentException("Le prix total doit être supérieur à 0", nameof(totalPrice));

            if (memberId <= 0)
                throw new ArgumentException("Le membre est obligatoire", nameof(memberId));

            if (shippingAddressId <= 0)
                throw new ArgumentException("L'adresse de livraison est obligatoire", nameof(shippingAddressId));

            TotalPrice = totalPrice;
            MemberId = memberId;
            ShippingAddressId = shippingAddressId;
            BillingAddressId = billingAddressId;
            CouponCode = couponCode;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            OrderItems = new List<OrderItem>();
        }
    }
}
