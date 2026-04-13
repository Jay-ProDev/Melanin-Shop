using Melanin.Domain.Entities;

namespace Melanin.Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int id);
    Task<Address?> GetByMemberIdAsync(int memberId);
    Task<Address> AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(int id);
}