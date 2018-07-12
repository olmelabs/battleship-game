using Microsoft.VisualStudio.TestTools.UnitTesting;
using olmelabs.battleship.api.Logic;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests
{
    [TestClass]
    public class GameServiceTests
    {
        [TestMethod]
        public async Task StartNewGameTest()
        {
            GameState dbObj;
            GameState game;
            IGameLogic gameLogic = new GameLogic();
            IStorage storage = new InMemoryStaticStorage();

            GameService svc = new GameService(storage, gameLogic);
            game = await svc.StartNewGameAsync("connectionid");

            dbObj = await storage.FindActiveGameAsync(game.GameId);

            Assert.IsNotNull(game);
            Assert.IsTrue(game.ServerBoard.Board.Length == 100);
            Assert.IsTrue(game.ClientBoard.Board.Length == 100);
            Assert.IsNull(game.DateEnd);
            Assert.IsNotNull(dbObj);
        }

        [TestMethod]
        public async Task StopGameTest()
        {
            GameState game;
            IGameLogic gameLogic = new GameLogic();
            IStorage storage = new InMemoryStaticStorage();
            List<ShipInfo> ships = new List<ShipInfo>{
                new ShipInfo(false, new []{3,4,5,6}),
                new ShipInfo(false, new []{26,27,28}),
                new ShipInfo(false, new []{30,31,32}),
            };

            GameService svc = new GameService(storage, gameLogic);
            game = await svc.StartNewGameAsync("connectionid");
            game = await svc.StopGameAsync(game.GameId, ships);


            Assert.IsNotNull(game);
            Assert.IsNotNull(game.DateEnd);
            Assert.IsNotNull(game.ClientBoard.Ships);
            Assert.AreEqual(3, game.ClientBoard.Ships.Count);
        }

        [TestMethod]
        public async Task FireCannonTest()
        {
            GameState game;
            FireResult res1, res2, res3, res4, res5;
            IGameLogic gameLogic = new GameLogic();
            IStorage storage = new InMemoryStaticStorage();

            GameService svc = new GameService(storage, gameLogic);

            game = await svc.StartNewGameAsync("connectionid");

            //find first ship with 4 cells
            var ship4CellsInfo = game.ServerBoard.Ships[0];
            //find index of fisrt element where cell value is 0
            int emptyIdx = game.ServerBoard.Board.Select((number, index) => new { index, i = number }).Where(n => n.i == 0).FirstOrDefault().index;

            res1 = await svc.FireCannon(game.GameId, ship4CellsInfo.Cells[0]);
            res2 = await svc.FireCannon(game.GameId, ship4CellsInfo.Cells[1]);
            res3 = await svc.FireCannon(game.GameId, ship4CellsInfo.Cells[2]);
            res4 = await svc.FireCannon(game.GameId, ship4CellsInfo.Cells[3]);
            res5 = await svc.FireCannon(game.GameId, emptyIdx);

            Assert.IsTrue(res1.IsHit);
            Assert.IsTrue(game.IsAwaitingServerTurn);
            Assert.IsNull(res1.ShipDestroyed);

            Assert.IsTrue(res2.IsHit);
            Assert.IsNull(res2.ShipDestroyed);

            Assert.IsTrue(res3.IsHit);
            Assert.IsNull(res3.ShipDestroyed);

            Assert.IsTrue(res4.IsHit);
            Assert.IsNotNull(res4.ShipDestroyed);
            Assert.IsFalse(res4.IsGameOver);

            Assert.IsFalse(res5.IsHit);
            Assert.IsNull(res5.ShipDestroyed);
        }
    }
}
