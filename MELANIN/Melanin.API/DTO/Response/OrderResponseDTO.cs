using Melanin.Domain.Enums;

namespace Melanin.API.DTO.Response
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string? CouponCode { get; set; }

        public int MemberId { get; set; }
        public string MemberFirstName { get; set; } = default!;
        public string MemberLastName { get; set; } = default!;
        public string MemberEmail { get; set; } = default!;

        public AddressResponseDTO ShippingAddress { get; set; } = default!;
        public AddressResponseDTO? BillingAddress { get; set; }

        public ICollection<OrderItemResponseDTO> OrderItems { get; set; } = default!;
    }
}