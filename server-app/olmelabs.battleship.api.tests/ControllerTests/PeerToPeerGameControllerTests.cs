using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Controllers;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;
using olmelabs.battleship.api.SignalRHubs;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class PeerToPeerGameControllerTests: BaseControllerTests
    {
        [TestMethod]
        public async Task StartNewGameTest()
        {
            var signalRHub = new Mock<IHubContext<GameHub>>();

            var p2pSvc = new Mock<IPeerToPeerGameService>();
            p2pSvc.Setup(x => x.StartNewSessionAsync(It.IsAny<string>()))
                .ReturnsAsync(new PeerToPeerSessionState() { Code = "12345" });

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object);
            var output = await controller.StartSession("connectionid");

            p2pSvc.Verify(p => p.StartNewSessionAsync(It.IsAny<string>()), Times.Once);

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(P2PStartSessionDto));
        }

        [TestMethod]
        public async Task StartNewGameTest_EmptyConnection()
        {
            var p2pSvc = new Mock<IPeerToPeerGameService>();

            var controller = new PeerToPeerGameController(p2pSvc.Object, _mapper, _signalRHub.Object);
            var output = await controller.StartSession(null);

            p2pSvc.Verify(p => p.StartNewSessionAsync(It.IsAny<string>()), Times.Never);

            Assert.AreEqual(output.GetType(), typeof(BadRequestResult));
        }

    }
}
