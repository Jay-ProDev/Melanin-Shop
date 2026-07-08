using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Moq;

namespace Melanin.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task DeleteAsync_ProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // ===================================================================
            // MOCKING — Simulation des dépendances
            // Les deux mocks sont créés car le constructeur de ProductService
            // les exige tous les deux. Seul productRepository sera configuré ;
            // categoryRepository n'est là que pour satisfaire le constructeur.
            // ===================================================================
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCategoryRepository = new Mock<ICategoryRepository>();

            // ===================================================================
            // ARRANGE — Préparation
            // ===================================================================

            // Le produit n'existe pas → le repo renvoie null
            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Product?)null);

            ProductService service = new ProductService(
                mockProductRepository.Object,
                mockCategoryRepository.Object
            );

            // ===================================================================
            // ACT & ASSERT — l'exception est attendue
            // ===================================================================
            await Assert.ThrowsAsync<ProductNotFoundException>(
                () => service.DeleteAsync(99)
            );
        }


        [Fact]
        public async Task DeleteAsync_ProductExists_ShouldCallRepositoryDelete()
        {
            // ===================================================================
            // MOCKING — Simulation des dépendances
            // ===================================================================
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ICategoryRepository> mockCategoryRepository = new Mock<ICategoryRepository>();

            // ===================================================================
            // ARRANGE — Préparation
            // ===================================================================

            // Le produit existe → le repo le renvoie quand on cherche l'id 1
            Product existingProduct = new Product(
                name: "Perruque Lisse",
                description: "Une belle perruque",
                unitPrice: 50m,
                stockQuantity: 10,
                categoryId: 1
            );

            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingProduct);

            ProductService service = new ProductService(
                mockProductRepository.Object,
                mockCategoryRepository.Object
            );

            // ===================================================================
            // ACT — Action
            // ===================================================================
            await service.DeleteAsync(1);

            // ===================================================================
            // ASSERT — Vérification du COMPORTEMENT
            // On vérifie que la suppression a bien été déléguée au repository
            // ===================================================================
            mockProductRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}