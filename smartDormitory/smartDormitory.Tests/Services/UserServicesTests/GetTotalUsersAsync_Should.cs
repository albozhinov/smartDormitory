using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.UserServicesTests
{
    [TestClass]
    public class GetTotalUsersAsync_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        public async Task ThrowArgumenNullException_WhenParameterSearchIsNull()
        {
            //Arrange
            var contextMock = new Mock<smartDormitoryDbContext>();
            var UserServiceMock = new UserService(contextMock.Object);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await UserServiceMock.GetUsersAsync(null, 1, 1));
        }

        [TestMethod]
        [DataRow("validId", "username")]
        public async Task Return_TotalUsersCount_WhenParametersAreValid(string id, string username)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "Return_TotalUsersCount_WhenParametersAreValid")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Users.AddRangeAsync(new User
                {
                    Id = id,
                    UserName = username
                },
                new User
                {
                    Id = "anotherValidId",
                    UserName = "Ivan"
                });

                await assertContext.SaveChangesAsync();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userService = new UserService(assertContext);

                //Act 
                var usersCount = await userService.GetTotalUserAsync(username);


                //Assert
                Assert.IsTrue(usersCount == 1);
            }
        }
    }
}
