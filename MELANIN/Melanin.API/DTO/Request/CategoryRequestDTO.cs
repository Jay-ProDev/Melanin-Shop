using System.ComponentModel.DataAnnotations;

namespace Melanin.API.DTO.Request
{
    public class CreateCategoryDTO
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Slug { get; set; }
    }

    public class UpdateCategoryDTO
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Slug { get; set; }
    }
}