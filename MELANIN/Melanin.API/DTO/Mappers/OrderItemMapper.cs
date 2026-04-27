using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class OrderItemMapper
{
    // OrderItem → OrderItemResponseDTO
    public static OrderItemResponseDTO ToDto(this OrderItem orderItem)
    {
        return new OrderItemResponseDTO
        {
            Id = orderItem.Id,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.Quantity * orderItem.UnitPrice,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product.Name,
            ProductImageUrl = orderItem.Product.ImageUrl,
            HairColor = orderItem.Product.HairColor,
            HairLength = orderItem.Product.HairLength?.ToString(),
            HairTexture = orderItem.Product.HairTexture?.ToString(),
            CapSize = orderItem.Product.CapSize?.ToString()
        };
    }

    //ToDto → vers la réponse (on lit en base → on renvoie au client)
}