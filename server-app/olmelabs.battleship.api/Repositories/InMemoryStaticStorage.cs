using olmelabs.battleship.api.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Repositories
{
    public class InMemoryStaticStorage : IStorage
    {
        //game-id - game
        private static ConcurrentDictionary<string, GameState> _games;
        //emai - user
        private static ConcurrentDictionary<string, User> _users;
        //token - email
        private static ConcurrentDictionary<string, RefreshToken> _refreshTokens;
        //code - email
        private static ConcurrentDictionary<string, string> _resetPasswordCodes;
        //code - email
        private static ConcurrentDictionary<string, string> _confirmEmailCodes;
        //dummy - ClientStatistics - only one record for now
        private static ConcurrentDictionary<string, ClientStatistics> _clientStatistics;
        //p2p code - p2p session
        private static ConcurrentDictionary<string, PeerToPeerSessionState> _p2pSessions;
        //game-id - game
        private static ConcurrentDictionary<string, PeerToPeerGameState> _p2pGames;
        

        static InMemoryStaticStorage()
        {
            _games = new ConcurrentDictionary<string, GameState>();
            _users = new ConcurrentDictionary<string, User>();
            _refreshTokens = new ConcurrentDictionary<string, RefreshToken>();
            _clientStatistics = new ConcurrentDictionary<string, ClientStatistics>();
            _resetPasswordCodes = new ConcurrentDictionary<string, string>();
            _confirmEmailCodes = new ConcurrentDictionary<string, string>();
            _p2pSessions = new ConcurrentDictionary<string, PeerToPeerSessionState>();
            _p2pGames = new ConcurrentDictionary<string, PeerToPeerGameState>();
        }

        public InMemoryStaticStorage()
        {
            HoursToLive = 4;
        }

        public int PeerToPeerSessionsCount => _p2pSessions.Count;

        public int PeerToPeerGamesCount => _p2pGames.Count;

        public int GamesCount => _games.Count;

        public int HoursToLive { get; set; }

        public Task Prepare()
        {
            var user = new User { FirstName = "John", LastName = "Doe", Email = "user@domain.com", PasswordHash = "AQAAAAEAACcQAAAAEEI1y09DRnWeVEUmJfBSYoLkYp6Ps+yQZTdxGB3PKWzX/GNs/P8BxIyqGOc/VGIEDA==", IsEmailConfirmed = true };
            _users.AddOrUpdate(user.Email, user, (key, val) => user);

            ClientStatistics stat = ClientStatistics.CreateNew();
            _clientStatistics.GetOrAdd(typeof(ClientStatistics).ToString(), stat);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Cleanup old objects from memory storage
        /// </summary>
        /// <returns></returns>
        public Task Cleanup()
        {
            DateTime now = DateTime.Now;

            if (_games.Count > 500)
            {
                foreach (string key in _games.Keys)
                {
                    GameState g = _games[key];
                    TimeSpan ts = now - g.LastUpdated;
                    if (ts.TotalHours >= HoursToLive)
                        _games.TryRemove(key, out _);
                }
            }

            if (_p2pGames.Count > 500)
            {
                foreach (string key in _p2pGames.Keys)
                {
                    PeerToPeerGameState g = _p2pGames[key];
                    TimeSpan ts = now - g.LastUpdated;
                    if (ts.TotalHours >= HoursToLive)
                        _p2pGames.TryRemove(key, out _);
                }
            }

            if (_p2pSessions.Count > 500)
            {
                foreach (string key in _p2pSessions.Keys)
                {
                    PeerToPeerSessionState g = _p2pSessions[key];
                    TimeSpan ts = now - g.LastUpdated;
                    if (ts.TotalHours >= HoursToLive)
                        _p2pSessions.TryRemove(key, out _);
                }
            }

            return Task.FromResult(0);
        }

        #region Singleplayer
        public Task<GameState> FindActiveGameAsync(string gameId)
        {
            var game = _games.Values.FirstOrDefault(g => g.GameId == gameId && g.DateEnd == null);
            return Task.FromResult(game);
        }

        public IQueryable<GameState> SelectGames(Func<GameState, bool> predicate)
        {
            var query = _games.Values.Where(predicate).AsQueryable();
            return query;
        }

        public Task<GameState> FirstOrDefaultGameAsync(Func<GameState, bool> predicate)
        {
            GameState game = _games.Values.FirstOrDefault(predicate);
            return Task.FromResult(game);
        }

        public Task<GameState> AddGameAsync(GameState game)
        {
            game.LastUpdated = DateTime.Now;
            game = _games.GetOrAdd(game.GameId, game);
            return Task.FromResult(game);
        }

        public Task<GameState> UpdateGameAsync(GameState game)
        {
            game.LastUpdated = DateTime.Now;
            game = _games.AddOrUpdate(game.GameId, game, (key, val) => game);
            return Task.FromResult(game);
        }
        #endregion

        #region P2P (multiplayer)
       public Task<PeerToPeerSessionState> FindP2PSessionAsync(string code)
        {
            PeerToPeerSessionState p2pSession = _p2pSessions.FirstOrDefault(u => u.Key == code).Value;
            return Task.FromResult(p2pSession);
        }

        public Task<PeerToPeerSessionState> AddP2PSessionAsync(PeerToPeerSessionState p2pSession)
        {
            p2pSession.LastUpdated = DateTime.Now;
            p2pSession = _p2pSessions.GetOrAdd(p2pSession.Code, p2pSession);
            return Task.FromResult(p2pSession);
        }

        public Task<PeerToPeerSessionState> UpdateP2PSessionAsync(PeerToPeerSessionState p2pSession)
        {
            p2pSession.LastUpdated = DateTime.Now;
            p2pSession = _p2pSessions.AddOrUpdate(p2pSession.Code, p2pSession, (key, val) => p2pSession);
            return Task.FromResult(p2pSession);
        }

        public Task<PeerToPeerGameState> FindActiveP2PGameAsync(string gameId)
        {
            var game = _p2pGames.Values.FirstOrDefault(g => g.GameId == gameId && g.DateEnd == null);
            return Task.FromResult(game);
        }

        public Task<PeerToPeerGameState> AddP2PGameAsync(PeerToPeerGameState game)
        {
            game.LastUpdated = DateTime.Now;
            game = _p2pGames.GetOrAdd(game.GameId, game);
            return Task.FromResult(game);
        }

        public Task<PeerToPeerGameState> UpdateP2PGameAsync(PeerToPeerGameState game)
        {
            game.LastUpdated = DateTime.Now;
            game = _p2pGames.AddOrUpdate(game.GameId, game, (key, val) => game);
            return Task.FromResult(game);
        }
        #endregion

        #region Account
        public Task<User> FindUserAsync(string email)
        {
            User user = _users.FirstOrDefault(u => u.Key == email).Value;
            return Task.FromResult(user);
        }

        public Task<User> UpdateUserAsync(User user)
        {
            User oldUser = _users.FirstOrDefault(u => u.Key == user.Email).Value;
            var res = _users.TryUpdate(user.Email, user, oldUser);
            return Task.FromResult(user);
        }

        public Task<User> RegisterUserAsync(User user)
        {
            if (_users.ContainsKey(user.Email))
            {
                throw new ArgumentException("email already exists.");
            }
            _users.GetOrAdd(user.Email, user);
            return Task.FromResult(user);
        }

        public Task<RefreshToken> GetEmailByRefreshTokenAsync(string refreshToken)
        {
            RefreshToken rt = null;
            if (_refreshTokens.ContainsKey(refreshToken))
            {
                rt = _refreshTokens[refreshToken];
            }
            return Task.FromResult(rt);
        }

        public Task AddRefreshTokenAsync(RefreshToken rt)
        {
            _refreshTokens.AddOrUpdate(rt.Token, rt, (key, val) => rt);

            return Task.FromResult(0);
        }

        public Task DeleteRefreshTokenAsync(string refreshToken)
        {
            _refreshTokens.TryRemove(refreshToken, out _);
            return Task.FromResult(0);
        }

        public Task<ClientStatistics> GetClientStatisticsAsync()
        {
            return Task.FromResult(_clientStatistics.First().Value);
        }

        public Task UpdateClientStatisticsAsync(ClientStatistics statistics)
        {
            _clientStatistics.AddOrUpdate(typeof(ClientStatistics).ToString(), statistics, (key, value) => statistics);
            return Task.FromResult(0);
        }

        public Task<string> GetEmailByResetPasswordCodeAsync(string code)
        {
            string email = null;
            if (_resetPasswordCodes.ContainsKey(code))
            {
                email = _resetPasswordCodes[code];
            }
            return Task.FromResult(email);
        }

        public Task AddResetPasswordCodeAsync(string code, string email)
        {
            _resetPasswordCodes.AddOrUpdate(code, email, (key, val) => email);
            return Task.FromResult(0);
        }

        public Task DeleteResetPasswordCodeAsync(string code)
        {
            _resetPasswordCodes.TryRemove(code, out _);
            return Task.FromResult(0);
        }

        public Task<string> GetEmailByConfirmationCodeAsync(string code)
        {
            string email = null;
            if (_confirmEmailCodes.ContainsKey(code))
            {
                email = _confirmEmailCodes[code];
            }
            return Task.FromResult(email);
        }

        public Task AddEmailConfirmationCodeCodeAsync(string code, string email)
        {
            _confirmEmailCodes.AddOrUpdate(code, email, (key, val) => email);
            return Task.FromResult(0);
        }

        public Task DeleteEmailConfirmationCodeAsync(string code)
        {
            _confirmEmailCodes.TryRemove(code, out _);
            return Task.FromResult(0);
        }
        #endregion
    }
}
