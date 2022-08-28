namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public class DbCommandHandlerTest
{
    [Fact]
    public void DbCommandHandler_Is_Class()
    {
        //Arrange
        var type = TypeOfCommandHandler();

        //Act
        var result = type.IsClass;

        //Assert
        Assert.True(result);
    }

    private static Type TypeOfCommandHandler()
    {
        return typeof(DbCommandHandler<,,,>);
    }

    [Fact]
    public void DbCommandHandler_Is_Abstract()
    {
        //Arrange
        var type = TypeOfCommandHandler();

        //Act
        var result = type.IsAbstract;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DbCommandHandler_Is_Generic()
    {
        //Arrange
        var type = TypeOfCommandHandler();

        //Act
        var result = type.IsGenericType;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void The_DbCommandHandler_Has_Four_Generic_Attributes()
    {
        //Arrange
        var type = TypeOfCommandHandler();

        //Act
        var numberOfGenericArguments = type.GetGenericArguments().Count();

        //Assert
        Assert.True(numberOfGenericArguments == 4);
    }

    [Fact]
    public void The_Name_Of_The_First_Generic_Argument_Is_TCommand()
    {
        //Arrange
        var name = "TCommand";

        //Act
        var argumentName = GetNameOfGenericArgument();
        //Assert
        Assert.True(name == argumentName);
    }

    [Fact]
    public void The_Name_Of_The_Second_Generic_Argument_Is_TResult()
    {
        //Arrange
        var name = "TResult";

        //Act
        var argumentName = GetNameOfGenericArgument(1);

        //Assert
        Assert.True(name == argumentName);
    }

    [Fact]
    public void The_Name_Of_The_Third_Generic_Argument_Is_TContext()
    {
        //Arrange
        var name = "TContext";

        //Act
        var argumentName = GetNameOfGenericArgument(2);

        //Assert
        Assert.True(name == argumentName);
    }

    [Fact]
    public void The_Name_Of_The_Fourth_Generic_Argument_Is_TEntity()
    {
        //Arrange
        var name = "TEntity";

        //Act
        var argumentName = GetNameOfGenericArgument(3);

        //Assert
        Assert.True(name == argumentName);
    }

    /// <summary>
    /// Returns the name of a generic argument
    /// </summary>
    /// <param name="numberOfArgument">Argument numbering starts at zero</param>
    /// <returns>Argument name</returns>
    public string GetNameOfGenericArgument(int numberOfArgument = 0)
    {
        var type = TypeOfCommandHandler();
        return type.GetGenericArguments()[numberOfArgument].Name;
    }

    [Fact]
    public void The_Base_Type_Name_DbCommandHandler_Is_The_Same_As_The_CommandHandler_Type_NBame()
    {
        //Arrange
        var commandHandlerTypeName = typeof(CommandHandler<,>).Name;

        //Act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var dbCommandHandlerTypeName = typeof(DbCommandHandler<,,,>).BaseType.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        //Assert
        Assert.True(commandHandlerTypeName == dbCommandHandlerTypeName);
    }

    [Fact]
    public void AwesomeCommandHandler_Inherit_From_DbCommandHandler_Of_IRequest_Of_Int_Comma_Int_Comma_DbContext_Comma_AwesomeEntity()
    {
        //Arrange
        var type = typeof(DbCommandHandler<IRequest<int>, int, DbContext, AwesomeEntity>);

        var handler = CreateAwesomeHandler();

        //Assert
        Assert.IsAssignableFrom(type, handler);
    }

    [Fact]
    public void AwesomeBooleanCommandHandler_Inherit_From_DbCommandHandler_Of_IRequest_Of_Bool_Comma_DbContext_Comma_AwesomeEntity()
    {
        //Arrange
        var type = typeof(DbCommandHandler<IRequest<bool>, DbContext, AwesomeEntity>);
        var contextMock = new Mock<DbContext>();
        var mapperMock = new Mock<IMapper>();

        //Act
        var handler = new AwesomeBooleanCommandHandler(mapperMock.Object, contextMock.Object);

        //Assert
        Assert.IsAssignableFrom(type, handler);
    }

    private static AwesomeCommandHandler CreateAwesomeHandler()
    {
        var factoryMock = new Mock<IDbContextFactory<DbContext>>();

        var contextMock = new Mock<DbContext>();
        var setMock = new Mock<DbSet<AwesomeEntity>>();
       

        contextMock.Setup(p => p.Set<AwesomeEntity>()).Returns(setMock.Object);

        var mapperMock = new Mock<IMapper>();

        //Act
        var handler = new AwesomeCommandHandler(mapperMock.Object, contextMock.Object);
        return handler;
    }

    [Fact]
    public void Protected_Property_Context_Return_Type_DbContext()
    {
        //Arrange
        var handler = CreateAwesomeHandler();

        //Act
        var context = handler.GetProtectedPropertyContext();

        //Assert
        Assert.IsAssignableFrom<DbContext>(context);
    }

    [Fact]
    public void Protected_Property_Set_Return_DbSet_Of_TEntity()
    {
        //Arrange
        var handler = CreateAwesomeHandler();

        //Act
        var set = handler.GetProtectedPropertyEntities();

        //Assert
        Assert.IsAssignableFrom<DbSet<AwesomeEntity>>(set);
    }

    [Fact]
    public void DbCommandHandler_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        //Arrange
        var mapperMock = new Mock<IMapper>();

        //Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var ex = Assert.Throws<ArgumentNullException>(() => new AwesomeCommandHandler(mapperMock.Object, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        //Assert
        Assert.True(ex.Message is "Value cannot be null. (Parameter 'context')");
    }
}