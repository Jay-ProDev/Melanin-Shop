namespace Melanin.API.DTO.Response
{
    public class AddressResponseDTO
    {
        public int Id { get; set; }
        public string City { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? FullName { get; set; }
    }
}