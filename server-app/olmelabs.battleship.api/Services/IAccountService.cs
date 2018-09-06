using olmelabs.battleship.api.Models.Entities;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface IAccountService
    {
        Task<User> FindUserAsync(string email);

        Task<User> RegisterUserAsync(User user);

        Task<string> RegisterResetPasswordCodeAsync(User user);

        Task<User> ResetPasswordAsync(User user);

        Task<User> GetUserByResetPasswordTokenAsync(string code);

        Task<string> RegisterEmailConfirmationCodeAsync(User user);

        Task<User> GetUserByEmailConfirmationCodeAsync(string code);

        Task<User> ConfirmEmailAsync(User user);
    }
}
