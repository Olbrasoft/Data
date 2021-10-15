using Olbrasoft.Data;
using System;
using Olbrasoft.Data.Sorting;
using Xunit;

namespace Data.Sorting.Tests
{
    public class OrderDirectionTest
    {
        [Theory]
        [InlineData(OrderDirection.Asc)]
        [InlineData(OrderDirection.Desc)]
        public void Is_Enum(OrderDirection order)
        {
            Assert.True((Convert.ToInt32(order) > -1));
        }
    }
}