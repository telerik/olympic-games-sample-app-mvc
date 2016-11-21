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
    /// Tests for CountriesControllerTest
    /// </summary>
    [TestClass]
    public class CountriesControllerTest
    {
        private Mock<OlympicsEntities> mockContext;
        private Mock<DbSet<country>> mockSet;
        private readonly List<country> data = new List<country>
        {
            new country() { name = "name1", abbr = "abbr1", id = 1 }
        };

        [TestInitialize()]
        public void Initialize()
        {
            this.mockContext = new Mock<OlympicsEntities>();
            this.mockSet = new Mock<DbSet<country>>();
            this.mockContext.Setup(m => m.countries).Returns(mockSet.Object);
        }

        [TestMethod]
        public void Get_Should_Return_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            CountriesController controller = new CountriesController(this.mockContext.Object);

            IQueryable<country> result = controller.GetCountries();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Zero_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, new List<country>());

            CountriesController controller = new CountriesController(this.mockContext.Object);

            IQueryable<country> result = controller.GetCountries();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Country_By_ID_Should_Return_Country()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            CountriesController controller = new CountriesController(this.mockContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetCountry(data.First().id);

            var result = response as OkNegotiatedContentResult<country>;
            var coutryResult = result.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(coutryResult);
            Assert.AreEqual(data.First(), coutryResult);
        }


    }
}
