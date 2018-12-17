using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.UserServicesTests
{
    [TestClass]
    public class GetUserAsync_Should
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
        [DataRow("validSearch", 0, 1)]
        [DataRow("validSearch", 1, 0)]
        public async Task ThrowArgumenException_WhenParametersAreInvalid(string searchText, int page, int pageSize)
        {
            //Arrange
            var contextMock = new Mock<smartDormitoryDbContext>();
            var UserServiceMock = new UserService(contextMock.Object);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await UserServiceMock.GetUsersAsync(searchText, page, pageSize));
        }

        [TestMethod]
        [DataRow("validId", "username")]
        public async Task Return_UserICollection_WhenParametersAreValid(string id, string username)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "Return_UserICollection_WhenParametersAreValid")
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
                var allUsers = new List<User>(await userService.GetUsersAsync(username, 1, 2));


                //Assert
                Assert.IsTrue(allUsers[0].Id == id);
                Assert.IsTrue(allUsers[0].UserName == username);
                Assert.IsTrue(allUsers.Count == 1);
            }
        }
    }
}
