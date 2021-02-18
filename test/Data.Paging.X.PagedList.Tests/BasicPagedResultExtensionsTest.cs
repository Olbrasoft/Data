using Moq;
using Olbrasoft.Data.Paging;
using Olbrasoft.Data.Paging.X.PagedList;
using System;
using X.PagedList;
using Xunit;

namespace Data.Paging.X.PagedList.Tests
{
    public class BasicPagedResultExtensionsTest
    {
        [Fact]
        public void AsPagedList_Return_IPagedList()
        {
            //Arrange
            var resultMock = new Mock<IBasicPagedResult<object>>();

            //Act
            var list = resultMock.Object.AsPagedList(new PageInfo());

            //Assert
            Assert.IsAssignableFrom<IPagedList<object>>(list);
        }

        [Fact]
        public void Source_ArgumentNullException()
        {
            //Arrange
            IBasicPagedResult<object> result = null;
            var expectedParameterName = "source";

            try //Act
            {
                result.AsPagedList(new PageInfo());
            }
            catch (ArgumentException e) //Assert
            {
                Assert.Equal(expectedParameterName, e.ParamName);

                Assert.IsAssignableFrom<ArgumentNullException>(e);
            }
        }

        [Fact]
        public void Paging_ArgumentNullException()
        {
            //Arrange
            var resultMock = new Mock<IBasicPagedResult<object>>();
            var expectedParameterName = "paging";

            try //Act
            {
                resultMock.Object.AsPagedList(null);
            }
            catch (ArgumentException e) //Assert
            {
                Assert.Equal(expectedParameterName, e.ParamName);
                Assert.IsAssignableFrom<ArgumentNullException>(e);
            }
        }
    }
}