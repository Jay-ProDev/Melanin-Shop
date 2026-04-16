using System.ComponentModel.DataAnnotations;

namespace Melanin.API.DTO.Request
{
    public class CreateAddressDTO
    {
        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        [Required]
        public required string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Phone { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }
    }

    public class UpdateAddressDTO
    {
        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        [Required]
        public required string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Phone { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }
    }
}