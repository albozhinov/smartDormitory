using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data;
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

        [TestMethod]
        [DataRow("Tag123")]
        public async Task GetSensorsTypeMinMaxValues_WhenArgumentIsCorrect(string tag)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "GetSensorsTypeMinMaxValues_WhenArgumentIsCorrect")
                .Options;
            const double minValue = 5.00;
            const double maxValue = 15.00;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                await assertContext.Sensors.AddRangeAsync(
                    new Sensor
                    {
                        Id = 1,
                        PollingInterval = 10,
                        Description = "Some description",
                        Tag = tag,
                        MinValue = minValue,
                        MaxValue = maxValue,
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
                var minMax = new List<double>(await userSensorServiceMock.GetSensorsTypeMinMaxValues(tag));
                var min = minMax[0];
                var max = minMax[1];

                // Assert
                Assert.IsTrue(min == minValue);
                Assert.IsTrue(max == maxValue);
            }
        }
    }
}
