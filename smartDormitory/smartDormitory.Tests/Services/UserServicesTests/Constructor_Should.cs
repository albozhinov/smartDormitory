using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using smartDormitory.Data.Context;
using smartDormitory.Services;
using System;

namespace smartDormitory.Tests.Services.UserServicesTests
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowArgumentException_WhenParameterContextIsNull()
        {
            //Arrange && Act && Assert
            Assert.ThrowsException<ArgumentNullException>(() => new UserService(null));
        }

        [TestMethod]
        public void CreateInstance_When_CorrectParametersArePassesd()
        {
            //Arrange
            var context = new Mock<smartDormitoryDbContext>();

            //Act && Assert
            Assert.IsInstanceOfType(new UserService(context.Object), typeof(UserService));
        }

    }
}
