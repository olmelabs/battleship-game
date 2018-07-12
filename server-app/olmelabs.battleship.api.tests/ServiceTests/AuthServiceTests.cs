using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services;

namespace olmelabs.battleship.api.tests.ServiceTests
{
    [TestClass]
    public class AuthServiceTests
    {
        [TestMethod]
        public void GetPrincipalFromExpiredToken_Test()
        {
            IStorage storage = new InMemoryStaticStorage();

            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x["Jwt:Key"]).Returns("9e41f7cd-b71e-4d9c-a6a4-0a5c4aa0f66a");

            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGRvbWFpbi5jb20iLCJlbWFpbCI6InVzZXJAZG9tYWluLmNvbSIsImp0aSI6IjM0NjkwOWU3LTNmOTktNGY3Ni1iMjg2LWQ4YzUxNzViZTg2YyIsImV4cCI6MTUzMDU0MjQ5MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo2MzM1NC8iLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjYzMzU0LyJ9.1LughzKLTFWyJijEVhtOaT-zAVvKRKU-koNX_Zc095w";

            AuthService svc = new AuthService(storage, config.Object);
            var principal = svc.GetPrincipalFromExpiredToken(token);

            Assert.IsNotNull(principal);
        }
    }
}
