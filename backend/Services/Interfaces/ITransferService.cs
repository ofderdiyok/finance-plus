using FinancePlus.Models;

namespace FinancePlus.Services.Interfaces
{
    public interface ITransferService
    {
        Task<PagedResult<Transfer>> GetPagedBySenderAsync(Guid senderUuid, int page, int pageSize, string? search, string? sortBy, string? sortDir);
        Task<Transfer?> GetByUuidAsync(Guid uuid);
        Task<Transfer> CreateAsync(Guid senderUuid, Transfer transfer);
        Task SaveAsync();
        Task<bool> DeleteAsync(Guid senderUuid, Guid uuid);
    }
}