using Melanin.Domain.BusinessExecptions;
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
        public ICollection<Payment> Payments { get; private set; } = default!;

        // Clés étrangères
        public int MemberId { get; private set; }
        public int ShippingAddressId { get; private set; }
        public int? BillingAddressId { get; private set; }

        // Constructeur EF Core
        private Order() { }

        // Constructeur principal
        public Order(int memberId, int shippingAddressId, int? billingAddressId = null, string? couponCode = null)
        {
            if (memberId <= 0)
                throw new ArgumentException("Le membre est obligatoire", nameof(memberId));

            if (shippingAddressId <= 0)
                throw new ArgumentException("L'adresse de livraison est obligatoire", nameof(shippingAddressId));

            MemberId = memberId;
            ShippingAddressId = shippingAddressId;
            BillingAddressId = billingAddressId;
            CouponCode = couponCode;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            TotalPrice = 0m;
            OrderItems = new List<OrderItem>();
            Payments = new List<Payment>();
        }
        public void AddItem(OrderItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            OrderItems.Add(item);
            TotalPrice += item.UnitPrice * item.Quantity;
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOrderStatusException("confirmer", Status.ToString());

            Status = OrderStatus.Confirmed;
        }

        public void Ship()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOrderStatusException("expédier", Status.ToString());

            Status = OrderStatus.Shipped;
        }

        public void Deliver()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOrderStatusException("livrer", Status.ToString());

            Status = OrderStatus.Delivered;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
                throw new InvalidOrderStatusException("annuler", Status.ToString());

            Status = OrderStatus.Cancelled;
        }

    }
}
