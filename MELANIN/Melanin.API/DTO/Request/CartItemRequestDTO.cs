using System.ComponentModel.DataAnnotations;

namespace Melanin.API.DTO.Request;

public class AddToCartDTO
{
    [Required]
    public required int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
    public required int Quantity { get; set; }
}

public class UpdateCartItemDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
    public required int Quantity { get; set; }
}