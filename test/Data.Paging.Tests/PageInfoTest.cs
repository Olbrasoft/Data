using Xunit;

namespace Olbrasoft.Data.Paging
{
    public class PageInfoTest
    {
        [Fact]
        public void Instance_Implement_Interface_IPageInfo()
        {
            //Arrange
            var type = typeof(IPageInfo);

            //Act
            var info = new PageInfo();

            //Assert
            Assert.IsAssignableFrom(type, info);
        }

        [Fact]
        public void Have_PageSize_TypeOf_Integer()
        {
            //Arrange
            var info = new PageInfo();

            //Act
            var size = info.PageSize;

            //Assert
            Assert.IsAssignableFrom<int>(size);
        }

        [Fact]
        public void Have_NumberOfSelectedPage_Type_Of_Integer()
        {
            //Arrange
            var info = new PageInfo();

            //Act
            var page = info.NumberOfSelectedPage;

            //Assert
            Assert.IsAssignableFrom<int>(page);
        }
    }
}