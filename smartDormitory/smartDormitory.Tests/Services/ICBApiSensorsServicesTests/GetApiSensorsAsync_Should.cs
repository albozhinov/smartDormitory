using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Data.DTOModels;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.ICBApiSensorsServicesTests
{
    [TestClass]
    public class GetApiSensorsAsync_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;
        [TestMethod]
        public async Task Return_SensorDTOModelIEnumerable()
        {
            //Arrange
            var contextMock = new Mock<smartDormitoryDbContext>();
            var measureTypesServiceMock = new Mock<IMeasureTypesService>();
            var ICBApiSensorsServiceMock = new ICBApiSensorsService(contextMock.Object, measureTypesServiceMock.Object);

            //Act
            var sensors = await ICBApiSensorsServiceMock.GetApiSensorsAsync();

            //Act
            Assert.IsInstanceOfType(sensors, typeof(IEnumerable<SensorDTOModel>));
        }
    }
}
