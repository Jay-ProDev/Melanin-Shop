namespace Melanin.API.DTO.Response
{
    public class OrderItemResponseDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string? ProductImageUrl { get; set; }

        // Caractéristiques (comme CartItemResponseDTO)
        public string? HairColor { get; set; }
        public string? HairLength { get; set; }
        public string? HairTexture { get; set; }
        public string? CapSize { get; set; }
    }
}