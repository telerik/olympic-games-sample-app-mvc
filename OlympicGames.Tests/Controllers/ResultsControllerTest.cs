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


    }
}
