using Melanin.Application.Interfaces.Repositories;
using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Melanin.Infrastructure.Database.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly MelaninDbContext _dbContext;

    public AddressRepository(MelaninDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Address?> GetByIdAsync(int id)
    {
        return await _dbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Address?> GetByMemberIdAsync(int memberId)
    {
        return await _dbContext.Addresses
            .OrderBy(a => a.Id)
            .FirstOrDefaultAsync(a => a.MemberId == memberId);
    }

    public async Task<Address> AddAsync(Address address)
    {
        await _dbContext.Addresses.AddAsync(address);
        await _dbContext.SaveChangesAsync();
        return address;
    }

    public async Task UpdateAsync(Address address)
    {
        _dbContext.Addresses.Update(address);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Address? address = await _dbContext.Addresses.FindAsync(id);
        _dbContext.Addresses.Remove(address!);
        await _dbContext.SaveChangesAsync();
    }
}