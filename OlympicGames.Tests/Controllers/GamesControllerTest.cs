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
    /// Tests for GamesControllerTest
    /// </summary>
    [TestClass]
    public class GamesControllerTest
    {
        private Mock<OlympicsEntities> mockContext;
        private Mock<DbSet<game>> mockSet;
        private readonly List<game> data = new List<game>
        {
            new game() {city="city", country=1, id=1,name="name",year=12 }
        };

        [TestInitialize()]
        public void Initialize()
        {
            this.mockContext = new Mock<OlympicsEntities>();
            this.mockSet = new Mock<DbSet<game>>();
            this.mockContext.Setup(m => m.games).Returns(mockSet.Object);
        }

        [TestMethod]
        public void Get_Should_Return_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            GamesController controller = new GamesController(this.mockContext.Object);

            IQueryable<game> result = controller.GetGames();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Zero_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, new List<game>());

            GamesController controller = new GamesController(this.mockContext.Object);

            IQueryable<game> result = controller.GetGames();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Game_By_ID_Should_Return_Game()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            GamesController controller = new GamesController(this.mockContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetGame(data.First().id);

            var result = response as OkNegotiatedContentResult<game>;
            var coutryResult = result.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(coutryResult);
            Assert.AreEqual(data.First(), coutryResult);
        }


    }
}
