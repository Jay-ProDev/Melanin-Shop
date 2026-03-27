using Melanin.API.DTO.Request;
using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class CategoryMapper
{
    // Category → CategoryResponseDTO
    public static CategoryResponseDTO ToDto(this Category category)
    {
        return new CategoryResponseDTO
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug
        };
    }

    // CreateCategoryDTO → Category
    public static Category ToEntity(this CreateCategoryDTO dto)
    {
        return new Category(
            dto.Name,
            dto.Slug
        );
    }
}