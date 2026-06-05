using Melanin.Domain.Entities;

namespace Melanin.Application.Interfaces.Services;

public interface IAddressService
{
    Task<Address?> GetByMemberIdAsync(int memberId);
    Task<Address> GetByIdAsync(int id);
    Task<Address> GetByIdForMemberAsync(int id, int memberId);
    Task<Address> AddAsync(Address address);
    Task UpdateAsync(Address address, int memberId);
    Task DeleteAsync(int id, int memberId);
}