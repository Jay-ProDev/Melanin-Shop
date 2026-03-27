using Melanin.Application.Interfaces;
using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;

namespace Melanin.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            // On appelle le REPOSITORY directement → null est normal ici
            Category? existing = await _categoryRepository.GetBySlugAsync(category.Slug);
            if (existing is not null)
                throw new CategoryAlreadyExistsException(category.Slug);

            return await _categoryRepository.AddAsync(category);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                throw new CategoryNotFoundException(id);
            return category;
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
           
            Category? category = await _categoryRepository.GetBySlugAsync(slug);
            if (category is null)
                throw new CategoryNotFoundException();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            Category? categoryInDB = await _categoryRepository.GetByIdAsync(category.Id);
            if (categoryInDB is null)
                throw new CategoryNotFoundException();

            
            Category? slugExists = await _categoryRepository.GetBySlugAsync(category.Slug);
            if (slugExists is not null && slugExists.Id != category.Id)
                throw new CategoryAlreadyExistsException(category.Slug);

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                throw new CategoryNotFoundException();

            await _categoryRepository.DeleteAsync(id);
        }
    }
}