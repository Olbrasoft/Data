using Olbrasoft.Data.Sorting;

namespace Olbrasoft.Data.Cqrs.Queries;

public class DataTablePagedQueryTest : BaseTest
{
    [Fact]
    public void DataTablesPagedQuery_Inherit_From_PagedQuery()
    {
        //Arrange
        var type = typeof(PagedQuery<object>);

        //Act
        var query = new DataTablesPagedQuery<object>(DispatcherMock.Object);

        //Assert
        Assert.IsAssignableFrom(type, query);
    }

    [Fact]
    public void DataTablesPagedQuery_Have_Property_OrderByDirection_Type_Of_OrderDirection()
    {
        //Arrange
        var query = new DataTablesPagedQuery<object>(HandlerMock.Object);

        //Act
        var order = query.OrderByDirection;

        //Assert
        Assert.IsAssignableFrom<OrderDirection>(order);
    }

    [Fact]
    public void DataTablesPagedQuery_Have_Property_OrderByColumnName_Type_Of_String()
    {
        //Arrange
        var query = new DataTablesPagedQuery<object>(DispatcherMock.Object) { OrderByColumnName = "" };

        //Act
        var name = query.OrderByColumnName;

        //Assert
        Assert.IsAssignableFrom<string>(name);
    }

    [Fact]
    public void DataTablesPagedQuery_Have_Property_Search_Of_Type_String()
    {
        //Arrange
        var query = new DataTablesPagedQuery<object>(DispatcherMock.Object) { Search = "" };

        //Act
        var name = query.Search;

        //Assert
        Assert.IsAssignableFrom<string>(name);
    }
}