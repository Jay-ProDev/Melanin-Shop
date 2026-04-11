using Melanin.Domain.Entities;

public interface ICartItemRepository
{
    Task<CartItem?> GetByIdAsync(int id);
    Task<IEnumerable<CartItem>> GetByMemberIdAsync(int memberId);
    Task<CartItem> AddAsync(CartItem cartItem);
    Task UpdateAsync(CartItem cartItem);
    Task DeleteAsync(int id);
    Task ClearCartAsync(int memberId);
}