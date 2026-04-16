using Melanin.Domain.Entities;

namespace Melanin.Application.Interfaces.Services;

public interface IAddressService
{
    Task<Address?> GetByMemberIdAsync(int memberId);
    Task<Address> GetByIdAsync(int id);
    Task<Address> AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(int id);
}