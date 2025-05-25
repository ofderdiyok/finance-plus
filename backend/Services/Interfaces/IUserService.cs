using FinancePlus.Models;


namespace FinancePlus.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUuidAsync(Guid uuid);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(Guid uuid, User updatedUser);
        Task<bool> DeleteAsync(Guid uuid);
        Task<User?> AuthenticateAsync(string email, string password);
        Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir);

    }
}
