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
        public static void SetupDbSet<T>(Mock<DbSet<T>> mockSet, IEnumerable<T> list) where T : class
        {
            var data = list.AsQueryable();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

       

        
    }
}
