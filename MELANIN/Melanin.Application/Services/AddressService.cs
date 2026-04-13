using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;

namespace Melanin.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Address?> GetByMemberIdAsync(int memberId)
    {
        return await _addressRepository.GetByMemberIdAsync(memberId);
    }

    public async Task<Address> GetByIdAsync(int id)
    {
        Address? address = await _addressRepository.GetByIdAsync(id);
        if (address is null)
            throw new AddressNotFoundException(id);
        return address;
    }

    public async Task<Address> AddAsync(Address address)
    {
        return await _addressRepository.AddAsync(address);
    }

    public async Task UpdateAsync(Address address)
    {
        Address? existing = await _addressRepository.GetByIdAsync(address.Id);
        if (existing is null)
            throw new AddressNotFoundException(address.Id);
        await _addressRepository.UpdateAsync(address);
    }

    public async Task DeleteAsync(int id)
    {
        Address? existing = await _addressRepository.GetByIdAsync(id);
        if (existing is null)
            throw new AddressNotFoundException(id);
        await _addressRepository.DeleteAsync(id);
    }
}