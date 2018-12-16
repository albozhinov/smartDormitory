using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Data.DTOModels;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.ICBApiSensorsServicesTests
{
    [TestClass]
    public class GetApiSensorByIdAsync_Should
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenParameterSensorIsNull()
        {
            var contextMock = new Mock<smartDormitoryDbContext>();
            var mesureTypesServiceMock = new Mock<IMeasureTypesService>();
            var ICBApiSensorsServiceMock = new ICBApiSensorsService(contextMock.Object, mesureTypesServiceMock.Object);

            string id = "sensorId";

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await ICBApiSensorsServiceMock.GetApiSensorByIdAsync(null, id));
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenParameterIdIsNull()
        {
            var contextMock = new Mock<smartDormitoryDbContext>();
            var mesureTypesServiceMock = new Mock<IMeasureTypesService>();
            var ICBApiSensorsServiceMock = new ICBApiSensorsService(contextMock.Object, mesureTypesServiceMock.Object);
            var sensor = new Mock<SensorDTOModel>();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await ICBApiSensorsServiceMock.GetApiSensorByIdAsync(sensor.Object, null));
        }

        [TestMethod]
        public async Task GetApiSensorByIdAsync_WhenIdIsNotCorrect()
        {
            var contextMock = new Mock<smartDormitoryDbContext>();
            var mesureTypesServiceMock = new Mock<IMeasureTypesService>();
            var ICBApiSensorsServiceMock = new ICBApiSensorsService(contextMock.Object, mesureTypesServiceMock.Object);
            var sen = new Mock<SensorDTOModel>();
            string id = "invalidId";

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await ICBApiSensorsServiceMock.GetApiSensorByIdAsync(sen.Object, id));

        }

        [TestMethod]
        public async Task GetApiSensorByIdAsync_WhenArgumentsAreCorrect()
        {
            //Arrange
            var contextMock = new Mock<smartDormitoryDbContext>();
            var mesureTypesServiceMock = new Mock<IMeasureTypesService>();
            var ICBApiSensorsServiceMock = new ICBApiSensorsService(contextMock.Object, mesureTypesServiceMock.Object);
            var sen = new Mock<SensorDTOModel>();
            //Valid sensor Id
            string id = "f1796a28-642e-401f-8129-fd7465417061";

            //Act
            var sensor = await ICBApiSensorsServiceMock.GetApiSensorByIdAsync(sen.Object, id);

            //Assert
            Assert.IsInstanceOfType(sensor, typeof(SensorDTOModel));
            Assert.IsTrue(sensor.ValueType.Equals("°C"));
        }
    }
}
