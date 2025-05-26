using FinancePlus.Data;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancePlus.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Transaction>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir)
        {
            var query = _context.Transactions.Where(t => !t.IsDeleted);

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

            return new PagedResult<Transaction>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<Transaction>> GetPagedByUserAsync(Guid userUuid, int page, int pageSize, string? search, string? sortBy, string? sortDir, Guid? categoryUuid = null)
        {
            var query = _context.Transactions
                .Where(t => t.UserUuid == userUuid && !t.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.Description.ToLower().Contains(search.ToLower()));

            if (categoryUuid.HasValue)
                query = query.Where(t => t.CategoryUuid == categoryUuid.Value);

            bool desc = sortDir?.ToLower() == "desc";
            query = sortBy?.ToLower() switch
            {
                "amount" => desc ? query.OrderByDescending(t => t.Amount) : query.OrderBy(t => t.Amount),
                "createdat" => desc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Transaction>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Transaction?> GetByUuidAsync(Guid userUuid, Guid uuid)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.Uuid == uuid && t.UserUuid == userUuid && !t.IsDeleted);
        }

        public async Task<Transaction> CreateAsync(Guid userUuid, Transaction transaction)
        {
            transaction.UserUuid = userUuid;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(Guid userUuid, Guid uuid, Transaction updated)
        {
            var transaction = await GetByUuidAsync(userUuid, uuid);
            if (transaction == null) return null;

            transaction.Amount = updated.Amount;
            transaction.Description = updated.Description;
            transaction.CategoryUuid = updated.CategoryUuid;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteAsync(Guid userUuid, Guid uuid)
        {
            var transaction = await GetByUuidAsync(userUuid, uuid);
            if (transaction == null) return false;

            transaction.IsDeleted = true;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
