using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;

namespace Melanin.Application.Services;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;

    public CartItemService(ICartItemRepository cartItemRepository, IProductRepository productRepository)
    {
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<CartItem>> GetCartByMemberIdAsync(int memberId)
    {
        return await _cartItemRepository.GetByMemberIdAsync(memberId);
    }

    public async Task AddToCartAsync(int memberId, int productId, int quantity)
    {
        // Récupérer le produit pour avoir le prix et vérifier le stock
        Product? product = await _productRepository.GetByIdAsync(productId);

        // Vérifier le stock
        if (product!.StockQuantity < quantity)
            throw new ProductOutOfStockException(product.Name);

        // Vérifier si le produit est déjà dans le panier
        IEnumerable<CartItem> cartItems = await _cartItemRepository.GetByMemberIdAsync(memberId);
        CartItem? existingItem = cartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            await _cartItemRepository.UpdateAsync(existingItem);
        }
        else
        {
            CartItem cartItem = new CartItem(quantity, product.UnitPrice, productId, memberId);
            await _cartItemRepository.AddAsync(cartItem);
        }
    }

    public async Task UpdateQuantityAsync(int cartItemId, int quantity)
    {
        CartItem? cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
        if (cartItem is null)
            throw new CartItemNotFoundException(cartItemId);

        cartItem.UpdateQuantity(quantity);
        await _cartItemRepository.UpdateAsync(cartItem);
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        CartItem? cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
        if (cartItem is null)
            throw new CartItemNotFoundException(cartItemId);

        await _cartItemRepository.DeleteAsync(cartItemId);
    }

    public async Task ClearCartAsync(int memberId)
    {
        await _cartItemRepository.ClearCartAsync(memberId);
    }
}