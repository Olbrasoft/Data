using Olbrasoft.Extensions.Data;
using System;
using Xunit;

namespace Olbrasoft.Data.Paging
{
    public class PageInfoExtensionsTest
    {
        [Fact]
        public void Exception_When_PageInfo_Is_Null()
        {
            //Arrange
            IPageInfo info = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => info.CalculateSkip());
        }

        [Fact]
        public void CalculateSkip()
        {
            //Arrange
            var info = new PageInfo(10, 2);

            //Act
            var skip = info.CalculateSkip();

            //Assert
            Assert.True(skip == 10);
        }
    }
}