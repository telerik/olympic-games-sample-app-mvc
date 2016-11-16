using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlympicGames.Models;
using System.Linq;
using OlympicGames.Controllers;
using Moq;
using System.Data.Entity;

namespace OlympicGames.Tests.Controllers
{
    /// <summary>
    /// Tests for CountriesControllerTest
    /// </summary>
    [TestClass]
    public class CountriesControllerTest
    {

        [TestMethod]
        public void Get()
        {
            Mock<OlympicsEntities> mockedContext = GetMockedContext();

            CountriesController controller = new CountriesController(mockedContext.Object);

            IQueryable<country> result = controller.Getcountries();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        private static Mock<OlympicsEntities> GetMockedContext()
        {
            var data = new List<country>
            {
                new country() { name = "name1", abbr = "abbr1", id = 1 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<country>>();
            mockSet.As<IQueryable<country>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<country>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<country>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<country>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<OlympicsEntities>();
            mockContext.Setup(m => m.countries).Returns(mockSet.Object);
            return mockContext;
        }
    }
}
