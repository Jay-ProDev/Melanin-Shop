using Melanin.Domain.Enums;

namespace Melanin.API.DTO.Response
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string? HairColor { get; set; }
        public HairLength? HairLength { get; set; }
        public HairTexture? HairTexture { get; set; }
        public CapSize? CapSize { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}