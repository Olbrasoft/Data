using Moq;
using Olbrasoft.Data.Paging;
using Olbrasoft.Data.Paging.X.PagedList;
using Olbrasoft.Extensions.Paging;
using System;
using System.Collections.Generic;
using X.PagedList;
using Xunit;

namespace Data.Paging.X.PagedList.Tests
{
    public class PagedEnumarableExtensionsTest
    {
        [Fact]
        public void AsPagedList_Return_IPagedList()
        {
            //Arrange
            var pagedEnumerable = new List<object>().AsPagedEnumerable();

            //Act
            var list = pagedEnumerable.AsPagedList(new PageInfo());

            //Assert
            Assert.IsAssignableFrom<IPagedList<object>>(list);
        }

        [Fact]
        public void Source_ArgumentNullException()
        {
            //Arrange
            IPagedEnumerable<object> result = null;
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
            var resultMock = new Mock<IPagedEnumerable<object>>();
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