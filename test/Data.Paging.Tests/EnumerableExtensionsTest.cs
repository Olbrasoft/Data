using Olbrasoft.Extensions.Paging;
using System.Collections.Generic;
using Xunit;

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
        public void AsPagedEnumerable_WithTotalCount_Return_IPagedEnumerable_WithSettingProperty_TotalCount()
        {
            //Arrange
            var data = new List<object>() { new object(), new object() };

            //Act
            var result = data.AsPagedEnumerable(3);

            //Assert
            Assert.True(result.TotalCount == 3);
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

        [Fact]
        public void AsPagedResult_WithTotalCount_Return_IPagedResult_WithSettingProperty_TotalCount()
        {
            //Arrange
            var data = new List<object>();

            //Act
            var result = data.AsPagedResult(3);

            //Assert
            Assert.True(result.TotalCount == 3);
        }

        [Fact]
        public void AsPagedResult_WithTotalCountAndFilteredCount_Return_IPagedResult_WithSettingPropertie_TotalCountAndFilteredCount()
        {
            //Arrange
            var data = new List<object>();

            //Act
            var result = data.AsPagedResult(3, 5);

            //Assert
            Assert.True(result.TotalCount == 3 && result.FilteredCount == 5);
        }
    }
}