using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Moq;

namespace Melanin.Tests.Services
{
    public class CartItemServiceTests
    {
        [Fact]
        public async Task AddToCartAsync_StockInsufficient_ShouldThrowProductOutOfStockException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Le produit existe mais n'a que 2 en stock
            Product product = new Product(
                name: "Perruque Lisse",
                description: "Une belle perruque noire super lisse",
                unitPrice: 50m,
                stockQuantity: 2,
                categoryId: 7
            );

            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(4))
                .ReturnsAsync(product);

            CartItemService service = new CartItemService(
                mockCartItemRepository.Object,
                mockProductRepository.Object
            );

            // ===================================================================
            // ACT & ASSERT
            // On demande 5 unités alors qu'il n'y en a que 2 → exception
            // ===================================================================
            await Assert.ThrowsAsync<ProductOutOfStockException>(
                () => service.AddToCartAsync(memberId: 99, productId: 4, quantity: 5)
            );
        }


        [Fact]
        public async Task AddToCartAsync_ProductAlreadyInCart_ShouldUpdateExistingItem()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Produit avec assez de stock
            Product product = new Product(
                name: "Perruque Lisse",
                description: "Une belle perruque noire super lisse",
                unitPrice: 50m,
                stockQuantity: 10,
                categoryId: 7
            );

            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(4))
                .ReturnsAsync(product);

            // Le panier contient DÉJÀ ce produit (productId = 4)
            CartItem existingItem = new CartItem(
                quantity: 2,
                unitPrice: 50m,
                productId: 4,
                memberId: 99
            );

            mockCartItemRepository
                .Setup(repo => repo.GetByMemberIdAsync(99))
                .ReturnsAsync(new List<CartItem> { existingItem });

            CartItemService service = new CartItemService(
                mockCartItemRepository.Object,
                mockProductRepository.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            await service.AddToCartAsync(memberId: 99, productId: 4, quantity: 3);

            // ===================================================================
            // ASSERT
            // On a pris la branche "déjà présent" → UpdateAsync appelé, AddAsync jamais
            // ===================================================================
            mockCartItemRepository.Verify(repo => repo.UpdateAsync(existingItem), Times.Once);
            mockCartItemRepository.Verify(repo => repo.AddAsync(It.IsAny<CartItem>()), Times.Never);
        }


        [Fact]
        public async Task AddToCartAsync_ProductNotInCart_ShouldAddNewItem()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Produit avec assez de stock
            Product product = new Product(
                name: "Perruque Lisse",
                description: "Une belle perruque noire super lisse",
                unitPrice: 50m,
                stockQuantity: 10,
                categoryId: 7
            );

            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(4))
                .ReturnsAsync(product);

            // Le panier est VIDE → le produit n'y est pas encore
            mockCartItemRepository
                .Setup(repo => repo.GetByMemberIdAsync(99))
                .ReturnsAsync(new List<CartItem>());

            CartItemService service = new CartItemService(
                mockCartItemRepository.Object,
                mockProductRepository.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            await service.AddToCartAsync(memberId: 99, productId: 4, quantity: 3);

            // ===================================================================
            // ASSERT
            // On a pris la branche "nouveau" → AddAsync appelé, UpdateAsync jamais
            // ===================================================================
            mockCartItemRepository.Verify(repo => repo.AddAsync(It.IsAny<CartItem>()), Times.Once);
            mockCartItemRepository.Verify(repo => repo.UpdateAsync(It.IsAny<CartItem>()), Times.Never);
        }
    }
}