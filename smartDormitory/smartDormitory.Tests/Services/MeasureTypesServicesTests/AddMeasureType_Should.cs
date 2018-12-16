using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.MeasureTypesServicesTests
{
    [TestClass]
    public class AddMeasureType_Should
    {
        private DbContextOptions<smartDormitoryDbContext> contextOptions;

        [TestMethod]
        public void ThrowArgumentNullException_WhenParameterTypeIsNull()
        {
            //Arrange
            var context = new Mock<smartDormitoryDbContext>();
            var measureTypeServiceMock = new MeasureTypesService(context.Object);

            //Act && Assert
            Assert.ThrowsException<ArgumentNullException>(() => measureTypeServiceMock.AddMeasureType(null));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("invalidParameterForType")]
        public void ThrowArgumentException_WhenTypeIsOutOfRange(string type)
        {
            //Arrange
            var context = new Mock<smartDormitoryDbContext>();
            var measureTypeServiceMock = new MeasureTypesService(context.Object);

            //Act && Assert
            Assert.ThrowsException<ArgumentException>(() => measureTypeServiceMock.AddMeasureType(type));
        }

        [TestMethod]
        public void ThrowArgumentException_WhenSensorExists()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenSensorExists")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                assertContext.MeasureTypes.Add(new MeasureType()
                {
                    Type = "Type"
                });
                assertContext.SaveChanges();
            }

            //Act && Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var measureTypeServiceMock = new MeasureTypesService(assertContext);
               
                Assert.ThrowsException<ArgumentException>(() => measureTypeServiceMock.AddMeasureType("Type"));
            }
        }

        [TestMethod]
        public async Task AddMeasureType_WhenParameterTypeIsCorrect()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenSensorExists")
                .Options;

            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                assertContext.MeasureTypes.Add(new MeasureType()
                {
                    Type = "Type"
                });

                assertContext.MeasureTypes.Add(new MeasureType()
                {
                    Type = "TestType"
                });

                assertContext.MeasureTypes.Add(new MeasureType()
                {
                    Type = "C"
                });
               await assertContext.SaveChangesAsync();
            }

            //Act && Assert
            using (var assertContext = new smartDormitoryDbContext(contextOptions))
            {
                var measureTypeServiceMock = new MeasureTypesService(assertContext);
                var newType = "F";

                measureTypeServiceMock.AddMeasureType(newType);

                var measureType = await assertContext.MeasureTypes
                    .FirstOrDefaultAsync(m => m.Type == newType);

                Assert.AreEqual(newType, measureType.Type); 
            }
        }


    }
}
