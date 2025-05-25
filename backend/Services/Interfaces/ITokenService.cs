using FinancePlus.Models;

namespace FinancePlus.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
