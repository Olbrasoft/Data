namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class DbCommandHandlerTests
{

    //test DbCommandHandler is public class
    [Fact]
    public void DbCommandHandler_IsPublicClass()
    {
        //Arrange
        var type = typeof(DbCommandHandler<,,,>);
        //Act
        var isPublic = type.IsPublic;
        //Assert
        Assert.True(isPublic);
    }

    //DbCommandHandler Assembly is "MediatR.Cqrs.EntityFrameworkCore"
    [Fact]
    public void DbCommandHandler_AssemblyShouldBe_MediatR_Cqrs_EntityFrameworkCore()
    {
        //Arrange
        var type = typeof(DbCommandHandler<,,,>);
        //Act
        var assembly = type.Assembly.GetName().Name;
        //Assert
        Assert.Equal("Olbrasoft.Data.Cqrs.EntityFrameworkCore", assembly);
    }

    //is Anstract class
    [Fact]
    public void DbCommandHandler_IsAnstractClass()
    {
        //Arrange
        var type = typeof(DbCommandHandler<,,,>);
        //Act
        var isAbstract = type.IsAbstract;
        //Assert
        Assert.True(isAbstract);
    }

    //Should Be Inherit from DbHandler
    [Fact]
    public void DbCommandHandler_ShouldBeInheritFrom_DbHandler()
    {
        //Arrange
        var type = typeof(DbCommandHandler<DbContext, PingBook, BaseCommand<string>, string>);
        //Act
        var isSubClass = typeof(DbRequestHandler<DbContext, PingBook, BaseCommand<string>, string>).IsAssignableFrom(type);
        //Assert
        Assert.True(isSubClass);
    }

    //Statict method ThrowIfCommandIsNullOrCancellationRequested throw CommandNullException when command is null





}
