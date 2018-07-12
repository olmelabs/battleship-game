using olmelabs.battleship.api.Models.Entities;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface IAccountService
    {
        Task<User> FindUserAsync(string email);

        Task<User> RegisterUserAsync(User user);
    }
}
