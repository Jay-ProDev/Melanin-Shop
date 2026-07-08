using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Melanin.Tests.TestHelpers;
using Moq;

namespace Melanin.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task UpdateAsync_CategoryDoesNotExist_ShouldThrowCategoryNotFoundException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<ICategoryRepository> mockCategoryRepository = new Mock<ICategoryRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // La catégorie qu'on veut mettre à jour
            Category category = new Category("Perruques", "wigs");
            TestHelper.SetId(category, 1);

            // La catégorie n'existe pas en base → GetByIdAsync renvoie null
            mockCategoryRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Category?)null);

            CategoryService service = new CategoryService(mockCategoryRepository.Object);

            // ===================================================================
            // ACT & ASSERT
            // ===================================================================
            await Assert.ThrowsAsync<CategoryNotFoundException>(
                () => service.UpdateAsync(category)
            );
        }


        [Fact]
        public async Task UpdateAsync_SlugUsedByAnotherCategory_ShouldThrowCategoryAlreadyExistsException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<ICategoryRepository> mockCategoryRepository = new Mock<ICategoryRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // La version qu'on veut sauvegarder : Id = 1, on lui met le slug "wigs"
            Category updatedCategory = new Category("Perruques Premium", "wigs");
            TestHelper.SetId(updatedCategory, 1);

            // La catégorie existe bien en base
            mockCategoryRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(updatedCategory);

            // PROBLÈME : le slug "wigs" est déjà utilisé par une AUTRE catégorie (Id = 2)
            Category conflictingCategory = new Category("Tresses", "wigs");
            TestHelper.SetId(conflictingCategory, 2);

            mockCategoryRepository
                .Setup(repo => repo.GetBySlugAsync("wigs"))
                .ReturnsAsync(conflictingCategory);

            CategoryService service = new CategoryService(mockCategoryRepository.Object);

            // ===================================================================
            // ACT & ASSERT
            // ===================================================================
            await Assert.ThrowsAsync<CategoryAlreadyExistsException>(
                () => service.UpdateAsync(updatedCategory)
            );
        }


        [Fact]
        public async Task UpdateAsync_SlugBelongsToSameCategory_ShouldUpdate()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<ICategoryRepository> mockCategoryRepository = new Mock<ICategoryRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // La version ACTUELLE en base : Id = 1, nom "Perruques", slug "wigs"
            Category categoryInDb = new Category("Perruques", "wigs");
            TestHelper.SetId(categoryInDb, 1);

            // La version MODIFIÉE qu'on veut sauvegarder : même Id = 1, même slug,
            // mais le NOM a changé ("Perruques Premium")
            Category updatedCategory = new Category("Perruques Premium", "wigs");
            TestHelper.SetId(updatedCategory, 1);

            // Le repo trouve bien la catégorie existante par son Id
            mockCategoryRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(categoryInDb);

            // Le slug "wigs" existe... mais il appartient à la MÊME catégorie (Id = 1)
            // → ce n'est pas un conflit, la mise à jour doit passer
            mockCategoryRepository
                .Setup(repo => repo.GetBySlugAsync("wigs"))
                .ReturnsAsync(categoryInDb);

            CategoryService service = new CategoryService(mockCategoryRepository.Object);

            // ===================================================================
            // ACT
            // ===================================================================
            await service.UpdateAsync(updatedCategory);

            // ===================================================================
            // ASSERT — la mise à jour a bien été déléguée au repository
            // ===================================================================
            mockCategoryRepository.Verify(repo => repo.UpdateAsync(updatedCategory), Times.Once);
        }
    }
}