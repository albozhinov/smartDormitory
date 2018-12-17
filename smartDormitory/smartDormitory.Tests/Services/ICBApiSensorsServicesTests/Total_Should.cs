using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Tests.Services.ICBApiSensorsServicesTests
{
    [TestClass]
    public class Total_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(1, "ValidDescription", "ValidSensorId", 2.0, 2.0, "ValidTag", "ValidUrl", 1.0, 10)]

        public void Return_SensorTotalCount(int id, string description, string icbSensorId, double maxVal, double minVal, string tag, string url, double value, int pollingInterval)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
               .UseInMemoryDatabase(databaseName: "Return_SensorTotalCount")
               .Options;
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                assertContext.MeasureTypes.Add(new MeasureType
                {
                    Id = 1,
                    Type = "C"
                });

                assertContext.Sensors.AddRange(new Sensor
                {
                    Id = id,
                    Description = description,
                    IcbSensorId = icbSensorId,
                    MaxValue = maxVal,
                    MinValue = minVal,
                    Tag = tag,
                    Url = url,
                    Value = value,
                    TimeStamp = DateTime.Now,
                    PollingInterval = pollingInterval,
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1

                },
                new Sensor
                {
                    Id = 2,
                    Description = description,
                    IcbSensorId = icbSensorId,
                    MaxValue = maxVal,
                    MinValue = minVal,
                    Tag = tag,
                    Url = url,
                    Value = value,
                    TimeStamp = DateTime.Now,
                    PollingInterval = pollingInterval,
                    ModifiedOn = DateTime.Now,
                    MeasureTypeId = 1

                },

               new Sensor
               {
                   Id = 3,
                   Description = description,
                   IcbSensorId = icbSensorId,
                   MaxValue = maxVal,
                   MinValue = minVal,
                   Tag = "AnotherTag",
                   Url = url,
                   Value = value,
                   TimeStamp = DateTime.Now,
                   PollingInterval = pollingInterval,
                   ModifiedOn = DateTime.Now,
                   MeasureTypeId = 1

               });

                assertContext.SaveChanges();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var mesureTypesServiceMock = new Mock<IMeasureTypesService>();
                var ICBApiSensorsServiceMock = new ICBApiSensorsService(assertContext, mesureTypesServiceMock.Object);

                //Act 
                var count = ICBApiSensorsServiceMock.Total();

                //Assert
                Assert.IsTrue(count == 3);
            }
        }
    }
}
