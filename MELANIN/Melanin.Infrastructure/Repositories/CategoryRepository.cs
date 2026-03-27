using Melanin.Application.Interfaces.Repositories;
using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Melanin.Infrastructure.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MelaninDbContext _dbContext;

        public CategoryRepository(MelaninDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<Category?> GetBySlugAsync(string slug)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category? category = await _dbContext.Categories.FindAsync(id);
            _dbContext.Categories.Remove(category!);
            await _dbContext.SaveChangesAsync();
        }
    }
}