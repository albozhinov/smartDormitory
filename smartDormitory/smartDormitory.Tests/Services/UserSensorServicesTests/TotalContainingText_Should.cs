using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data;
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
    public class TotalContainingText_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(null)]
        public void ThrowArgumentNullException_WhenParameterIsNull(string searchText)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorsServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            Assert.ThrowsException<ArgumentNullException>(() => userSensorsServiceMock.TotalContainingText(searchText));
        }

        [TestMethod]
        [DataRow("Humidity")]
        public void ReturnCountOfUserSensorsBySearchText_WhenArgumentIsCorrect(string searcheText)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCountOfUserSensorsBySearchText_WhenArgumentIsCorrect")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var sensor1 = new Sensor
                {
                    Id = 1,
                    PollingInterval = 10,
                    Description = "Some description",
                    Tag = "Humidity",
                    MinValue = 10.00,
                    MaxValue = 20.00,
                    TimeStamp = DateTime.Now,
                    Value = 15.00,
                    Url = "Some URL",
                    ModifiedOn = DateTime.Now,
                };

                var sensor2 = new Sensor
                {
                    Id = 2,
                    PollingInterval = 10,
                    Description = "Some description1",
                    Tag = "Temperature",
                    MinValue = 10.00,
                    MaxValue = 20.00,
                    TimeStamp = DateTime.Now,
                    Value = 15.00,
                    Url = "Some URL",
                    ModifiedOn = DateTime.Now,
                };

                assertContext.Sensors.AddRange(sensor1, sensor2);

                assertContext.UserSensors.AddRange(
                new UserSensors
                {
                    UserId = "123",
                    SensorId = 1,
                    Sensor = sensor1,
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
                    UserId = "123",
                    SensorId = 1,
                    Sensor = sensor1,
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
                    UserId = "123" + "123",
                    SensorId = 2,
                    Sensor = sensor2,
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
                     UserId = "123",
                     SensorId = 2,
                     Sensor = sensor2,
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

                assertContext.SaveChangesAsync();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);
                // Act
                var userSensorsMock = userSensorServiceMock.TotalContainingText(searcheText);

                // Assert
                Assert.IsTrue(userSensorsMock == 1);
            }
        }

    }
}
