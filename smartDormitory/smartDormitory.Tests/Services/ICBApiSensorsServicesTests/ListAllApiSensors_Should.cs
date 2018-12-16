﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using System;
using System.Linq;

namespace smartDormitory.Tests.Services.ICBApiSensorsServicesTests
{
    [TestClass]
    public class ListAllApiSensors_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        [DataRow(1, "ValidDescription", "ValidSensorId", 1.0, 1.0, "Valid Tag", "Valid Url", 1.0, 10)]

        public void Return_Successfully_AllApiSensors(int id, string description, string icbSensorId, double maxVal, double minVal, string tag, string url, double value, int pollingInterval)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "Return_Successfully_AllApiSensors")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                assertContext.MeasureTypes.Add(new MeasureType
                {
                    Id = 1,
                    Type = "C"
                });

                assertContext.SaveChanges();

                assertContext.Sensors.Add(new Sensor
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

                });

                assertContext.SaveChanges();
            }

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var mesureTypesServiceMock = new Mock<IMeasureTypesService>();
                var ICBApiSensorsServiceMock = new ICBApiSensorsService(assertContext, mesureTypesServiceMock.Object);

                //Act 
                var allSensors = ICBApiSensorsServiceMock.ListAllApiSensors().ToList();

                //Assert

                Assert.IsTrue(allSensors[0].Id == id);
                Assert.IsTrue(allSensors[0].Description == description);
                Assert.IsTrue(allSensors[0].Tag == tag);
                Assert.IsTrue(allSensors[0].MinValue == minVal);
                Assert.IsTrue(allSensors[0].MaxValue == maxVal);
                Assert.IsTrue(allSensors[0].Value == value);
                Assert.IsTrue(allSensors[0].PollingInterval == pollingInterval);
                Assert.IsTrue(allSensors[0].Url == url);
                Assert.IsTrue(allSensors[0].IcbSensorId == icbSensorId);
                Assert.IsTrue(allSensors[0].MeasureTypeId == 1);
            }
        }
    }
}
