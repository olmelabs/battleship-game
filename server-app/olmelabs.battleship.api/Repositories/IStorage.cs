using olmelabs.battleship.api.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Repositories
{
    //TODO: refactor interface and impl to smaller pieces. remember - Interface segregation
    public interface IStorage
    {
        Task Prepare();

        #region Singleplayer
        Task<GameState> FindActiveGameAsync(string gameId);

        IQueryable<GameState> SelectGames(Func<GameState, bool> predicate);

        Task<GameState> AddGameAsync(GameState game);

        Task<GameState> UpdateGameAsync(GameState game);
        #endregion

        #region P2P (multiplayer)
        Task<PeerToPeerSessionState> FindP2PSessionAsync(string code);

        Task<PeerToPeerSessionState> AddP2PSessionAsync(PeerToPeerSessionState p2pSession);

        Task<PeerToPeerSessionState> UpdateP2PSessionAsync(PeerToPeerSessionState p2pSession);

        Task<PeerToPeerGameState> FindActiveP2PGameAsync(string gameId);

        Task<PeerToPeerGameState> AddP2PGameAsync(PeerToPeerGameState game);

        Task<PeerToPeerGameState> UpdateP2PGameAsync(PeerToPeerGameState game);
        #endregion

        #region Account
        Task<User> FindUserAsync(string email);

        Task<User> UpdateUserAsync(User user);

        Task<User> RegisterUserAsync(User user);

        Task<RefreshToken> GetEmailByRefreshTokenAsync(string refreshToken);

        Task AddRefreshTokenAsync(RefreshToken token);

        Task DeleteRefreshTokenAsync(string refreshToken);

        Task<ClientStatistics> GetClientStatisticsAsync();

        Task UpdateClientStatisticsAsync(ClientStatistics statistics);

        Task<string> GetEmailByResetPasswordCodeAsync(string code);

        Task AddResetPasswordCodeAsync(string code, string email);

        Task DeleteResetPasswordCodeAsync(string code);

        Task<string> GetEmailByConfirmationCodeAsync(string code);

        Task AddEmailConfirmationCodeCodeAsync(string code, string email);

        Task DeleteEmailConfirmationCodeAsync(string code);
        #endregion
    }
}
