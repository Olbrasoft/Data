namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class DbCommandHandlerTests
{

    //test DbCommandHandler is public class
    [Fact]
    public void DbCommandHandler_IsPublicClass()
    {
        //Arrange
        Type type = typeof(DbCommandHandler<,,,>);
        //Act
        bool isPublic = type.IsPublic;
        //Assert
        Assert.True(isPublic);
    }

    //DbCommandHandler Assembly is "MediatR.Cqrs.EntityFrameworkCore"
    [Fact]
    public void DbCommandHandler_AssemblyShouldBe_MediatR_Cqrs_EntityFrameworkCore()
    {
        //Arrange
        Type type = typeof(DbCommandHandler<,,,>);
        //Act
        string? assembly = type.Assembly.GetName().Name;
        //Assert
        Assert.Equal("Olbrasoft.Data.Cqrs.EntityFrameworkCore", assembly);
    }

    //is Anstract class
    [Fact]
    public void DbCommandHandler_IsAnstractClass()
    {
        //Arrange
        Type type = typeof(DbCommandHandler<,,,>);
        //Act
        bool isAbstract = type.IsAbstract;
        //Assert
        Assert.True(isAbstract);
    }

    //Should Be Inherit from DbHandler
    [Fact]
    public void DbCommandHandler_ShouldBeInheritFrom_DbHandler()
    {
        //Arrange
        Type type = typeof(DbCommandHandler<DbContext, PingBook, BaseCommand<string>, string>);
        //Act
        bool isSubClass = typeof(DbRequestHandler<DbContext, PingBook, BaseCommand<string>, string>).IsAssignableFrom(type);
        //Assert
        Assert.True(isSubClass);
    }





}
