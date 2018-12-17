using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.UserServicesTests
{
    [TestClass]
    public class GetUserEmailAsync_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        public async Task ThrowArgumenNullException_WhenParameterIdIsNull()
        {
            //Arrange
            var contextMock = new Mock<smartDormitoryDbContext>();
            var UserServiceMock = new UserService(contextMock.Object);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await UserServiceMock.GetUserEmailAsync(null));
        }

        [TestMethod]
        [DataRow("validId", "username", "validEmail")]
        public async Task Return_UserEmail_WhenParametersAreValid(string id, string username, string email)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "Return_UserEmail_WhenParametersAreValid")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Users.AddRangeAsync(new User
                {
                    Id = id,
                    UserName = username,
                    Email = email
                },
                new User
                {
                    Id = "anotherValidId",
                    UserName = "Ivan",
                    Email = email
                });

                await assertContext.SaveChangesAsync();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userService = new UserService(assertContext);

                //Act 
                var userEmail = await userService.GetUserEmailAsync(id);


                //Assert
                Assert.IsTrue(userEmail == email);
            }
        }
    }
}
