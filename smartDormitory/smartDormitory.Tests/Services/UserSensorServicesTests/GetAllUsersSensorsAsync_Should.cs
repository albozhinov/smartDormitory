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
    public class GetAllUsersSensorsAsync_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(null, "Some text", 1, 10)]
        [DataRow("Some text", null, 1, 10)]
        public async Task ThrowArgumentNullException_WhenArgumentsAreNull(string searchByName, string searchByTag, int page, int pageSize)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorsServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorsServiceMock.GetAllUsersSensorsAsync(searchByName, searchByTag, page, pageSize));
        }

        [TestMethod]
        [DataRow("Gosho", "", 1, 10)]
        public async Task ReturnAllUsersUserSensorsBySearchName_WhenArgumentsAreCorrect(string searchByName, string searchByTag, int page, int pageSize)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnAllUsersUserSensorsBySearchName_WhenArgumentsAreCorrect")
                .Options;


            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                string userId = Guid.NewGuid().ToString();

                var gosho = new User
                {
                    Id = userId,
                    UserName = "Gosho",
                    UserSensors = new List<UserSensors>(),
                };

                var pesho = new User
                {
                    Id = userId + "123",
                    UserName = "Pesho",
                    UserSensors = new List<UserSensors>(),
                };

                await assertContext.Users.AddRangeAsync(gosho, pesho);

                await assertContext.UserSensors.AddRangeAsync(
                new UserSensors
                {
                    UserId = userId,
                    User = gosho,
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
                    User = gosho,
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
                    User = pesho,
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
                     User = pesho,
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
                    MeasureTypeId = 1,
                };

                var sensor2 = new Sensor
                {
                    Id = 2,
                    PollingInterval = 10,
                    Description = "Some description1",
                    Tag = "Some tag1",
                    MinValue = 10.00,
                    MaxValue = 20.00,
                    TimeStamp = DateTime.Now,
                    Value = 15.00,
                    Url = "Some URL1",
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1,
                };


                await assertContext.Sensors.AddAsync(sensor1);

                await assertContext.MeasureTypes.AddAsync(new MeasureType
                {
                    Id = 1,
                    Type = "TestType",
                    Sensors = new List<Sensor>() { sensor1, sensor2 }
                });

                await assertContext.SaveChangesAsync();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);
                // Act
                var userSensorsMock = await userSensorServiceMock.GetAllUsersSensorsAsync(searchByName, searchByTag, page, pageSize);

                // Assert
                Assert.IsTrue(userSensorsMock.Count() == 2);
            }
        }

        [TestMethod]
        [DataRow("", "Humidity", 1, 10)]
        public async Task ReturnAllUsersUserSensorsBySearchTag_WhenArgumentsAreCorrect(string searchByName, string searchByTag, int page, int pageSize)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnAllUsersUserSensorsBySearchTag_WhenArgumentsAreCorrect")
                .Options;


            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                string userId = Guid.NewGuid().ToString();

                var gosho = new User
                {
                    Id = userId,
                    UserName = "Gosho",
                    UserSensors = new List<UserSensors>(),
                };

                var pesho = new User
                {
                    Id = userId + "123",
                    UserName = "Pesho",
                    UserSensors = new List<UserSensors>(),
                };

                await assertContext.Users.AddRangeAsync(gosho, pesho);

                await assertContext.UserSensors.AddRangeAsync(
                new UserSensors
                {
                    UserId = userId,
                    User = gosho,
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
                    User = gosho,
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
                    User = pesho,
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
                     User = pesho,
                     SensorId = 2,
                     Name = "Some name3",
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
                    Tag = "Humidity",
                    MinValue = 10.00,
                    MaxValue = 20.00,
                    TimeStamp = DateTime.Now,
                    Value = 15.00,
                    Url = "Some URL",
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1,
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
                    Url = "Some URL1",
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1,
                };


                await assertContext.Sensors.AddAsync(sensor1);

                await assertContext.MeasureTypes.AddAsync(new MeasureType
                {
                    Id = 1,
                    Type = "TestType",
                    Sensors = new List<Sensor>() { sensor1, sensor2 }
                });

                await assertContext.SaveChangesAsync();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);
                // Act
                var userSensorsMock = await userSensorServiceMock.GetAllUsersSensorsAsync(searchByName, searchByTag, page, pageSize);

                // Assert
                Assert.IsTrue(userSensorsMock.Count() == 2);
            }
        }

        [TestMethod]
        [DataRow("", "", 1, 10)]
        public async Task ReturnAllUsersUserSensors_WhenArgumentsAreEmpty(string searchByName, string searchByTag, int page, int pageSize)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnAllUsersUserSensors_WhenArgumentsAreEmpty")
                .Options;


            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                string userId = Guid.NewGuid().ToString();

                var gosho = new User
                {
                    Id = userId,
                    UserName = "Gosho",
                    UserSensors = new List<UserSensors>(),
                };

                var pesho = new User
                {
                    Id = userId + "123",
                    UserName = "Pesho",
                    UserSensors = new List<UserSensors>(),
                };

                await assertContext.Users.AddRangeAsync(gosho, pesho);

                await assertContext.UserSensors.AddRangeAsync(
                new UserSensors
                {
                    UserId = userId,
                    User = gosho,
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
                    User = gosho,
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
                    User = pesho,
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
                     User = pesho,
                     SensorId = 2,
                     Name = "Some name3",
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
                    Tag = "Humidity",
                    MinValue = 10.00,
                    MaxValue = 20.00,
                    TimeStamp = DateTime.Now,
                    Value = 15.00,
                    Url = "Some URL",
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1,
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
                    Url = "Some URL1",
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1,
                };


                await assertContext.Sensors.AddAsync(sensor1);

                await assertContext.MeasureTypes.AddAsync(new MeasureType
                {
                    Id = 1,
                    Type = "TestType",
                    Sensors = new List<Sensor>() { sensor1, sensor2 }
                });

                await assertContext.SaveChangesAsync();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);
                // Act
                var userSensorsMock = await userSensorServiceMock.GetAllUsersSensorsAsync(searchByName, searchByTag, page, pageSize);

                // Assert
                Assert.IsTrue(userSensorsMock.Count() == 3);
            }
        }





    }
}
