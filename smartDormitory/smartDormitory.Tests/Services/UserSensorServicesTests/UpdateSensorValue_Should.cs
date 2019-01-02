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
    public class UpdateSensorValue_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(null)]
        public async Task ThrowArgumentException_WhenArgumentIsIncorrect(string id)
        {
            // Arrange
            var dbContextStub = new Mock<smartDormitoryDbContext>();
            var userSensorServiceMock = new UserSensorService(dbContextStub.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await userSensorServiceMock.UpdateSensorValue(id));
        }

        [TestMethod]
        [DataRow("1")]
        public async Task ReturnSensor_WhenArgumentIsCorrect(string id)
        {
            // Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnSensor_WhenArgumentIsCorrect")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {

                var userId = Guid.NewGuid().ToString();
                await assertContext.Sensors.AddAsync(
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
                     });
                await assertContext.SaveChangesAsync();
            }
                        
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var userSensorServiceMock = new UserSensorService(assertContext);

                // Act
                var updatedSensor = await userSensorServiceMock.UpdateSensorValue(id);
                var contextSensor = await assertContext.Sensors.FirstOrDefaultAsync(s => s.IcbSensorId == id);

                // Assert
                Assert.AreSame(updatedSensor, contextSensor);
            }
        }

    }
}
