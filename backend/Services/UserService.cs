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
    }
}
