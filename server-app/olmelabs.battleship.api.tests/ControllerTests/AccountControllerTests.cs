﻿using Microsoft.AspNetCore.Mvc;
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
    public class AccountControllerTests : BaseControllerTests
    {

        [TestMethod]
        public async Task Register_Ok()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new AccountController(null, accountService.Object, _mapper);
            var output = await controller.Register(new RegisterModelDto());

            accountService.Verify(m => m.RegisterUserAsync(It.IsAny<User>()), Times.Once());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(RegisterResponseDto));

            var res = (RegisterResponseDto)dto;
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task Register_Fail()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.FindUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "user@domain.com" });

            var controller = new AccountController(null, accountService.Object, _mapper);
            var output = await controller.Register(new RegisterModelDto());
            
            accountService.Verify(m => m.RegisterUserAsync(It.IsAny<User>()), Times.Never());

            Assert.AreEqual(output.GetType(), typeof(OkObjectResult));

            dynamic dto = ((OkObjectResult)output).Value;
            Assert.AreEqual(dto.GetType(), typeof(RegisterResponseDto));

            var res = (RegisterResponseDto)dto;
            Assert.IsFalse(res.Success);
        }
    }
}
