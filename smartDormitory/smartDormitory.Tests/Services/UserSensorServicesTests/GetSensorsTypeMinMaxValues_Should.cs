using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.UserSensorServicesTests
{
    [TestClass]
    public class GetSensorsTypeMinMaxValues_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(null)]
        public async Task ThrowArgumentNullException_WhenArgumentIsNull(string tag)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.GetSensorsTypeMinMaxValues(tag));
        }

        //[TestMethod]
        //[DataRow("Valid useId", 1, "Valid name", "Valid description", 11.00, 21.00, 40, 2.15, 5.15, true, false, "URL")]
        //public async Task ThrowArgumentException_WhenMaxValueIsOutOfSensorValueRange(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
        //{
        //    // Arrange
        //    contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
        //        .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenMaxValueIsOutOfSensorValueRange")
        //        .Options;

        //    int existID = sensorId;

        //    using (var assertContext = new smartDormitoryDbContext(contextOptions))
        //    {
        //        await assertContext.Sensors.AddRangeAsync(
        //            new Sensor
        //            {
        //                Id = existID,
        //                PollingInterval = 10,
        //                Description = "Some description",
        //                Tag = "Some tag",
        //                MinValue = 10.00,
        //                MaxValue = 20.00,
        //                TimeStamp = DateTime.Now,
        //                Value = 15.00,
        //                Url = "Some URL",
        //                ModifiedOn = DateTime.Now,
        //            },
        //            new Sensor
        //            {
        //                Id = 2,
        //                PollingInterval = 10,
        //                Description = "Some description",
        //                Tag = "Some tag",
        //                MinValue = 10.00,
        //                MaxValue = 20.00,
        //                TimeStamp = DateTime.Now,
        //                Value = 15.00,
        //                Url = "Some URL",
        //                ModifiedOn = DateTime.Now,
        //            });
        //        await assertContext.SaveChangesAsync();
        //    }

        //    // Act and Assert
        //    using (var assertContext = new smartDormitoryDbContext(contextOptions))
        //    {
        //        var userSensorServiceMock = new UserSensorService(assertContext);

        //        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await userSensorServiceMock.AddSensorAsync(userId, sensorId, name, description, minValue, maxValue, pollingInterval, latitude, longitude, isPublic, alarm, imageUrl));
        //    }
        //}



    }
}
