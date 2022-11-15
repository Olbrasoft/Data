using Olbrasoft.Data;
using Olbrasoft.Data.Sorting;
using Olbrasoft.Extensions.Linq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Data.Sorting.Extensions.Tests
{
    public class QueryableExtensionsTest
    {
        [Fact]
        public void SortingDefault()
        {
            //Arrange
            var queryable = CreateQueryable();

            var first = queryable.OrderBy("Text").First();

            //Assert
            Assert.True(first.Text == "aaa");
        }

        [Fact]
        public void SortingAscending()
        {
            var queryable = CreateQueryable();

            //Act
            var first = queryable.OrderBy("Text", OrderDirection.Asc).First();

            //Assert
            Assert.True(first.Text == "aaa");
        }

        [Fact]
        public void SortingDescending()
        {
            var queryable = CreateQueryable();

            //Act
            var first = queryable.OrderBy("Text", OrderDirection.Desc).First();

            //Assert
            Assert.True(first.Text == "ccc");
        }

        private static IQueryable<AwesomeDto> CreateQueryable()
        {
            //Arrange
            return new List<AwesomeDto>() { new AwesomeDto { Text = "bbb" }, new AwesomeDto { Text = "ccc" }, new AwesomeDto { Text = "aaa" } }.AsQueryable();
        }
    }
}