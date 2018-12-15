using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data;
using smartDormitory.Data.Context;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.UserSensorServicesTests
{
    [TestClass]
    public class AddUserSensorAsyncShould
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(null, 1, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, null, "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", null, 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, null)]
        public async Task ThrowArgumentNullException_WhenArgumentsAreNull(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl));
        }

        [TestMethod]
        [DataRow("Valid userId", 0, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", -1, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "1", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, " ", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "More than 20 symbols!!", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", "1", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", " ", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", "", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", "More than 250 symbolsssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", "Valid description", 5.00, 15.00, 41, 2.15, 5.15, true, false, "URL")]
        [DataRow("Valid userId", 1, "Valid name", "Valid description", 5.00, 15.00, 9, 2.15, 5.15, true, false, "URL")]
        public async Task ThrowArgumentException_WhenArgumentsAreIncorrect(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl));
        }

        [TestMethod]
        [DataRow("Valid useId", 5, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        public async Task ThrowArgumentNullException_WhenSensorDoesNotExist(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenSensorDoseNotExist")
                .Options;

            int existID = 1;
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = existID,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    },
                    new Sensor
                    {
                        Id = 2,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl));
            }
        }

        [TestMethod]
        [DataRow("Valid useId", 1, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false, "URL")]
        public async Task ThrowArgumentException_WhenMinValueIsOutOfSensorValueRange(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenMinValueIsOutOfSensorValueRange")
                .Options;

            int existID = sensorId;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = existID,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    },
                    new Sensor
                    {
                        Id = 2,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl));
            }
        }

        [TestMethod]
        [DataRow("Valid useId", 1, "Valid name", "Valid description", 11.00, 21.00, 40, 2.15, 5.15, true, false, "URL")]
        public async Task ThrowArgumentException_WhenMaxValueIsOutOfSensorValueRange(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenMaxValueIsOutOfSensorValueRange")
                .Options;

            int existID = sensorId;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = existID,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    },
                    new Sensor
                    {
                        Id = 2,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl));
            }
        }

        [TestMethod]
        [DataRow("Valid useId", 1, "Valid name", "Valid description", 11.00, 19.00, 40, 2.15, 5.15, true, false, "URL")]
        public async Task AddUserSensor_WhenArgumentsAreCorrect(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "AddUserSensor_WhenArgumentsAreCorrect")
                .Options;

            int existID = sensorId;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = existID,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    },
                    new Sensor
                    {
                        Id = 2,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = "Some tag",
                        MinValue = 10.00,
                        MaxValue = 20.00,
                        TimeStamp = DateTime.Now,
                        Value = 15.00,
                        Url = "Some URL",
                        ModifiedOn = DateTime.Now,
                    });
                await assertContext.SaveChangesAsync();
            }

             
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);
                // Act
                await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl);

                var addedSensor = await assertContext.UserSensors.FirstOrDefaultAsync(s => s.SensorId == sensorId);

                // Assert
                Assert.IsTrue(addedSensor.UserId == userId);
                Assert.IsTrue(addedSensor.SensorId == sensorId);
                Assert.IsTrue(addedSensor.Name == name);
                Assert.IsTrue(addedSensor.Description == description);
                Assert.IsTrue(addedSensor.MinValue == minValue);
                Assert.IsTrue(addedSensor.MaxValue == maxValue);
                Assert.IsTrue(addedSensor.PollingInterval == pollingInterval);
                Assert.IsTrue(addedSensor.Latitude == latitude);
                Assert.IsTrue(addedSensor.Longitude == longitude);
                Assert.IsTrue(addedSensor.IsPublic == isPublic);
                Assert.IsTrue(addedSensor.Alarm == alarm);
                Assert.IsTrue(addedSensor.ImageUrl == imageUrl);
            }
        }
    }
}
