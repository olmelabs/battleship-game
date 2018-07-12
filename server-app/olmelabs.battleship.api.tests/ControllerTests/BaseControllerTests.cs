using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using olmelabs.battleship.api.Models;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class BaseControllerTests
    {
        protected Mapper _mapper;

        [TestInitialize()]
        public void Initialize()
        {
            AutomapperProfile automapperProfile = new AutomapperProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(automapperProfile));
            _mapper = new Mapper(configuration);
        }

    }
}
