using Olbrasoft.Data.Paging;
using Olbrasoft.Data.Paging.X.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using Xunit;

namespace Data.Paging.X.PagedList.Tests
{
    public class QueryableExtensionsTest
    {
        [Fact]
        public void AsPagedList_Source_ArgumentNullException()
        {
            //Arrange
            IQueryable<object> source = null;
            var expectedParameterName = "source";

            //Act
            try
            {
                source.AsPagedList(new PageInfo());
            }
            catch (ArgumentException e) //Assert
            {
                Assert.Equal(expectedParameterName, e.ParamName);

                Assert.IsAssignableFrom<ArgumentNullException>(e);
            }
        }

        [Fact]
        public void AsPagedList_Paging_ArgumentNullException()
        {
            //Arrange
            IQueryable<object> source = new List<object>().AsQueryable();
            var expectedParameterName = "paging";

            //Act
            try
            {
                source.AsPagedList(null);
            }
            catch (ArgumentException e) //Assert
            {
                Assert.Equal(expectedParameterName, e.ParamName);

                Assert.IsAssignableFrom<ArgumentNullException>(e);
            }
        }

        [Fact]
        public void AsPagedList_Return_IPagedList_Of()
        {
            //Arrange
            var type = typeof(IPagedList<object>);
            IQueryable<object> source = new List<object>().AsQueryable();

            //Act
            var list = source.AsPagedList(new PageInfo());

            //Assert
            Assert.IsAssignableFrom(type, list);
        }
    }
}