using System.Collections.Generic;
using Xunit;
using Olbrasoft.Extensions.Paging;

namespace Olbrasoft.Data.Paging
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void AsPagedEnumerable_Return_IPagedEnumerable()
        {
            //Arrange
            var data = new List<object>();

            //Act
            var result = data.AsPagedEnumerable();

            //Asserta
            Assert.IsAssignableFrom<IEnumerable<object>>(result);
        }

        [Fact]
        public void AsPagedResult_Return_IpagedResult()
        {
            //Arrange
            var data = new List<object>();

            //Act
            var result = data.AsPagedResult();

            //Assert
            Assert.IsAssignableFrom<IPagedResult<object>>(result);
        }
    }
}