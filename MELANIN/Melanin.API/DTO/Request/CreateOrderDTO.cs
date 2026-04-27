using System.ComponentModel.DataAnnotations;

namespace Melanin.API.DTO.Request
{
    public class CreateOrderDTO
    {
        [Required]
        public required int ShippingAddressId { get; set; }

        public int? BillingAddressId { get; set; }

        [MaxLength(50)]
        public string? CouponCode { get; set; }
    }
}