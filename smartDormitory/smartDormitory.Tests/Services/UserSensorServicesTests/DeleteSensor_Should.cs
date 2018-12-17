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

namespace smartDormitory.Tests.Services.UserSensorServicesTests
{
    [TestClass]
    public class DeleteSensor_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task ThrowArgumentException_WhenArgumentIsIncorrect(int id)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.DeleteSensor(id));
        }

        [TestMethod]
        [DataRow(5)]
        public async Task ThrowArgumentNullEcxeption_WhenUserSensorIsNotFound(int id)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentNullEcxeption_WhenUserSensorIsNotFound")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {

                var userId = Guid.NewGuid().ToString();
                await assertContext.UserSensors.AddAsync(
                    new UserSensors
                    {
                        Id = 1,
                        UserId = userId,
                        SensorId = 1,
                        Name = "Some name",
                        Description = "Some description",
                        MinValue = 13,
                        MaxValue = 20,
                        PollingInterval = 33,
                        Latitude = 3.15,
                        Longitude = 7.15,
                        IsPublic = false,
                        Alarm = false,
                        IsDeleted = false,
                        ImageUrl = "Some Url",
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.DeleteSensor(id));
            }
        }

        [TestMethod]
        [DataRow(1)]
        public async Task DeleteUserSensor_WhenArgumentIsCorrect(int id)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUserSensor_WhenArgumentIsCorrect")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {

                var userId = Guid.NewGuid().ToString();
                await assertContext.UserSensors.AddAsync(
                    new UserSensors
                    {
                        Id = 1,
                        UserId = userId,
                        SensorId = 1,
                        Name = "Some name",
                        Description = "Some description",
                        MinValue = 13,
                        MaxValue = 20,
                        PollingInterval = 33,
                        Latitude = 3.15,
                        Longitude = 7.15,
                        IsPublic = false,
                        Alarm = false,
                        IsDeleted = false,
                        ImageUrl = "Some Url",
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                // Act
                await userSensorServiceMock.DeleteSensor(id);
                var deletedSensor = await assertContext.UserSensors.FirstOrDefaultAsync(s => s.Id == id);

                Assert.IsTrue(deletedSensor.IsDeleted == true);
            }
        }


    }
}
