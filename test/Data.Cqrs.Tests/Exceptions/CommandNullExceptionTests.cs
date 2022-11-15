namespace Olbrasoft.Data.Cqrs.Exceptions;
public class CommandNullExceptionTests
{
    [Fact]
    public void TypeOf_CommandNullException_IsPublicShouldBeTrue()
    {
        // Arrange
        var type = typeof(CommandNullException);
        // Act
        var isPublic = type.IsPublic;
        // Assert
        isPublic.ShouldBeTrue();
    }

    [Fact]
    public void TypeOf_CommandNullException_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(BaseCommand<>).Assembly;
        // Act
        var sut = typeof(CommandNullException).Assembly;

        // Assert
        sut.ShouldBeSameAs(expected);

    }

    [Fact]
    public void CommandNullException_WithOutParams_ShouldBeAssingableToExpected()
    {
        // Arrange
        var exoected = typeof(ArgumentNullException);
        // Act
        var sut = new CommandNullException();
        // Assert
        sut.ShouldBeAssignableTo(exoected);
    }

    [Fact]
    public void CommandNullException_ParamName_SholdBeExpected()
    {
        // Arrange
        var expected = "command";
        // Act
        var sut = new CommandNullException();
        // Assert
        sut.ParamName.ShouldBe(expected);
    }
}
