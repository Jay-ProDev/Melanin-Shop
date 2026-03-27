using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;

namespace Melanin.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            // Vérifier que la catégorie existe
            Category? category = await _categoryRepository.GetByIdAsync(product.CategoryId);
            if (category is null)
                throw new CategoryNotFoundException(product.CategoryId);
            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Product? product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new ProductNotFoundException(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await _productRepository.GetAllActiveAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _productRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            return await _productRepository.SearchAsync(keyword);
        }

        public async Task UpdateAsync(Product product)
        {
            Product? productInDB = await _productRepository.GetByIdAsync(product.Id);
            if (productInDB is null)
                throw new ProductNotFoundException();

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            Product? product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new ProductNotFoundException();

            await _productRepository.DeleteAsync(id);
        }
    }
}