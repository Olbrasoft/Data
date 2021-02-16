using System.Collections.Generic;
using Xunit;

namespace Olbrasoft.Data.Paging
{
    public class PagedResultTest
    {
        [Fact]
        public void Instance_Implement_Interface_IPagedResult()
        {
            //Arrange
            var type = typeof(IPagedResult<object>);

            //Act
            var result = new PagedResult<object>(new List<object>());

            //Assert
            Assert.IsAssignableFrom(type, result);
        }

        [Fact]
        public void Instance_Inherit_From_BasicPagedResult()
        {
            //Arrange
            var type = typeof(BasicPagedResult<object>);

            //Act
            var result = new PagedResult<object>(new List<object>());

            //Assert
            Assert.IsAssignableFrom(type, result);
        }

        [Fact]
        public void Have_FilteredCount_Of_Type_Integer()
        {
            //Arrange
            var result = new PagedResult<object>(new List<object>());

            //Act
            var filteredCount = result.FilteredCount;

            //Assert
            Assert.IsAssignableFrom<int>(filteredCount);
        }
    }
}