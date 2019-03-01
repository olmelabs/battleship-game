using Microsoft.VisualStudio.TestTools.UnitTesting;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Implementations;
using System;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.tests.ServiceTests
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public async Task Register_Ok()
        {
            IStorage storage = new InMemoryStaticStorage();

            User u = new User() { Email = "user1@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            User newUser = await svc.RegisterUserAsync(u);

            Assert.IsNotNull(newUser);
            Assert.IsFalse(newUser.IsEmailConfirmed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Register_Duplicate_Fail()
        {
            IStorage storage = new InMemoryStaticStorage();

            User u = new User() { Email = "user1@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            User newUser = await svc.RegisterUserAsync(u);
            newUser = await svc.RegisterUserAsync(u);

            Assert.IsNotNull(newUser);
            Assert.IsTrue(newUser.IsEmailConfirmed);
        }

        [TestMethod]
        public async Task RegisterResetPasswordCode_Ok()
        {
            IStorage storage = new InMemoryStaticStorage();

            User u = new User() { Email = "user1@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            string  code = await svc.RegisterResetPasswordCodeAsync(u);

            string res = await storage.GetEmailByResetPasswordCodeAsync(code);

            Assert.AreEqual(u.Email, res);
        }

        [TestMethod]
        public async Task GetUserByResetPasswordToken_Ok()
        {
            IStorage storage = new InMemoryStaticStorage();
            await storage.Prepare();

            User u = new User() { Email = "user@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            string code = await svc.RegisterResetPasswordCodeAsync(u);
            User res = await svc.GetUserByResetPasswordTokenAsync(code);

            //now should return null as code already used
            var code2 = await storage.GetEmailByResetPasswordCodeAsync(code);

            Assert.AreEqual(u.Email, res.Email);
            Assert.IsNull(code2);
        }

        [TestMethod]
        public async Task RegisterEmailConfirmationCode_Ok()
        {
            IStorage storage = new InMemoryStaticStorage();

            User u = new User() { Email = "user1@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            string code = await svc.RegisterEmailConfirmationCodeAsync(u);

            string res = await storage.GetEmailByConfirmationCodeAsync(code);

            Assert.AreEqual(u.Email, res);
        }

        [TestMethod]
        public async Task GetUserByEmailConfirmationCode_Ok()
        {
            IStorage storage = new InMemoryStaticStorage();
            await storage.Prepare();

            User u = new User() { Email = "user@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            string code = await svc.RegisterEmailConfirmationCodeAsync(u);
            User res = await svc.GetUserByEmailConfirmationCodeAsync(code);

            //now should return null as code already used
            var code2 = await storage.GetEmailByConfirmationCodeAsync(code);

            Assert.AreEqual(u.Email, res.Email);
            Assert.IsNull(code2);
        }

        [TestMethod]
        public async Task ConfirmEmail_Ok()
        {
            IStorage storage = new InMemoryStaticStorage();

            User u = new User() { Email = "user3@domain.com", Password = "password" };
            AccountService svc = new AccountService(storage);

            User newUser = await svc.RegisterUserAsync(u);
            var res1 = newUser.IsEmailConfirmed;

            await svc.ConfirmEmailAsync(u);
            var res2 = newUser.IsEmailConfirmed;

            Assert.IsNotNull(newUser);
            Assert.IsFalse(res1);
            Assert.IsTrue(res2);
        }
    }
}
