using FinancePlus.Data;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FinancePlus.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
        }

        public async Task<User?> GetByUuidAsync(Guid uuid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Uuid == uuid && !u.IsDeleted);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password!);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }



        public async Task<User?> UpdateAsync(Guid uuid, User updatedUser)
        {
            var user = await GetByUuidAsync(uuid);
            if (user == null) return null;

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.IBAN = updatedUser.IBAN;
            user.Currency = updatedUser.Currency;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid uuid)
        {
            var user = await GetByUuidAsync(uuid);
            if (user == null) return false;

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir)
        {
            var query = _context.Users.Where(u => !u.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FullName.ToLower().Contains(search.ToLower()) ||
                    u.Email.ToLower().Contains(search.ToLower()));
            }

            bool descending = (sortDir?.ToLower() == "desc");
            query = sortBy?.ToLower() switch
            {
                "fullname" => descending ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName),
                "email" => descending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "createdat" => descending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                _ => query.OrderBy(u => u.FullName) 
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
