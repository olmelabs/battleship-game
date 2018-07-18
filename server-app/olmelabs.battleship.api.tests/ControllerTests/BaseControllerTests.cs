using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Models;
using olmelabs.battleship.api.Services;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class BaseControllerTests
    {
        protected Mapper _mapper;
        protected Mock<IGameStatisticsService> _statisticsSvcMock;

        [TestInitialize()]
        public void Initialize()
        {
            AutomapperProfile automapperProfile = new AutomapperProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(automapperProfile));
            _mapper = new Mapper(configuration);

            _statisticsSvcMock = new Mock<IGameStatisticsService>();
        }

    }
}
