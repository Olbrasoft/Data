namespace Olbrasoft.Data.Cqrs;

public class CommandHandlerTest : BaseTest
{
    [Fact]
    public void AwesomeCommandHandler_Inherit_From_CommandHandler()
    {
        //Arrange
        var type = typeof(CommandHandler<Request<bool>, bool>);

        //Act
        var handler = new AwesomeCommandHandler<Request<bool>, bool>(CreateMapperMock().Object);

        //Assert
        Assert.IsAssignableFrom(type, handler);
    }

    private static Mock<IMapper> CreateMapperMock()
    {
        return new Mock<IMapper>();
    }

    [Fact]
    public void AwesomeCommandHandler_MapTo_Calls_The_MapTo_Method_On_The_Mapper()
    {
        //Arrange
        var mapperMock = CreateMapperMock();
        var handler = new AwesomeCommandHandler<Request<bool>, bool>(mapperMock.Object);

        //Act
        handler.Call_MapTo_Of_String_With_Parameter_Of_Type_String();

        //Assert
        mapperMock.Verify(P => P.MapTo<string>(""));
    }

    [Fact]
    //The CommandHandler's TCommand generic
    public void A_CommandHandler_With_Two_Generic_Arguments_TCommand_Constraint_Name_Is_IRequest_Of()
    {
        //Arrange
        var typeName = typeof(IRequest<>).Name;
        var argument = typeof(CommandHandler<,>).GetGenericArguments()[0];

        //Act
        var constraintName = argument.GetGenericParameterConstraints()[0].Name;

        //Assert
        Assert.True(typeName == constraintName);
    }

    [Fact]
    public void CommandHandler_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        //Arrange
        IMapper mapper = default;

        //Act
        // ReSharper disable once ExpressionIsAlwaysNull
        var ex = Assert.Throws<ArgumentNullException>(() => new AwesomeCommandHandler<Request<bool>, bool>(mapper));

        //Assert
        Assert.True(ex.Message is "Value cannot be null. (Parameter 'mapper')");
    }
}