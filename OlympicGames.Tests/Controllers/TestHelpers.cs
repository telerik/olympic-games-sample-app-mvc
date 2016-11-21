using Moq;
using OlympicGames.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicGames.Tests.Controllers
{
    static class TestHelpers
    { 
        public static Mock<OlympicsEntities> GetMockedContext(IQueryable<country> data)
        {
            //var data = data;

            var mockSet = new Mock<DbSet<country>>();
            mockSet.As<IQueryable<country>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<country>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<country>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<country>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<OlympicsEntities>();
            mockContext.Setup(m => m.countries).Returns(mockSet.Object);
            return mockContext;
        }

        public static Mock<OlympicsEntities> GetMockedContext(IQueryable<sport> data)
        {
            //var data = data;

            var mockSet = new Mock<DbSet<sport>>();
            mockSet.As<IQueryable<sport>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<sport>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<sport>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<sport>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<OlympicsEntities>();
            mockContext.Setup(m => m.sports).Returns(mockSet.Object);
            return mockContext;
        }
    }
}
