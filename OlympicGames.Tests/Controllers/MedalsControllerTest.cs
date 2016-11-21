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
    /// Tests for MedalsControllerTest
    /// </summary>
    [TestClass]
    public class MedalsControllerTest
    {
        private Mock<OlympicsEntities> mockContext;
        private Mock<DbSet<medal>> mockSet;
        private readonly List<medal> data = new List<medal>
        {
            new medal() { id = 1, name = "medal" }
        };

        [TestInitialize()]
        public void Initialize()
        {
            this.mockContext = new Mock<OlympicsEntities>();
            this.mockSet = new Mock<DbSet<medal>>();
            this.mockContext.Setup(m => m.medals).Returns(mockSet.Object);
        }

        [TestMethod]
        public void Get_Should_Return_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            MedalsController controller = new MedalsController(this.mockContext.Object);

            IQueryable<medal> result = controller.GetMedals();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }



        [TestMethod]
        public void Get_Zero_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, new List<medal>());

            MedalsController controller = new MedalsController(this.mockContext.Object);

            IQueryable<medal> result = controller.GetMedals();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_Medal_By_ID_Should_Return_Medal()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            MedalsController controller = new MedalsController(this.mockContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetMedal(data.First().id);

            var result = response as OkNegotiatedContentResult<medal>;
            var coutryResult = result.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(coutryResult);
            Assert.AreEqual(data.First(), coutryResult);
        }
    }
}
