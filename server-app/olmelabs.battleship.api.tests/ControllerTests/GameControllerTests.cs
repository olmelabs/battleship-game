using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Controllers;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class GameControllerTests : BaseControllerTests
    {

        [TestMethod]
        public void ValidateBoardTest()
        {
            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.ValidateClientBoard(It.IsAny<List<ClientShipDto>>()))
                .Returns(true);

            var controller = new GameController(gameService.Object, _mapper);
            var output = controller.ValidateBoard(new ClientShipDto[] {});

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            bool res = (bool)dto.GetType().GetProperty("result").GetValue(dto, null);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void GenerateBoardTest()
        {
            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.GenerateClientBoard())
                .Returns(new BoardInfo());

            var controller = new GameController(gameService.Object, _mapper);
            var output = controller.GenerateBoard();

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(BoardInfoDto));
        }

        [TestMethod]
        public async Task StartNewGameTest()
        {
            var gameLogic = new Mock<IGameLogic>();
            gameLogic.Setup(x => x.GenerateBoad())
                .Returns(new BoardInfo());

            var game = GameState.CreateNew(gameLogic.Object);
            game.ConnectionId = "connectionId";

            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.StartNewGameAsync(It.IsAny<string>()))
                .ReturnsAsync(game);

            var controller = new GameController(gameService.Object, _mapper);
            var output = await controller.StartNewGame("connectionid");

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(NewGameDto));

            NewGameDto dto = ((NewGameDto)((OkObjectResult)output).Value);
            Assert.AreEqual(game.GameId, dto.GameId);

            //gameService.VerifyAll();
        }

        [TestMethod]
        public async Task StopGameTest_Ok()
        {
            var gameLogic = new Mock<IGameLogic>();
            gameLogic.Setup(x => x.GenerateBoad())
                .Returns(new BoardInfo());

            GameState game = GameState.CreateNew(gameLogic.Object);

            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.StopGameAsync(It.IsAny<string>(), It.IsAny<List<ShipInfo>>()))
                .ReturnsAsync(game);

            var controller = new GameController(gameService.Object, _mapper);
            var output = await controller.StopGame(new GameOverClientDto() { GameId = "dummy"});

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(GameOverDto));
        }

        [TestMethod]
        public async Task StopGameTest_BadRequest()
        {
            GameState game = null;

            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.StopGameAsync(It.IsAny<string>(), It.IsAny<List<ShipInfo>>()))
                .ReturnsAsync(game);

            var controller = new GameController(gameService.Object, _mapper);
            var output = await controller.StopGame(new GameOverClientDto() { GameId = "dummy" });

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task FireCannon_Ok()
        {
            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.FireCannon(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new FireResult() { IsHit = true });

            var inputDto = new FireCannonDto { CellId = 1, GameId = "" };

            var controller = new GameController(gameService.Object, _mapper);
            var output = await controller.FireCannon(inputDto);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(FireCannonResultDto));
        }

        [TestMethod]
        public async Task FireCannon_BadRequest()
        {
            FireResult res  = null;
            var gameService = new Mock<IGameService>();
            gameService.Setup(x => x.FireCannon(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(res);

            var inputDto = new FireCannonDto { CellId = 1, GameId = "" };

            var controller = new GameController(gameService.Object, _mapper);
            var output = await controller.FireCannon(inputDto);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }
    }
}
