namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public class DbQueryHandlerTest
{
    [Fact]
    public void DbQueryHandler_Is_Abstract()
    {
        //Arrange
        var type = typeof(DbQueryHandler<,,,>);

        //Act
        var result = type.IsAbstract;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void AwesomeQueryHandler_Inherit_From_DbQueryHandler_Of_Request_Of_Object_Comma_Object_AwesomeEntity()
    {
        //Arrange
        var type = typeof(DbQueryHandler<Request<object>, object, DbContext, AwesomeEntity>);

        //Act
        var handler = CreateAwesomeQueryHandler();

        //Assert
        Assert.IsAssignableFrom(type, handler);
    }

    [Fact]
    public void AwesomeDbQueryHandler_Of_AwesomeEntity_Comma_Request_Of_Object_Comma_Object_Inherit_From_QueryHandler_Of_Request_Of_Object_comma_Object()
    {
        //Arrange
        var type = typeof(QueryHandler<Request<object>, object>);

        //Act
        var handler = CreateAwesomeQueryHandler();

        //Assert
        Assert.IsAssignableFrom(type, handler);
    }

    [Fact]
    public void AwesomeQueryHandler_Property_Context_Return_Base_Protected_Property_Context_Type_Of_DbContext()
    {
        //Arrange
        var handler = CreateAwesomeQueryHandler();

        //Act
        var result = handler.Context;

        //Assert
        Assert.IsAssignableFrom<DbContext>(result);
    }

    [Fact]
    public void AwesomeQueryHandler_Property_Entities_Return_Base_Protected_Property_Type_Of_IQueryable_Of_AwesomeEntity()
    {
        //Arrange
        var handler = CreateAwesomeQueryHandler();

        //Act
        var result = handler.Entities;

        //Assert
        Assert.IsAssignableFrom<IQueryable<AwesomeEntity>>(result);
    }

    [Fact]
    public void AwesomeBooleanQueryHandler_Inherit_From_DbQueryHandler_Of_IRequest_Of_Bool_Comma_DbContext_Comma_AwesomeEntity()
    {
        //Arrange
        var type = typeof(DbQueryHandler<IRequest<bool>, DbContext, AwesomeEntity>);

        var handler = CreateAwesomeBooleanQueryHandler();

        //Assert
        Assert.IsAssignableFrom(type, handler);
    }

    [Fact]
    public void AwesomeBooleanQueryHandler_GetProtectedPropertyContext_Return_Context()
    {
        //Arrange
        var handler = CreateAwesomeBooleanQueryHandler();

        //Act
        var result = handler.GetProtectedPropertyContext();

        //Assert
        Assert.IsAssignableFrom<DbContext>(result);
    }

    [Fact]
    public void AwesomeBooleanQueryHandler_GetProtectedProperyEntities_Return_Protected_Property_Type_Of_IQueryable_Of_AwesomeEntity()
    {
        var handler = CreateAwesomeBooleanQueryHandler();

        //Act
        var result = handler.GetProtectedProperyEntities();

        //Assert
        Assert.IsAssignableFrom<IQueryable<AwesomeEntity>>(result);
    }

    private static AwesomeBooleanQueryHandler CreateAwesomeBooleanQueryHandler()
    {
        var handler = new AwesomeBooleanQueryHandler(CreateFakeContext());
        return handler;
    }

    private static AwesomeQueryHandler CreateAwesomeQueryHandler()
    {
        var projectorMock = new Mock<IProjector>();

        return new AwesomeQueryHandler(projectorMock.Object, CreateFakeContext());
    }

    private static DbContext CreateFakeContext()
    {

        var contextMock = new Mock<DbContext>();
        var setMock = new Mock<DbSet<AwesomeEntity>>();

        contextMock.Setup(p => p.Set<AwesomeEntity>()).Returns(setMock.Object);



        return contextMock.Object;
    }

    [Fact]
    public void DbQueryHandler_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        //Arrange
        var projectorMock = new Mock<IProjector>();

        //Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var ex = Assert.Throws<ArgumentNullException>((() => new AwesomeQueryHandler(projectorMock.Object, null)));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        //Assert
        Assert.True(ex.Message is "Value cannot be null. (Parameter 'context')");
    }

    [Fact]
    public void BooleanDbQueryHandler_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        //Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var ex = Assert.Throws<ArgumentNullException>(() => new AwesomeBooleanQueryHandler(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        //Assert
        Assert.True(ex.Message is "Value cannot be null. (Parameter 'context')");
    }
}