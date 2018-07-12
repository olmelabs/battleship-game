using Microsoft.VisualStudio.TestTools.UnitTesting;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services;
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
            Assert.IsTrue(newUser.IsEmailConfirmed);
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
    }
}
