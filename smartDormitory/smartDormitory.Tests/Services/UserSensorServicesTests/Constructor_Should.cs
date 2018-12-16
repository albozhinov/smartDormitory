using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Tests.Services.UserSensorServicesTests
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void CreateInstance_When_CorrectParametersArePassed()
        {
            var context = new Mock<smartDormitoryDbContext>();

            Assert.IsInstanceOfType(new UserSensorService(context.Object), typeof(UserSensorService));
        }

        [TestMethod]
        public void ThrowArgumentNullException_When_IncorrectArgumentPassed()
        {
            smartDormitoryDbContext context = null;

            Assert.ThrowsException<ArgumentNullException>(() => new UserSensorService(context));
        }
    }
}
