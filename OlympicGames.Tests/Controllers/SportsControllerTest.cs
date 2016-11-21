using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlympicGames.Models;
using System.Linq;
using OlympicGames.WebApiControllers;
using Moq;
using System.Data.Entity;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;

namespace OlympicGames.Tests.Controllers
{
    /// <summary>
    /// Tests for SportsControllerTest
    /// </summary>
    [TestClass]
    public class SportsControllerTest
    {

        [TestMethod]
        public void Get_Should_Return_Items()
        {
            var data = new List<sport>
            {
                new sport() { abbr="abbr", category=1, id=1, name="name" }
            }.AsQueryable();

            Mock<OlympicsEntities> mockedContext = TestHelpers.GetMockedContext(data);

            SportsController controller = new SportsController(mockedContext.Object);

            IQueryable<sport> result = controller.GetSports();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Zero_Items()
        {
            var data = new List<sport>
            {
            }.AsQueryable();

            Mock<OlympicsEntities> mockedContext = TestHelpers.GetMockedContext(data);

            SportsController controller = new SportsController(mockedContext.Object);

            IQueryable<sport> result = controller.GetSports();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_sport_By_ID_Should_Return_sport()
        {
            var data = new List<sport>
            {
                new sport() { abbr="abbr", category=1, id=1, name="name" }
            }.AsQueryable();

            Mock<OlympicsEntities> mockedContext = TestHelpers.GetMockedContext(data);

            SportsController controller = new SportsController(mockedContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetSport(data.First().id);

            var result = response as OkNegotiatedContentResult<sport>;
            var coutryResult = result.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(coutryResult);
            Assert.AreEqual(data.First(), coutryResult);
        }


    }
}
