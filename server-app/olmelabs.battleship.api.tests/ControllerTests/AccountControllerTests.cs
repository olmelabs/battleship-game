using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Controllers;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : BaseControllerTests
    {

        [TestMethod]
        public async Task Register_Success()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.Register(new RegisterModelDto());

            accountService.Verify(m => m.RegisterUserAsync(It.IsAny<User>()), Times.Once());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));

            var res = (SimpleResponseDto)dto;
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task Register_Fail()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "user@domain.com" });

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.Register(new RegisterModelDto());
            
            accountService.Verify(m => m.RegisterUserAsync(It.IsAny<User>()), Times.Never());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));

            var res = (SimpleResponseDto)dto;
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ConfirmEmail_Success()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailConfirmationCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "user@domain.com" });

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.ConfirmEmail("code");

            accountService.Verify(m => m.ConfirmEmailAsync(It.IsAny<User>()), Times.Once());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));
        }

        [TestMethod]
        public async Task ConfirmEmail_Fail()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailConfirmationCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "user@domain.com", IsEmailConfirmed = true });

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.ConfirmEmail("code");

            accountService.Verify(m => m.ConfirmEmailAsync(It.IsAny<User>()), Times.Never());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));
        }

        [TestMethod]
        public async Task SendResetPasswordLink_EmailExists_Success()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "user@domain.com", IsEmailConfirmed = true });

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.SendResetPasswordLink("user@domain.com");

            accountService.Verify(m => m.RegisterResetPasswordCodeAsync(It.IsAny<User>()), Times.Once());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));
        }

        [TestMethod]
        public async Task SendResetPasswordLink_EmailDoesNotExists_Success()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.SendResetPasswordLink("user@domain.com");

            accountService.Verify(m => m.RegisterResetPasswordCodeAsync(It.IsAny<User>()), Times.Never());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));
        }

        [TestMethod]
        public async Task ResetPassword_Success()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByResetPasswordTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "user@domain.com", IsEmailConfirmed = true});

            ResetPasswordDto inputDto = new ResetPasswordDto { Code = "code", Password = "password", Password2 = "password" };

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.ResetPassword(inputDto);

            accountService.Verify(m => m.ResetPasswordAsync(It.IsAny<User>()), Times.Once());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));
        }

        [TestMethod]
        public async Task ResetPassword_Fail()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByResetPasswordTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            ResetPasswordDto inputDto = new ResetPasswordDto { Code = "code", Password = "password", Password2 = "password" };

            var controller = new AccountController(null, accountService.Object, _mapper, _notificationServiceMock.Object);
            var output = await controller.ResetPassword(inputDto);

            accountService.Verify(m => m.ResetPasswordAsync(It.IsAny<User>()), Times.Never());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));
            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(SimpleResponseDto));
        }

    }
}
