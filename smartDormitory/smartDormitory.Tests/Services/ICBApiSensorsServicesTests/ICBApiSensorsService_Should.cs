using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using smartDormitory.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Tests.Services.ICBApiSensorsServicesTests
{
    [TestClass]
    public class ICBApiSensorsService_Should
    {
        [TestMethod]
        public void SuccesfullyAddActorToDatabase()
        {
           /* var contextOptions = new DbContextOptionsBuilder<smartDormitoryDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnActorAddedToDatabase_SuccessMessage")
                .Options;

            var serviceValidator = new Mock<IServiceValidator>();
            var userSessionMock = new Mock<IUserSession>();
            userSessionMock.Setup(s => s.IsLoggedIn()).Returns(true);
            userSessionMock.Setup(s => s.IsAdmin()).Returns(true);

            var firstName = "John";
            var lastName = "Doe";
            var age = 29;


            using (var arrangeContext = new OnlineMovieStoreDbContext(contextOptions))
            {
                var unitOfWork = new UnitOfWork(arrangeContext);
                var actorService = new ActorsService(unitOfWork, serviceValidator.Object, userSessionMock.Object);
                actorService.AddActor(firstName, lastName, age);
            }

            using (var assertContext = new OnlineMovieStoreDbContext(contextOptions))
            {
                var unitOfWork = new UnitOfWork(assertContext);
                var movieService = new MoviesService(unitOfWork, serviceValidator.Object, userSessionMock.Object);

                Assert.AreEqual(1, unitOfWork.GetRepo<Actor>().All().Count());
            }*/
        }
        
    }
}
