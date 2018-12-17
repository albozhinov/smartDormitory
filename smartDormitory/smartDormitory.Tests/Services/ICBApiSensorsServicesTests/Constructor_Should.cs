using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Tests.Services.ICBApiSensorsServicesTests
{
    [TestClass]
    public class Constructor_Should
    {

        [TestMethod]
        public void ThrowArgumentException_WhenParameterContextIsNull()
        {
            var mesureTypesService = new Mock<IMeasureTypesService>();

            Assert.ThrowsException<ArgumentNullException>(() => new ICBApiSensorsService(null, mesureTypesService.Object));
        }

        [TestMethod]
        public void ThrowArgumentException_WhenParameterMesureTypesServiceIsNull()
        {
            var context = new Mock<smartDormitoryDbContext>();

            Assert.ThrowsException<ArgumentNullException>(() => new ICBApiSensorsService(context.Object, null));
        }

        [TestMethod]
        public void CreateInstance_When_CorrectParametersArePassesd()
        {
            var context = new Mock<smartDormitoryDbContext>();
            var mesureTypesService = new Mock<IMeasureTypesService>();

            Assert.IsInstanceOfType(new ICBApiSensorsService(context.Object, mesureTypesService.Object), typeof(ICBApiSensorsService));
        }
    }
}
