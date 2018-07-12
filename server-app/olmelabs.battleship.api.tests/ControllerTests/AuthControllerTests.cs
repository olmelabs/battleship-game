using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Controllers;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class AuthControllerTests : BaseControllerTests
    {
        [TestMethod]
        public async Task Login_Ok()
        {
            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x[It.IsAny<string>()]).Returns("00000000-0000-0000-0000-000000000000");

            var user = new User { FirstName = "John", LastName = "Doe", Email = "user@domain.com" };
            var authService = new Mock<IAuthService>();
            authService.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            var controller = new AuthController(config.Object, authService.Object, _mapper);
            var output = await controller.CreateToken(new Models.Dto.LoginModelDto());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(((OkObjectResult)output).Value.GetType(), typeof(UserModelDto));
        }


        [TestMethod]
        public async Task Login_Fail()
        {
            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x[It.IsAny<string>()]).Returns("00000000-0000-0000-0000-000000000000");

            var authService = new Mock<IAuthService>();
            authService.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new AuthController(config.Object, authService.Object, _mapper);
            var output = await controller.CreateToken(new Models.Dto.LoginModelDto());

            Assert.AreEqual(output.GetType(), typeof(UnauthorizedResult));

        }
    }
}
