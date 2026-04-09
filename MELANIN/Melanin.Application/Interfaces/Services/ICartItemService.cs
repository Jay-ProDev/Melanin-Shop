using Melanin.Domain.Entities;

namespace Melanin.Application.Interfaces.Services;

public interface ICartItemService
{
    Task<IEnumerable<CartItem>> GetCartByMemberIdAsync(int memberId);
    Task AddToCartAsync(int memberId, int productId, int quantity);
    Task UpdateQuantityAsync(int cartItemId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(int memberId);
}