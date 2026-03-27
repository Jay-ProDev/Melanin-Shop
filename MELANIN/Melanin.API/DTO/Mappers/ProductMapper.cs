using Melanin.API.DTO.Request;
using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class ProductMapper
{
    // Product → ProductResponseDTO
    public static ProductResponseDTO ToDto(this Product product)
    {
        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            HairColor = product.HairColor,
            HairLength = product.HairLength,
            HairTexture = product.HairTexture,
            CapSize = product.CapSize,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            CreatedAt = product.CreatedAt
        };
    }

    // CreateProductDTO → Product
    public static Product ToEntity(this CreateProductDTO dto)
    {
        return new Product(
            dto.Name,
            dto.Description,
            dto.UnitPrice,
            dto.StockQuantity,
            dto.CategoryId,
            dto.HairColor,
            dto.HairLength,
            dto.HairTexture,
            dto.CapSize
        );
    }
}