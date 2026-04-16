using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class CartItemMapper
{
    public static CartItemResponseDTO ToDto(this CartItem cartItem)
    {
        return new CartItemResponseDTO
        {
            Id = cartItem.Id,
            Quantity = cartItem.Quantity,
            UnitPrice = cartItem.UnitPrice,
            TotalPrice = cartItem.Quantity * cartItem.UnitPrice,
            ProductId = cartItem.ProductId,
            ProductName = cartItem.Product.Name,
            ProductImageUrl = cartItem.Product.ImageUrl,
            HairColor = cartItem.Product.HairColor,
            HairLength = cartItem.Product.HairLength?.ToString(),
            HairTexture = cartItem.Product.HairTexture?.ToString(),
            CapSize = cartItem.Product.CapSize?.ToString()
        };
    }
    //ToDto → vers la réponse(on lit en base → on renvoie au client)
}