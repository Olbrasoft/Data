namespace Data.Cqrs.Common.Tests;
public class PingTests
{
    //Ping IsAssignableFrom BaseRequest<string>

    [Fact]
    public void Ping_IsAssignableFrom_BaseRequest()
    {
        //Arrange
        var mediator = new Mock<IMediator>().Object;
        var ping = new Ping(mediator);

        //Act
        var isBaseRequest = ping is BaseRequest<string>;

        //Assert
        Assert.True(isBaseRequest);
    }

    //Constructor ShouldThrowArgumentNullExceptionWhenMediatorIsNull
    [Fact]
    public void Constructor_ShouldThrowArgumentNullExceptionWhenMediatorIsNull()
    {
        //Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IMediator mediator = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        //Act

#pragma warning disable CS8604 // Possible null reference argument.
        void act() => new Ping(mediator);
#pragma warning restore CS8604 // Possible null reference argument.


        //Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    //Constructor set Mediator
    [Fact]
    public void Constructor_SetMediator()
    {
        //Arrange
        var mediator = new Mock<IMediator>().Object;

        //Act
        var ping = new Ping(mediator);

        //Assert
        Assert.Equal(mediator, ping.Mediator);
    }

    //has order Constructor with out parameter
    [Fact]
    public void Constructor_HasOrderConstructorWithOutParameter()
    {
        //Arrange
        var request = new Ping();
        var constructors = typeof(Ping).GetConstructors();

        //Act
        var hasOrderConstructor = constructors.Any(c => c.GetParameters().Any(p => p.IsOut));

        //Assert
        Assert.False(hasOrderConstructor);
    }




}
