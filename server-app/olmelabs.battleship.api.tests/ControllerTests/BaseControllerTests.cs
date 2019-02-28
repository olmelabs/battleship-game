using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Controllers;
using olmelabs.battleship.api.Models;
using olmelabs.battleship.api.Services.Interfaces;
using olmelabs.battleship.api.SignalRHubs;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class BaseControllerTests
    {
        protected Mapper _mapper;
        protected Mock<IGameStatisticsService> _statisticsSvcMock;
        protected Mock<INotificationService> _notificationServiceMock;
        protected Mock<IHubContext<GameHub>> _signalRHub;
        protected Mock<ILogger<PeerToPeerGameController>> _logger;

        [TestInitialize()]
        public void Initialize()
        {
            AutomapperProfile automapperProfile = new AutomapperProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(automapperProfile));
            _mapper = new Mapper(configuration);

            _statisticsSvcMock = new Mock<IGameStatisticsService>();
            _notificationServiceMock = new Mock<INotificationService>();

            _signalRHub = new Mock<IHubContext<GameHub>>();

            var clients = new Mock<IHubClients>();
            var client = new Mock<IClientProxy>();
            clients.Setup(x => x.Client(It.IsAny<string>())).Returns(client.Object);

            _signalRHub.Setup(x => x.Clients).Returns(clients.Object);

            _logger = new Mock<ILogger<PeerToPeerGameController>>();
        }

    }
}
