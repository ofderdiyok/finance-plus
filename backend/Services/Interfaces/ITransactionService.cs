using FinancePlus.Models;

namespace FinancePlus.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedResult<Transaction>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir);
        Task<PagedResult<Transaction>> GetPagedByUserAsync(Guid userUuid, int page, int pageSize, string? search, string? sortBy, string? sortDir, Guid? categoryUuid);
        Task<Transaction?> GetByUuidAsync(Guid userUuid, Guid uuid);
        Task<Transaction> CreateAsync(Guid userUuid, Transaction transaction);
        Task<Transaction?> UpdateAsync(Guid userUuid, Guid uuid, Transaction updated);
        Task<bool> DeleteAsync(Guid userUuid, Guid uuid);
    }
}