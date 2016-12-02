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
    /// Tests for ResultsControllerTest
    /// </summary>
    [TestClass]
    public class ResultsControllerTest
    {
        private Mock<OlympicsEntities> mockContext;
        private Mock<DbSet<result>> mockSet;
        private readonly List<result> data = new List<result>
        {
            new result() {athlete=1, country=1, @event=1,game=1,id=1, medal=1 }
        };

        [TestInitialize()]
        public void Initialize()
        {
            this.mockContext = new Mock<OlympicsEntities>();
            this.mockSet = new Mock<DbSet<result>>();
            this.mockContext.Setup(m => m.results).Returns(mockSet.Object);
        }

        [TestMethod]
        public void Get_Should_Return_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            ResultsController controller = new ResultsController(this.mockContext.Object);

            IQueryable<result> result = controller.GetResults();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Zero_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, new List<result>());

            ResultsController controller = new ResultsController(this.mockContext.Object);

            IQueryable<result> result = controller.GetResults();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Sport_By_ID_Should_Return_Sport()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            ResultsController controller = new ResultsController(this.mockContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetResult(data.First().id);

            var result = response as OkNegotiatedContentResult<result>;
            var coutryResult = result.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(coutryResult);
            Assert.AreEqual(data.First(), coutryResult);
        }

        [TestMethod]
        public void Get_MedalsByCountry_Should_Return_Combined_Data()
        {
            TestHelpers.SetupDbSet(this.mockSet, new List<result>
            {
                new result() {athlete=1, country=1, @event=1,game=1,id=1, medal=1 },
                new result() {athlete=2, country=2, @event=1,game=2,id=2, medal=1 },
                new result() {athlete=3, country=2, @event=1,game=1,id=3, medal=1 },
                new result() {athlete=4, country=1, @event=1,game=2,id=4, medal=1 }
            });
           
            Mock<DbSet<country>> mockCountriesSet = new Mock<DbSet<country>>();

            var coutriesData = new List<country>
            {
                new country() { name = "US", abbr = "abbr1", id = 1 },
                new country() { name = "BG", abbr = "abbr1", id = 2 }
            };

            TestHelpers.SetupDbSet(mockCountriesSet, coutriesData);
            this.mockContext.Setup(m => m.countries).Returns(mockCountriesSet.Object);

            Mock<DbSet<game>> mockGamesSet = new Mock<DbSet<game>>();

            var gamesData = new List<game>
            {
                new game() {city="city", country=1, id=1,name="name",year=1 },
                new game() {city="city2", country=1, id=2,name="name2",year=2 }
            };

            TestHelpers.SetupDbSet(mockGamesSet, gamesData);
            this.mockContext.Setup(m => m.games).Returns(mockGamesSet.Object);

            Mock<DbSet<medal>> mockMedalSet = new Mock<DbSet<medal>>();


            TestHelpers.SetupDbSet(mockMedalSet, new List<medal>
            {
                new medal() { id = 1, name = "medal" }
            });
            this.mockContext.Setup(m => m.medals).Returns(mockMedalSet.Object);

            ResultsController controller = new ResultsController(this.mockContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetMedalsByCountry(new int[] { 1, 2 }, 1, 3);

            var result = response as OkNegotiatedContentResult<IQueryable<MedalsByCountry>>;
            var objectResult = result.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.Count(), 4);
            Assert.AreEqual(objectResult.First().Country, coutriesData.First().name);
            Assert.AreEqual(objectResult.First().Medals, 1);
            Assert.AreEqual(objectResult.First().Year, gamesData.First().year);
        }


    }
}
