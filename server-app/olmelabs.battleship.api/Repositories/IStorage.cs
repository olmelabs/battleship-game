using olmelabs.battleship.api.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Repositories
{
    public interface IStorage
    {
        Task Prepare();

        Task<GameState> FindActiveGameAsync(string gameId);

        IQueryable<GameState> SelectGames(Func<GameState, bool> predicate);

        Task<GameState> AddGameAsync(GameState game);

        Task<GameState> UpdateGameAsync(GameState game);
      
        Task<User> FindUserAsync(string email);

        Task<User> RegisterUserAsync(User user);

        Task<RefreshToken> GetEmailByRefreshTokenAsync(string refreshToken);

        Task AddRefreshTokenAsync(RefreshToken token);

        Task DeleteRefreshTokenAsync(string refreshToken);

        Task<ClientStatistics> GetClientStatisticsAsync();

        Task UpdateClientStatisticsAsync(ClientStatistics statistics);
    }
}
