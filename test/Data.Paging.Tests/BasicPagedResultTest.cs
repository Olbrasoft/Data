using System;
using System.Collections.Generic;
using Xunit;

namespace Olbrasoft.Data.Paging
{
    public class BasicPagedResultTest
    {
        [Fact]
        public void Instance_Implement_Interface_IBasicPagedResult()
        {
            //Arrange
            var type = typeof(IBasicPagedResult<object>);

            //Act
            var result = new BasicPagedResult<object>(new List<object>());

            //Assert
            Assert.IsAssignableFrom(type, result);
        }

        [Fact]
        public void Instance_Inherit_From_List()
        {
            //Arrange
            var type = typeof(List<object>);

            //Act
            var result = new BasicPagedResult<object>(new List<object>());

            //Assert
            Assert.IsAssignableFrom(type, result);
        }

        [Fact]
        public void Have_TotalCount_Type_Of_Integer()
        {
            //Arrange
            var result = new BasicPagedResult<object>(new List<object>());

            //Act
            var totalCount = result.TotalCount;

            //Assert
            Assert.IsAssignableFrom<int>(totalCount);
        }

        [Fact]
        public void Auto_Set_TotalCount_If_Count_Is_Bigger_Than_TotalCount()
        {
            //Arrange
            var result = new BasicPagedResult<object>(new List<object> { new object() });

            //Act
            var totalCount = result.TotalCount;

            //Assert
            Assert.True(totalCount == 1);
        }

        [Fact]
        public void Exception_When_Entering_The_TotalCount_Less_Than_The_Count()
        {
            //Arrange
            var result = new BasicPagedResult<object>(new List<object> { new object() });

            //Act and Assert
            Assert.Throws<Exception>(() => result.TotalCount = 0);
        }

        [Fact]
        public void Set_TotalCount()
        {
            //Arrange
            var result = new BasicPagedResult<object>(new List<object> { new object() });

            //Act
            result.TotalCount = 5;

            //Assert
            Assert.True(result.TotalCount == 5);
        }
    }
}