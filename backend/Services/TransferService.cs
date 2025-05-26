using FinancePlus.Data;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancePlus.Services
{
    public class TransferService : ITransferService
    {
        private readonly AppDbContext _context;

        public TransferService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Transfer>> GetPagedBySenderAsync(Guid senderUuid, int page, int pageSize, string? search, string? sortBy, string? sortDir)
        {
            var query = _context.Transfers.Where(t => t.senderUuid == senderUuid && !t.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Description.ToLower().Contains(search.ToLower()));
            }

            bool desc = sortDir?.ToLower() == "desc";
            query = sortBy?.ToLower() switch
            {
                "amount" => desc ? query.OrderByDescending(t => t.Amount) : query.OrderBy(t => t.Amount),
                "createdat" => desc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Transfer>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Transfer?> GetByUuidAsync(Guid uuid)
        {
            return await _context.Transfers.FirstOrDefaultAsync(t => t.Uuid == uuid && !t.IsDeleted);
        }

        public async Task<Transfer> CreateAsync(Guid senderUuid, Transfer transfer)
        {
            transfer.senderUuid = senderUuid;
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();
            return transfer;
        }

        public async Task<bool> DeleteAsync(Guid senderUuid, Guid uuid)
        {
            var transfer = await _context.Transfers.FirstOrDefaultAsync(t => t.Uuid == uuid && t.senderUuid == senderUuid && !t.IsDeleted);
            if (transfer == null) return false;

            transfer.IsDeleted = true;
            transfer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}