using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Tests.Services.UserSensorServicesTests
{
    [TestClass]
    public class TotalByName_Should
    {

        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow("Gosho")]
        public void ReturnCountOfAllNotDeletedUserSensorsBySearchName(string searchName)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCountOfAllNotDeletedUserSensorsBySearchName")
                .Options;
            
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {

                var userId = Guid.NewGuid().ToString();
                var userSensor1 = new UserSensors
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
                };

                var userSensor2 = new UserSensors
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
                };
                var userSensor3 = new UserSensors
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
                };

                assertContext.UserSensors.AddRange(userSensor1, userSensor2, userSensor3);

                assertContext.Users.AddRange(new User
                {
                    Id = userId,
                    UserName = "Gosho",
                    UserSensors = new List<UserSensors>() { userSensor1, userSensor2 }
                },
                new User
                {
                    Id = userId + "123",
                    UserName = "Pesho",
                    UserSensors = new List<UserSensors>() { userSensor3 }
                });

                assertContext.SaveChanges();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                // Act
                var totalSensorsCountMock = userSensorServiceMock.TotalByName(searchName);

                // Assert
                Assert.IsTrue(totalSensorsCountMock == 1);
            }
        }

    }
}
