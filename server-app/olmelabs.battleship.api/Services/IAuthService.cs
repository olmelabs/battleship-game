using olmelabs.battleship.api.Models.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string email, string password);

        Task<User> FindUserAsync(string email);

        Task<string> GetEmailByRefreshTokenAsync(string refreshToken);
        
        Task AddRefreshTokenAsync(string refreshToken, string email);

        Task DeleteRefreshTokenAsync(string refreshToken);

        string BuildJwtToken(User user);

        string BuildRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
