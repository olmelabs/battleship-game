﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Controllers;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    //In tests below it is not possble to verify SignalR SendAsync was called as it is static extension method. 
    //Verfying access to property only for now

    [TestClass]
    public class PeerToPeerGameControllerTests : BaseControllerTests
    {
        #region StartSession tests
        [TestMethod]
        public async Task StartNewGameTest_Ok()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();
            p2pSvc.Setup(x => x.StartNewSessionAsync(It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345" });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);
            var output = await controller.StartSession("connectionid");

            p2pSvc.Verify(p => p.StartNewSessionAsync(It.IsAny<string>()), Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(P2PStartSessionDto));
        }

        [TestMethod]
        public async Task StartNewGameTest_BadRequest()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartSession(null);

            p2pSvc.Verify(p => p.StartNewSessionAsync(It.IsAny<string>()), Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }
        #endregion

        #region JoinSession tests
        [TestMethod]
        public async Task JoinSessionTest_Ok()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto() { Code = "12345", ConnectionId = "123" };

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", FriendConnectionId = "c2" });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.JoinSession(dto);

            p2pSvc.Verify(p => p.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _signalRHub.VerifyGet(p => p.Clients, Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task JoinSessionTest_BadRequest()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.JoinSession(new P2PGameKeyDto() { Code = "12345" });

            p2pSvc.Verify(p => p.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task JoinSessionTest_BadRequest_2()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((PeerToPeerSessionState)null);

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.JoinSession(new P2PGameKeyDto() { Code = "12345", ConnectionId = "123" });

            p2pSvc.Verify(p => p.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task JoinSessionTest_BadRequest_3()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState { Code = "12345", HostConnectionId = "123" });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.JoinSession(new P2PGameKeyDto() { Code = "12345", ConnectionId = "123" });

            p2pSvc.Verify(p => p.JoinSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }
        #endregion

        #region StartNewGame tests
        [TestMethod]
        public async Task StartNewGame_FirstConnected_Ok()
        {
            P2PNewGametDto dto = new P2PNewGametDto() { Code = "12345", ConnectionId = "c2", Ships = new ClientShipDto[10] };

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.AddPeerToSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ShipInfo>>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", GameStartedCount = 1 });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(dto);

            p2pSvc.Verify(p => p.StartNewGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            _signalRHub.VerifyGet(p => p.Clients, Times.Exactly(2));

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task StartNewGame_BothConnected_Ok()
        {
            P2PNewGametDto dto = new P2PNewGametDto() { Code = "12345", ConnectionId = "c2", Ships = new ClientShipDto[10] };

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.AddPeerToSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ShipInfo>>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", FriendConnectionId = "c2", GameStartedCount = 2 });

            p2pSvc.Setup(x => x.StartNewGameAsync(It.IsAny<PeerToPeerSessionState>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", FriendConnectionId = "c2", GameStartedCount = 2, GameId = "7890" });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(dto);

            p2pSvc.Verify(p => p.StartNewGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            _signalRHub.VerifyGet(p => p.Clients, Times.Exactly(2));

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task StartNewGame_BadRequest_1()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(new P2PNewGametDto { Code = "12345", ConnectionId = null, Ships = null });

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task StartNewGame_BadRequest_2()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(new P2PNewGametDto { Code = null, ConnectionId = "c1", Ships = null });

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task StartNewGame_BadRequest_3()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(new P2PNewGametDto { Code = "12345", ConnectionId = "c1", Ships = null });

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task StartNewGame_BadRequest_4()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.AddPeerToSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ShipInfo>>()))
                .ReturnsAsync((PeerToPeerSessionState)null);

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(new P2PNewGametDto { Code = "12345", ConnectionId = "c1", Ships = new ClientShipDto[10] });

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        public async Task StartNewGame_BadRequest_5()
        {
            //checks same connection joins twice
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.AddPeerToSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ShipInfo>>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", GameStartedCount = 1 });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.StartNewGame(new P2PNewGametDto { Code = "12345", ConnectionId = "c1", Ships = new ClientShipDto[10] });

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }
        #endregion

        #region FireCannon tests
        [TestMethod]
        public async Task FireCannon_Ok()
        {
            P2PFireCannonDto dto = new P2PFireCannonDto() { Code = "12345", ConnectionId = "c2", CellId = 1 };

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannon(dto);

            _signalRHub.VerifyGet(p => p.Clients, Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
        }

        public async Task FireCannon_BadRequest_1()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannon(new P2PFireCannonDto() { Code = null, ConnectionId = "c2" });

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        public async Task FireCannon_BadRequest_2()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannon(new P2PFireCannonDto() { Code = "12345", ConnectionId = null });

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        public async Task FireCannon_BadRequest_3()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((PeerToPeerSessionState)null);

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannon(new P2PFireCannonDto() { Code = "12345", ConnectionId = "c1" });

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        #endregion

        #region FireCannonProcessResult tests
        [TestMethod]
        public async Task FireCannonProcessResult_Ok()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannonProcessResult(new P2PFireCannonCallbackDto { Code = "12345", ConnectionId = "c1" });

            p2pSvc.Verify(p => p.StopGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            _signalRHub.VerifyGet(p => p.Clients, Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task FireCannonProcessResult_GameOver_Ok()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", FriendShips= new List<ShipInfo>(), HostShips = new List<ShipInfo>() });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannonProcessResult(new P2PFireCannonCallbackDto { Code = "12345", ConnectionId = "c1", IsGameOver = true });

            p2pSvc.Verify(p => p.StopGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            _signalRHub.VerifyGet(p => p.Clients, Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(GameOverDto));
        }

        [TestMethod]
        public async Task FireCannonProcessResult_BadRequest_1()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannonProcessResult(new P2PFireCannonCallbackDto { Code = null, ConnectionId = "c1" });

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task FireCannonProcessResult_BadRequest_2()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannonProcessResult(new P2PFireCannonCallbackDto { Code = "12345", ConnectionId = null });

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task FireCannonProcessResult_BadRequest_3()
        {
            P2PGameKeyDto dto = new P2PGameKeyDto();

            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.AddPeerToSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ShipInfo>>()))
                .ReturnsAsync((PeerToPeerSessionState)null);

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.FireCannonProcessResult(new P2PFireCannonCallbackDto { Code = "12345", ConnectionId = "c1" });

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }
        #endregion

        #region RestartGame tests
        [TestMethod]
        public async Task RestartGame_Ok()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();
            var session = new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", };

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(session);

            p2pSvc.Setup(x => x.RestartGameAsync(It.IsAny<PeerToPeerSessionState>()))
                .ReturnsAsync(session);
            
            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.RestartGame(new P2PGameKeyDto { Code = "12345", ConnectionId = "c1" });

            p2pSvc.Verify(p => p.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            p2pSvc.Verify(p => p.RestartGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);
            
            _signalRHub.VerifyGet(p => p.Clients, Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task RestartGame_BadRequest_1()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.RestartGame(new P2PGameKeyDto { Code = null, ConnectionId = "c1" });

            p2pSvc.Verify(p => p.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            p2pSvc.Verify(p => p.RestartGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task RestartGame_BadRequest_2()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.RestartGame(new P2PGameKeyDto { Code = "12345", ConnectionId = null });

            p2pSvc.Verify(p => p.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            p2pSvc.Verify(p => p.RestartGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task RestartGame_BadRequest_3()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((PeerToPeerSessionState)null);

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.RestartGame(new P2PGameKeyDto { Code = "12345", ConnectionId = "c1" });

            p2pSvc.Verify(p => p.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            p2pSvc.Verify(p => p.RestartGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

        public async Task RestartGame_BadRequest_4()
        {
            //HostConnectionId should be the same as already in session, otherwise - bad request
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            p2pSvc.Setup(x => x.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345", HostConnectionId = "c1", });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object, _logger.Object);

            var output = await controller.RestartGame(new P2PGameKeyDto { Code = "12345", ConnectionId = "c2" });

            p2pSvc.Verify(p => p.FindActiveSessionAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            p2pSvc.Verify(p => p.RestartGameAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            _signalRHub.VerifyGet(p => p.Clients, Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }
        #endregion
    }
}
