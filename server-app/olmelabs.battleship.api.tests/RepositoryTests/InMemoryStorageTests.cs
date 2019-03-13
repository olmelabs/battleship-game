using Microsoft.VisualStudio.TestTools.UnitTesting;
using olmelabs.battleship.api.Logic;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.RepositoryTests
{
    [TestClass]
    public class InMemoryStorageTests
    {
        [TestMethod]
        public async Task CleanupTest()
        {
            InMemoryStaticStorage s = new InMemoryStaticStorage
            {
                HoursToLive = 0
            };


            //clean results of older runs
            await s.Cleanup();

            for (int i = 0; i < 1000; i++)
            {
                var game = PeerToPeerGameState.CreateNew();
                await s.AddP2PGameAsync(game);
            }

            for (int i = 0; i < 1000; i++)
            {
                var session = new PeerToPeerSessionState()
                {
                    Code = i.ToString(),
                };
                await s.AddP2PSessionAsync(session);
            }

            for (int i = 0; i < 1000; i++)
            {
                var game = new GameState
                {
                    GameId = Guid.NewGuid().ToString()
                };
                await s.AddGameAsync(game);
            }

            await s.Cleanup();

            Assert.AreEqual(0, s.PeerToPeerGamesCount);
            Assert.AreEqual(0, s.PeerToPeerSessionsCount);
            Assert.AreEqual(0, s.GamesCount);
        }
    }
}
