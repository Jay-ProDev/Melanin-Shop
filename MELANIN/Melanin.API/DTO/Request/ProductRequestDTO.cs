using Melanin.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Melanin.API.DTO.Request
{
    public class CreateProductDTO
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
        public required decimal UnitPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif")]
        public required int StockQuantity { get; set; }

        [Required]
        public required int CategoryId { get; set; }

        [MaxLength(50)]
        public string? HairColor { get; set; }

        public HairLength? HairLength { get; set; }
        public HairTexture? HairTexture { get; set; }
        public CapSize? CapSize { get; set; }
    }

    public class UpdateProductDTO
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
        public required decimal UnitPrice { get; set; }

        [Required]
        public required int CategoryId { get; set; }

        [MaxLength(50)]
        public string? HairColor { get; set; }

        public HairLength? HairLength { get; set; }
        public HairTexture? HairTexture { get; set; }
        public CapSize? CapSize { get; set; }
    }

    public class UpdateStockDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
        public required int Quantity { get; set; }
    }
}