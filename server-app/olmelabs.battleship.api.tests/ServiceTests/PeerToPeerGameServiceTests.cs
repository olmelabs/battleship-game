using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ServiceTests
{
    [TestClass]
    public class PeerToPeerGameServiceTests
    {
        private Mock<IStorage> _storage;

        [TestInitialize()]
        public void Initialize()
        {
            _storage = new Mock<IStorage>();
        }

        #region StartNewSessionAsync
        [TestMethod]
        public async Task StartNewSessionAsync_Ok()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);

            _storage.Setup(p => p.AddP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(new PeerToPeerSessionState());

            PeerToPeerSessionState res = await svc.StartNewSessionAsync("connectionId");

            _storage.Verify(p => p.AddP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
        }
        #endregion

        #region JoinSessionAsync
        [TestMethod]
        public async Task JoinSessionAsync_Ok()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(new PeerToPeerSessionState());
            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(new PeerToPeerSessionState());

            PeerToPeerSessionState res = await svc.JoinSessionAsync("code", "connectionId");

            _storage.Verify(p => p.FindP2PSessionAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task JoinSessionAsync_FriendAlreadyConnected()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(new PeerToPeerSessionState() { FriendConnectionId = "connectionId" });
            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(new PeerToPeerSessionState());

            PeerToPeerSessionState res = await svc.JoinSessionAsync("code", "connectionId");

            _storage.Verify(p => p.FindP2PSessionAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            Assert.IsNull(res);
        }
        #endregion

        #region AddPeerToSession
        [TestMethod]
        public async Task AddPeerToSession_HostAdded()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            var session = new PeerToPeerSessionState()
            {
                Code = "code",
                HostConnectionId = "HostConnectionId",
                FriendConnectionId = "FriendConnectionId"
            };

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(session);
            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "HostConnectionId", new List<ShipInfo>());

            _storage.Verify(p => p.FindP2PSessionAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
            Assert.IsTrue(session.HostStartedGame);
            Assert.AreEqual(1, session.GameStartedCount);
            Assert.IsNotNull(session.HostShips);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task AddPeerToSession_HostAlreadyConnectedBefore()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            var session = new PeerToPeerSessionState()
            {
                Code = "code",
                HostConnectionId = "HostConnectionId",
                FriendConnectionId = "FriendConnectionId",
                HostStartedGame = true,
            };

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "HostConnectionId", new List<ShipInfo>());
        }

        [TestMethod]
        public async Task AddPeerToSession_FriendAdded()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            var session = new PeerToPeerSessionState()
            {
                Code = "code",
                HostConnectionId = "HostConnectionId",
                FriendConnectionId = "FriendConnectionId"
            };

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(session);
            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "FriendConnectionId", new List<ShipInfo>());

            _storage.Verify(p => p.FindP2PSessionAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
            Assert.IsTrue(session.FriendStartedGame);
            Assert.AreEqual(1, session.GameStartedCount);
            Assert.IsNotNull(session.FriendShips);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task AddPeerToSession_FriendAlreadyConnectedBefore()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            var session = new PeerToPeerSessionState()
            {
                Code = "code",
                HostConnectionId = "HostConnectionId",
                FriendConnectionId = "FriendConnectionId",
                FriendStartedGame = true,
            };

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "FriendConnectionId", new List<ShipInfo>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task AddPeerToSession_GameAlreadyJoinedByAll()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            var session = new PeerToPeerSessionState()
            {
                Code = "code",
                HostConnectionId = "HostConnectionId",
                FriendConnectionId = "FriendConnectionId",
                GameStartedCount = 2,
            };

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "FriendConnectionId", new List<ShipInfo>());
        }

        [TestMethod]
        public async Task AddPeerToSession_SessionNotFound()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync((PeerToPeerSessionState)null);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "FriendConnectionId", new List<ShipInfo>());

            _storage.Verify(p => p.FindP2PSessionAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            Assert.IsNull(res);
        }

        [TestMethod]
        public async Task AddPeerToSession_WrongConnectionId()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            var session = new PeerToPeerSessionState()
            {
                Code = "code",
                HostConnectionId = "HostConnectionId",
                FriendConnectionId = "FriendConnectionId",
            };

            _storage.Setup(p => p.FindP2PSessionAsync(It.IsAny<string>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.AddPeerToSession("code", "WrongConnectionId", new List<ShipInfo>());

            _storage.Verify(p => p.FindP2PSessionAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Never);

            Assert.IsNull(res);
        }
        #endregion

        #region StartNewGameAsync / StopGameAsync / RestartGameAsync

        [TestMethod]
        public async Task StartNewGameAsync()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            PeerToPeerGameState game = PeerToPeerGameState.CreateNew();
            PeerToPeerSessionState session = new PeerToPeerSessionState();

            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.StartNewGameAsync(session);

            _storage.Verify(p => p.AddP2PGameAsync(It.IsAny<PeerToPeerGameState>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task StopGameAsync()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            PeerToPeerGameState game = PeerToPeerGameState.CreateNew();
            PeerToPeerSessionState session = new PeerToPeerSessionState();

            _storage.Setup(p => p.FindActiveP2PGameAsync(It.IsAny<string>())).ReturnsAsync(game);
            _storage.Setup(p => p.UpdateP2PGameAsync(It.IsAny<PeerToPeerGameState>())).ReturnsAsync(game);
            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.StopGameAsync(session);

            _storage.Verify(p => p.FindActiveP2PGameAsync(It.IsAny<string>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PGameAsync(It.IsAny<PeerToPeerGameState>()), Times.Once);
            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
        }


        [TestMethod]
        public async Task RestartGameAsync()
        {
            PeerToPeerGameService svc = new PeerToPeerGameService(_storage.Object);
            PeerToPeerSessionState session = new PeerToPeerSessionState();

            _storage.Setup(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>())).ReturnsAsync(session);

            PeerToPeerSessionState res = await svc.RestartGameAsync(session);

            _storage.Verify(p => p.UpdateP2PSessionAsync(It.IsAny<PeerToPeerSessionState>()), Times.Once);

            Assert.IsNotNull(res);
        }
        #endregion
    }
}
