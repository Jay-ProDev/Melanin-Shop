using Melanin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
