using Melanin.Application.Interfaces.Repositories;
using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Melanin.Infrastructure.Database.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly MelaninDbContext _dbContext;

    public CartItemRepository(MelaninDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        return await _dbContext.CartItems 
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == id);
    }

    public async Task<IEnumerable<CartItem>> GetByMemberIdAsync(int memberId)
    {
        return await _dbContext.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<CartItem> AddAsync(CartItem cartItem)
    {
        await _dbContext.CartItems.AddAsync(cartItem);
        await _dbContext.SaveChangesAsync();
        return await _dbContext.CartItems
            .Include(ci => ci.Product)
            .FirstAsync(ci => ci.Id == cartItem.Id);
    }

    public async Task UpdateAsync(CartItem cartItem)
    {
        _dbContext.CartItems.Update(cartItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        CartItem? cartItem = await _dbContext.CartItems.FindAsync(id);
        _dbContext.CartItems.Remove(cartItem!);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ClearCartAsync(int memberId)
    {
        List<CartItem> cartItems = await _dbContext.CartItems
            .Where(ci => ci.MemberId == memberId)
            .ToListAsync();

        _dbContext.CartItems.RemoveRange(cartItems);
        await _dbContext.SaveChangesAsync();
    }
}