using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using olmelabs.battleship.api.Models.Entities;

namespace olmelabs.battleship.api.Repositories
{
    public class MongoDbStorage : IStorage
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbStorage(IConfiguration config)
        {
            var client = new MongoClient(config["MongoConnection:ConnectionString"]);
            if (client != null)
                _database = client.GetDatabase(config["MongoConnection:Database"]);
        }

        public async Task Prepare()
        {
            //TODO: remove this later on
            var sampleUser = new User { FirstName = "John", LastName = "Doe", Email = "user@domain.com", PasswordHash = "AQAAAAEAACcQAAAAEEI1y09DRnWeVEUmJfBSYoLkYp6Ps+yQZTdxGB3PKWzX/GNs/P8BxIyqGOc/VGIEDA==", IsEmailConfirmed = true };
            var user = await FindUserAsync(sampleUser.Email);
            if (user == null)
            {
                user = await RegisterUserAsync(sampleUser);
            }

            var keysUser = Builders<User>.IndexKeys.Ascending("Email");
            CreateIndexModel<User> userIndex = new CreateIndexModel<User>(keysUser);
            await Users.Indexes.CreateOneAsync(userIndex);

            var keysGame = Builders<GameState>.IndexKeys.Ascending("GameId");
            CreateIndexModel<GameState> gameIndex = new CreateIndexModel<GameState>(keysGame);
            await Games.Indexes.CreateOneAsync(gameIndex);

            var statistics = await GetClientStatisticsAsync();
            if (statistics == null)
            {
                await Statistics.InsertOneAsync(ClientStatistics.CreateNew());
            }
        }

        public virtual IMongoCollection<User> Users => _database.GetCollection<User>("user");

        public virtual IMongoCollection<GameState> Games => _database.GetCollection<GameState>("game");

        public virtual IMongoCollection<RefreshToken> RefreshTokens => _database.GetCollection<RefreshToken>("refresh_token");

        public virtual IMongoCollection<ClientStatistics> Statistics => _database.GetCollection<ClientStatistics>("statistics");

        public virtual IMongoCollection<KeyValuePair> ResetPasswordCodes => _database.GetCollection<KeyValuePair>("reset_password_code");

        public virtual IMongoCollection<KeyValuePair> ConfirmEmailCodes => _database.GetCollection<KeyValuePair>("confirm_email_code");

        #region Singleplayer
        public async Task<GameState> AddGameAsync(GameState game)
        {
            await Games.InsertOneAsync(game);
            return game;
        }

        public async Task<GameState> FindActiveGameAsync(string gameId)
        {
            var game = await Games.Find(g => g.GameId == gameId && g.DateEnd == null).FirstOrDefaultAsync();
            return game;
        }

        public IQueryable<GameState> SelectGames(Func<GameState, bool> predicate)
        {
            var query = Games.AsQueryable().Where(predicate).AsQueryable();
            return query;
        }

        public async Task<GameState> UpdateGameAsync(GameState game)
        {
            var filter = Builders<GameState>.Filter.Where(g => g.GameId == game.GameId);
            await Games.ReplaceOneAsync(filter, game);
            return game; 
        }
        #endregion

        #region P2P (multiplayer)
        public async Task<PeerToPeerSessionState> FindP2PSessionAsync(string code)
        {
            //TODO: Implement
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public async Task<PeerToPeerSessionState> AddP2PSessionAsync(PeerToPeerSessionState p2pSession)
        {
            //TODO: Implement
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public async Task<PeerToPeerSessionState> UpdateP2PSessionAsync(PeerToPeerSessionState p2pSession)
        {
            //TODO: Implement
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public Task<PeerToPeerGameState> AddP2PGameAsync(PeerToPeerGameState game)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public Task<PeerToPeerGameState> UpdateP2PGameAsync(PeerToPeerGameState game)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public Task<PeerToPeerGameState> FindActiveP2PGameAsync(string gameId)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion

        #region Account
        public async Task<User> FindUserAsync(string email)
        {
            var user = await Users.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var filter = Builders<User>.Filter.Where(u => u.Email.ToLower() == user.Email.ToLower());
            await Users.ReplaceOneAsync(filter, user);
            return user;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            await Users.InsertOneAsync(user);
            return user;
        }

        public async Task<RefreshToken> GetEmailByRefreshTokenAsync(string refreshToken)
        {
            var email = await RefreshTokens.Find(t => t.Token == refreshToken).FirstOrDefaultAsync();
            return email;
        }

        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            await RefreshTokens.InsertOneAsync(token);
        }

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            await RefreshTokens.DeleteOneAsync(t => t.Token == refreshToken);
        }

        public async Task<ClientStatistics> GetClientStatisticsAsync()
        {
            return await Statistics.Find(_ => true).SingleOrDefaultAsync();
        }

        public async Task UpdateClientStatisticsAsync(ClientStatistics statistics)
        {
            await Statistics.FindOneAndReplaceAsync(_ => true, statistics);
        }

        public async Task<string> GetEmailByResetPasswordCodeAsync(string code)
        {
            var kvp = await ResetPasswordCodes.Find(t => t.Key == code).FirstOrDefaultAsync();
            return kvp?.Value;
        }

        public async Task AddResetPasswordCodeAsync(string code, string email)
        {
            await ResetPasswordCodes.InsertOneAsync(new KeyValuePair { Key = code, Value = email});
        }

        public async Task DeleteResetPasswordCodeAsync(string code)
        {
            await ResetPasswordCodes.DeleteOneAsync(t => t.Key == code);
        }

        public async Task<string> GetEmailByConfirmationCodeAsync(string code)
        {
            var kvp = await ConfirmEmailCodes.Find(t => t.Key == code).FirstOrDefaultAsync();
            return kvp?.Value;
        }

        public async Task AddEmailConfirmationCodeCodeAsync(string code, string email)
        {
            await ConfirmEmailCodes.InsertOneAsync(new KeyValuePair { Key = code, Value = email });
        }

        public async Task DeleteEmailConfirmationCodeAsync(string code)
        {
            await ConfirmEmailCodes.DeleteOneAsync(t => t.Key == code);
        }
        #endregion
 
    }
}
