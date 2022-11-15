using Olbrasoft.Data.Common;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace Olbrasoft.Data.SqlClient;
public class SqlConnectionFactoryTests
{
    [Fact]
    public void SqlConnectionFactory_Inherits_From_DbConnectionFactory()
    {

        //Arrange
        var type = typeof(DbConnectionFactory);

        //Act
        var factory = new SqlConnectionFactory("Ahoj");

        //Assert
        Assert.IsAssignableFrom(type, factory);
    }

    [Fact]
    public void SqlConnectionFactory_CreateConnection_Return_DbConnection()
    {
        //Arrange
        var factory = new SqlConnectionFactory("Server=.;Database=AwesomeDataBase;Trusted_Connection=True;MultipleActiveResultSets=true");

        //Act
        var connection = factory.CreateConnection();

        //Assert
        Assert.IsAssignableFrom<DbConnection>(connection);
    }


    [Fact]
    public void SqlConnectionFactory_CreateConnection_Return_SqlConnection()
    {
        //Arrange
        var factory = new SqlConnectionFactory("Server=.;Database=AwesomeDataBase;Trusted_Connection=True;MultipleActiveResultSets=true");
        
        //Act
        var connection = factory.CreateConnection();

        //Assert
        Assert.IsAssignableFrom<SqlConnection>(connection);
    }
}
