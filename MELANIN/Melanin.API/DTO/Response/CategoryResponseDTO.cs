namespace Melanin.API.DTO.Response
{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }
}