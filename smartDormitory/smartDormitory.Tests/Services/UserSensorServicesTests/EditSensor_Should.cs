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
    public class EditSensor_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(1, null, "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        [DataRow(1, "Valid icbSensorId", null, "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        [DataRow(1, "Valid icbSensorId", "Valid name", null, 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        public async Task ThrowArgumentNullException_WhenPartOfArgumentsAreNull(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm));
        }

        [TestMethod]
        [DataRow(0, "Valid icbSensorId", "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        [DataRow(-1, "Valid icbSensorId", "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        [DataRow(1, "Valid icbSensorId", "Valid name", "Valid description", 5.00, 15.00, 9, 2.15, 5.15, true, false)]
        [DataRow(1, "Valid icbSensorId", "Valid name", "Valid description", 5.00, 15.00, 41, 2.15, 5.15, true, false)]
        [DataRow(1, "Valid icbSensorId", "1", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        [DataRow(1, "Valid icbSensorId", "More than 20 symbols!!!", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        public async Task ThrowArgumentException_WhenArgumentsAreIncorrect(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm));
        }


        [TestMethod]
        [DataRow(1, "Valid icbSensorId", "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        public async Task ThrowArgumentNullException_WhenICBSensorDoesNotExist(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenICBSensorDoesNotExist")
                .Options;
            
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = 1,
                        IcbSensorId = "Another icbSensorId",
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
                        IcbSensorId = "Another icbSensorId 1",
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

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm));
            }
        }

        [TestMethod]
        [DataRow(1, "Valid icbSensorId", "Valid name", "Valid description", 5.00, 15.00, 40, 2.15, 5.15, true, false)]
        public async Task ThrowArgumentException_WhenMinValueIsOutOfICBSensorValuesRange(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenMinValueIsOutOfICBSensorValuesRange")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = 1,
                        IcbSensorId = "Valid icbSensorId",
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
                        IcbSensorId = "Another icbSensorId 1",
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

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm));
            }
        }

        [TestMethod]
        [DataRow(1, "Valid icbSensorId", "Valid name", "Valid description", 11.00, 21.00, 40, 2.15, 5.15, true, false)]
        public async Task ThrowArgumentException_WhenMaxValueIsOutOfICBSensorValuesRange(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenMaxValueIsOutOfICBSensorValuesRange")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = 1,
                        IcbSensorId = "Valid icbSensorId",
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
                        IcbSensorId = "Another icbSensorId 1",
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

                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm));
            }
        }

        [TestMethod]
        [DataRow(111, "Valid icbSensorId", "Valid name", "Valid description", 11.00, 20.00, 40, 2.15, 5.15, true, false)]
        public async Task ThrowArgumentNullException_WhenUserSensorNotFound(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenUserSensorNotFound")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = 1,
                        IcbSensorId = "Valid icbSensorId",
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
                        IcbSensorId = "Another icbSensorId 1",
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

                var userId = Guid.NewGuid().ToString();
                await assertContext.UserSensors.AddRangeAsync(
                    new UserSensors {
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
                    },
                    new UserSensors
                    {
                        Id = 2,
                        UserId = userId,
                        SensorId = 2,
                        Name = "Some name 2",
                        Description = "Some description 2",
                        MinValue = 11,
                        MaxValue = 20,
                        PollingInterval = 40,
                        Latitude = 4.15,
                        Longitude = 5.15,
                        IsPublic = false,
                        Alarm = false,
                        IsDeleted = false,
                        ImageUrl = "Some Url 2",
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm));
            }
        }

        [TestMethod]
        [DataRow(1, "Valid icbSensorId", "Valid name", "Valid description", 11.00, 20.00, 40, 2.15, 5.15, true, true)]
        public async Task EditUserSensor_WhenArgumentsAreCorrect(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "EditUserSensor_WhenArgumentsAreCorrect")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = 1,
                        IcbSensorId = "Valid icbSensorId",
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
                        IcbSensorId = "Another icbSensorId 1",
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

                var userId = Guid.NewGuid().ToString();
                await assertContext.UserSensors.AddRangeAsync(
                    new UserSensors
                    {
                        Id = 1,
                        UserId = userId,
                        SensorId = 1,
                        Name = "Some name",
                        Description = "Some description",
                        MinValue = 13,
                        MaxValue = 18,
                        PollingInterval = 33,
                        Latitude = 3.15,
                        Longitude = 7.15,
                        IsPublic = false,
                        Alarm = false,
                        IsDeleted = false,
                        ImageUrl = "Some Url",
                    },
                    new UserSensors
                    {
                        Id = 2,
                        UserId = userId,
                        SensorId = 2,
                        Name = "Some name 2",
                        Description = "Some description 2",
                        MinValue = 11,
                        MaxValue = 20,
                        PollingInterval = 40,
                        Latitude = 4.15,
                        Longitude = 5.15,
                        IsPublic = false,
                        Alarm = false,
                        IsDeleted = false,
                        ImageUrl = "Some Url 2",
                    });
                await assertContext.SaveChangesAsync();
            }

            // Act and Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                // Act
                await userSensorServiceMock.EditSensor(userSensorId, icbSensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm);
                var editedSensorMock = await assertContext.UserSensors.FirstOrDefaultAsync(s => s.Id == userSensorId);

                Assert.IsTrue(editedSensorMock.Name == name);
                Assert.IsTrue(editedSensorMock.Description == description);
                Assert.IsTrue(editedSensorMock.MinValue == minValue);
                Assert.IsTrue(editedSensorMock.MaxValue == maxValue);
                Assert.IsTrue(editedSensorMock.PollingInterval == pollingInterval);
                Assert.IsTrue(editedSensorMock.Latitude == latitude);
                Assert.IsTrue(editedSensorMock.Longitude == longitude);
                Assert.IsTrue(editedSensorMock.IsPublic == isPublic);
                Assert.IsTrue(editedSensorMock.Alarm == alarm);
            }
        }




    }
}
