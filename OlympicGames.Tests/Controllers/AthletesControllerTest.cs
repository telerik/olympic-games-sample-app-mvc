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
    /// Tests for AthletesControllerTest
    /// </summary>
    [TestClass]
    public class AthletesControllerTest
    {
        private Mock<OlympicsEntities> mockContext;
        private Mock<DbSet<athlete>> mockSet;
        private readonly List<athlete> data = new List<athlete>
        {
            new athlete() { birthday=DateTime.Now, country=1, firstName="name", id=1,
                lastName ="name",sport=1,middleName="name", url="url" }
        };

        [TestInitialize()]
        public void Initialize()
        {
            this.mockContext = new Mock<OlympicsEntities>();
            this.mockSet = new Mock<DbSet<athlete>>();
            this.mockContext.Setup(m => m.athletes).Returns(mockSet.Object);
        }

        [TestMethod]
        public void Get_Should_Return_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            AthletesController controller = new AthletesController(this.mockContext.Object);

            IQueryable<athlete> athlete = controller.GetAthletes();

            Assert.IsNotNull(athlete);
            Assert.AreNotEqual(0, athlete.Count());
        }

        [TestMethod]
        public void Get_Zero_Items()
        {
            TestHelpers.SetupDbSet(this.mockSet, new List<athlete>());

            AthletesController controller = new AthletesController(this.mockContext.Object);

            IQueryable<athlete> athlete = controller.GetAthletes();

            Assert.IsNotNull(athlete);
            Assert.AreEqual(0, athlete.Count());
        }

        [TestMethod]
        public void Get_Sport_By_ID_Should_Return_Sport()
        {
            TestHelpers.SetupDbSet(this.mockSet, this.data);

            AthletesController controller = new AthletesController(this.mockContext.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult response = controller.GetAthlete(data.First().id);

            var athlete = response as OkNegotiatedContentResult<athlete>;
            var coutryResult = athlete.Content;

            Assert.IsNotNull(response);
            Assert.IsNotNull(coutryResult);
            Assert.AreEqual(data.First(), coutryResult);
        }


    }
}
