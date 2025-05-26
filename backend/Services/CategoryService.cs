using FinancePlus.Data;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancePlus.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Category>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir)
        {
            var query = _context.Categories.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
            }

            bool desc = sortDir?.ToLower() == "desc";
            query = sortBy?.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "createdat" => desc ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                _ => query.OrderBy(c => c.Name)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Category>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Category?> GetByUuidAsync(Guid uuid)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Uuid == uuid && !c.IsDeleted);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(Guid uuid, Category updated)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Uuid == uuid && !c.IsDeleted);
            if (category == null) return null;

            category.Name = updated.Name;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(Guid uuid)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Uuid == uuid && !c.IsDeleted);
            if (category == null) return false;

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}