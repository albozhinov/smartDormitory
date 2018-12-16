using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Tests.Services.MeasureTypesServicesTests
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowArgumentException_WhenParameterContextIsNull()
        {
            //Arrange && Act && Assert
            Assert.ThrowsException<ArgumentNullException>(() => new MeasureTypesService(null));
        }

        [TestMethod]
        public void CreateInstance_When_CorrectParameterIsPassesd()
        {
            //Arrange
            var context = new Mock<smartDormitoryDbContext>();

            //Act && Assert
            Assert.IsInstanceOfType(new MeasureTypesService(context.Object), typeof(MeasureTypesService));
        }
    }
}
