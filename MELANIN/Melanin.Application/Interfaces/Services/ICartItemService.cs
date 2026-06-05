using Melanin.Domain.Entities;

namespace Melanin.Application.Interfaces.Services;

public interface ICartItemService
{
    Task<IEnumerable<CartItem>> GetCartByMemberIdAsync(int memberId);
    Task<CartItem> GetByIdForMemberAsync(int cartItemId, int memberId);
    Task AddToCartAsync(int memberId, int productId, int quantity);
    Task UpdateQuantityAsync(int cartItemId, int memberId, int quantity);
    Task RemoveFromCartAsync(int cartItemId, int memberId);
    Task ClearCartAsync(int memberId);
}