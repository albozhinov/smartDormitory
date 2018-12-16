using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.UserSensorServicesTests
{

    [TestClass]
    public class GetAllUserSensorsAsync_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(null)]
        public async Task ThrowArgumentNullException_WhenArgumentIsNull(string id)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorsServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorsServiceMock.GetAllUserSensorsAsync(id));
        }

        [TestMethod]
        public async Task ReturnAllUserSensorsAsync_WhenArgumentIsCorrect()
        {          
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnAllUserSensorsAsync_WhenArgumentIsCorrect")
                .Options;

            string userId = Guid.NewGuid().ToString();

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.UserSensors.AddRangeAsync(                  
                new UserSensors
                {
                    UserId = userId,
                    SensorId = 1,
                    Name = "Some name",
                    Description = "Some description",
                    MinValue = 11,
                    MaxValue = 18,
                    PollingInterval = 13,
                    Latitude = 1.15,
                    Longitude = 5.15,
                    IsPublic = true,
                    Alarm = false,
                    IsDeleted = false,
                    ImageUrl = "Some Url",
                },
                new UserSensors
                {
                    UserId = userId,
                    SensorId = 1,
                    Name = "Some name1",
                    Description = "Some description1",
                    MinValue = 12,
                    MaxValue = 19,
                    PollingInterval = 23,
                    Latitude = 2.15,
                    Longitude = 6.15,
                    IsPublic = true,
                    Alarm = false,
                    IsDeleted = true,
                    ImageUrl = "Some Url 1",
                },
                new UserSensors
                {
                    UserId = userId + "123",
                    SensorId = 1,
                    Name = "Some name2",
                    Description = "Some description2",
                    MinValue = 13,
                    MaxValue = 20,
                    PollingInterval = 33,
                    Latitude = 3.15,
                    Longitude = 7.15,
                    IsPublic = false,
                    Alarm = false,
                    IsDeleted = false,
                    ImageUrl = "Some Url 2",
                },
                 new UserSensors
                 {
                     UserId = userId,
                     SensorId = 2,
                     Name = "Some name2",
                     Description = "Some description3",
                     MinValue = 13,
                     MaxValue = 20,
                     PollingInterval = 33,
                     Latitude = 3.15,
                     Longitude = 7.15,
                     IsPublic = false,
                     Alarm = false,
                     IsDeleted = false,
                     ImageUrl = "Some Url 3",
                 }
                );

                var sensor1 = new Sensor
                {
                    Id = 1,
                    PollingInterval = 10,
                    Description = "Some description",
                    Tag = "Some tag",
                    MinValue = 10.00,
                    MaxValue = 20.00,
                    TimeStamp = DateTime.Now,
                    Value = 15.00,
                    Url = "Some URL",
                    ModifiedOn = DateTime.Now,
                };
                await assertContext.Sensors.AddAsync(sensor1);

                await assertContext.MeasureTypes.AddAsync(new MeasureType
                {
                    Type = "TestType",
                    Sensors = new List<Sensor>() { sensor1 }
                });

                await assertContext.SaveChangesAsync();
            }


            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);
                // Act
                var userSensorsMock = await userSensorServiceMock.GetAllUserSensorsAsync(userId);

                // Assert
                Assert.IsTrue(userSensorsMock.Count() == 1);
                Assert.IsTrue(userSensorsMock.First().UserId == userId);
            }
        }

    }
}
