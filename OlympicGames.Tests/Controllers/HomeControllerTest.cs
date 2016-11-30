using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlympicGames;
using OlympicGames.Controllers;

namespace OlympicGames.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Olympic Games - Medals by Country", result.ViewBag.Title);
        }

        [TestMethod]
        public void Medals_by_Country()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Medals_by_Country() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Olympic Games - Medals by Country", result.ViewBag.Title);
        }

        [TestMethod]
        public void Medals_by_Sport()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Medals_by_Sport() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Olympic Games - Medals by Sport", result.ViewBag.Title);
        }

        [TestMethod]
        public void Top_Results_by_Sport()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Top_Results_by_Sport() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Olympic Games - Top Results by Sport", result.ViewBag.Title);
        }

        [TestMethod]
        public void Athlete_Comparison()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Athlete_Comparison() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Olympic Games - Athlete Comparison", result.ViewBag.Title);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Olympic Games - About", result.ViewBag.Title);
        }

        [TestMethod]
        public void Error()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
