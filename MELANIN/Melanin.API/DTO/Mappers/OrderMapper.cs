using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class OrderMapper
{
    // Order → OrderResponseDTO
    public static OrderResponseDTO ToDto(this Order order)
    {
        return new OrderResponseDTO
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            CouponCode = order.CouponCode,

            MemberId = order.MemberId,
            MemberFirstName = order.Member.FirstName,
            MemberLastName = order.Member.LastName,

            ShippingAddress = order.ShippingAddress.ToDto(),
            BillingAddress = order.BillingAddress?.ToDto(),

            OrderItems = order.OrderItems.Select(oi => oi.ToDto()).ToList()
        };
    }

    //ToDto → vers la réponse (on lit en base → on renvoie au client)
}