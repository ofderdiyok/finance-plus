using FinancePlus.Models;

namespace FinancePlus.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResult<Category>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir);
        Task<Category?> GetByUuidAsync(Guid uuid);
        Task<Category> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Guid uuid, Category updated);
        Task<bool> DeleteAsync(Guid uuid);
    }
}