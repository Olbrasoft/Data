using Xunit;

namespace Olbrasoft.Data.Common;
public class DbConnectionFactoryTests
{
    [Fact]
    public void DbConnectionFactory_Is_Abstract_Class()
    {
        //Arrange
        var type = typeof(DbConnectionFactory);

        //Act
        var isAbstract = type.IsAbstract;

        //Assert
        Assert.True(isAbstract);
    }

    [Fact]
    public void DbConnectionFactory_Is_Public()
    {
        //Arrange
        var type = typeof(DbConnectionFactory);

        //Act
        var isPublic = type.IsPublic;

        //Assert
        Assert.True(isPublic);
    }

    [Fact]
    public void AwesomeDbConnectionFactory_Inherits_From_DbConnectionFactory()
    {
        //Arrange
        var type = typeof(DbConnectionFactory);

        //Act
        var connection = new AwesomeDbConnectionFactory("Awesome connection string");

        //Assert
        Assert.IsAssignableFrom(type, connection);
    }

    [Fact]
    public void AwesomeDbConnectionFactory_GetConnectionString_Return_String_Same_Constructor()
    {
        //Arrange
        var connectionString = "Awesome connection string";
        var connection = new AwesomeDbConnectionFactory(connectionString);

        //Act
        var result = connection.GetConnectionString();


        //Assert
        Assert.Same(connectionString, result);
    }

    [Fact]
    public void AwesomeDbConnectionFactory_Throw_ArgumentNullExeption_When_ConnectionString_Is_Null()
    {
       
        //Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => new AwesomeDbConnectionFactory(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }


    [Fact]
    public void AwesomeDbConnectionFactory_Throw_ArgumentExeption_When_ConnectionString_Is_WhiteSpace()
    {
        //Assert
        Assert.Throws<ArgumentException>(() => new AwesomeDbConnectionFactory(""));
    }

    [Fact]
    public void AwesomeDbConnectionFactory_Throw_ArgumentExeption_When_ConnectionString_Is_Empty()
    {
        //Assert
        Assert.Throws<ArgumentException>(() => new AwesomeDbConnectionFactory(string.Empty));
    }


}
