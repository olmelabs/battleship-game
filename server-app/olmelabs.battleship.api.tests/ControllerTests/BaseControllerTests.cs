using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

        [TestInitialize()]
        public void Initialize()
        {
            AutomapperProfile automapperProfile = new AutomapperProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(automapperProfile));
            _mapper = new Mapper(configuration);

            _statisticsSvcMock = new Mock<IGameStatisticsService>();
            _notificationServiceMock = new Mock<INotificationService>();

            _signalRHub = new Mock<IHubContext<GameHub>>();
        }

    }
}
