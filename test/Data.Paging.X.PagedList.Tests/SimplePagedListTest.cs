using Olbrasoft.Data.Paging.X.PagedList;
using System;
using System.Collections.Generic;
using X.PagedList;
using Xunit;

namespace Data.Paging.X.PagedList.Tests
{
    public class SimplePagedListTest
    {
        [Fact]
        public void SimplePagedList_Of_Object_Inherit_From_BasePagedList_Of_Object()
        {
            //Arrange
            var type = typeof(BasePagedList<object>);

            //Act
            var list = new SimplePagedList<object>(new List<object>(), 1, 1, 1);

            //Assert
            Assert.IsAssignableFrom(type, list);
        }

        [Fact]
        public void Subset_ArgumentNullException()
        {
            //assertt
            var ex = Assert.Throws<ArgumentNullException>(() => new SimplePagedList<object>(null, 1, 1, 1));
        }
    }
}