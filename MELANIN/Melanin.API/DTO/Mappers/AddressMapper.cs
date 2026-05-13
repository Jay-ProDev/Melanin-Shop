using Melanin.API.DTO.Request;
using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class AddressMapper
{
    // Address → AddressResponseDTO
    public static AddressResponseDTO ToDto(this Address address)
    {
        return new AddressResponseDTO
        {
            Id = address.Id,                            // AddressResponseDTO.Id = address.Id
            City = address.City,                        // AddressResponseDTO.City = address.City
            PostalCode = address.PostalCode,
            Country = address.Country,
            Street = address.Street,
            Phone = address.Phone,
            FullName = address.FullName
        };
    }

    // CreateAddressDTO → Address
    public static Address ToEntity(this CreateAddressDTO dto, int memberId)
    {
        return new Address(
            dto.City,
            dto.PostalCode,
            dto.Country,
            dto.Street,
            dto.Phone,
            memberId,
            dto.FullName
        );
    }
}