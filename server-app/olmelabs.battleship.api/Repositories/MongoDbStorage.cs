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
        }

        public virtual IMongoCollection<User> Users => _database.GetCollection<User>("user");

        public virtual IMongoCollection<GameState> Games => _database.GetCollection<GameState>("game");

        public virtual IMongoCollection<RefreshToken> RefreshTokens => _database.GetCollection<RefreshToken>("refresh_token");


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

        public async Task<User> FindUserAsync(string email)
        {
            var user = await Users.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            await Users.InsertOneAsync(user);
            return user;
        }

        public async Task<RefreshToken> GetEmailByRefreshTokenAsync(string refreshToken)
        {
            var token = await RefreshTokens.Find(t => t.Token == refreshToken).FirstOrDefaultAsync();
            return token;
        }

        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            await RefreshTokens.InsertOneAsync(token);
        }

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            await RefreshTokens.DeleteOneAsync(t => t.Token == refreshToken);
        }
    }
}
