using Microsoft.VisualStudio.TestTools.UnitTesting;
using smartDormitory.Data;
using smartDormitory.Data.Models;
using smartDormitory.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartDormitory.Tests.Services.MailServiceTests
{
    [TestClass]
    public class SendMail_Should
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenUserSensorsIsNull()
        {
            // Arrange
            var mailServiceMock = new MailService();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await mailServiceMock.SendEmail(null, "validUsername", "ValidEmail"));
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenUsernameIsNull()
        {
            // Arrange
            var mailServiceMock = new MailService();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await mailServiceMock.SendEmail(new List<UserSensors>(), null, "ValidEmail"));
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenEmailIsNull()
        {
            // Arrange
            var mailServiceMock = new MailService();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await mailServiceMock.SendEmail(new List<UserSensors>(), "validUsername", null));
        }

        [TestMethod]
        [DataRow("validUsername", "gosho.testov@gmail.com")]
        public async Task Return_True_WhenParametersAreValid(string username, string email)
        {
            //Arrange
            var userSensors = new List<UserSensors> { new UserSensors
            {
                Name = "TestName",
                Alarm = true,
                IsDeleted = false,
                MinValue = 1,
                MaxValue = 2,
                Sensor = new Sensor{Value = 3},
            },
            new UserSensors
            {
                Name = "AnotherSensor",
                Alarm = true,
                IsDeleted = false,
                MinValue = 2,
                MaxValue = 3,
                Sensor = new Sensor{Value = 4},
            }
            };

            var mailService = new MailService();

            //Act
            var isMailSended = await mailService.SendEmail(userSensors, username, email);

            //Assert
            Assert.IsTrue(isMailSended == true);
        }

        [TestMethod]
        [DataRow("validUsername", "gosho.testov@gmail.com")]
        public async Task Return_False_WhenUserSensorsIsEmpty(string username, string email)
        {
            //Arrange
            var userSensors = new List<UserSensors>();
            
            var mailService = new MailService();

            //Act
            var isMailSended = await mailService.SendEmail(userSensors, username, email);

            //Assert
            Assert.IsTrue(isMailSended == false);
        }




    }
}
