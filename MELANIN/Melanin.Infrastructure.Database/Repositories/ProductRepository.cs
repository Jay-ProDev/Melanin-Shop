using Melanin.Application.Interfaces.Repositories;
using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Melanin.Infrastructure.Database.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MelaninDbContext _dbContext;

        public ProductRepository(MelaninDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await _dbContext.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbContext.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            return await _dbContext.Products
                .Where(p => p.IsActive &&
                       (p.Name.Contains(keyword) ||
                        p.Description.Contains(keyword)))
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            // Recharger avec la Category incluse
            return await _dbContext.Products
                .Include(p => p.Category)
                .FirstAsync(p => p.Id == product.Id);
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Product? product = await _dbContext.Products.FindAsync(id);
            _dbContext.Products.Remove(product!);
            await _dbContext.SaveChangesAsync();
        }
    }
}