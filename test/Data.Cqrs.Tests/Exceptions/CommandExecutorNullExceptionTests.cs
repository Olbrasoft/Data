namespace Olbrasoft.Data.Cqrs.Exceptions;
public class CommandExecutorNullExceptionTests
{
    [Fact]
    public void TypeOf_CommandExecutorNullException_IsPublicShouldBeTrue()
    {
        // Arrange
        var type = typeof(CommandExecutorNullException);

        // Act
        var isPublic = type.IsPublic;

        // Assert
        isPublic.ShouldBeTrue();

    }

    [Fact]
    public void TypeOf_CommandExecutorNullException_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(CommandExecutor).Assembly;

        // Act
        var sut = typeof(CommandExecutorNullException).Assembly;

        // Assert
        sut.ShouldBeSameAs(expected);

    }

    [Fact]
    public void CommandExcecutorNUllException_WithOutParams_ShouldBeAssingableToArgumentNullException()
    {
        // Arrange
        var expected = typeof(ArgumentNullException);
        // Act
        var sut = new CommandExecutorNullException();

        // Assert
        sut.ShouldBeAssignableTo(expected);
    }

    [Fact]
    public void CommandExecutorNullException_WithOutParams_ParamNameShouldBeExecutor()
    {
        // Arrange
        var sut = new CommandExecutorNullException();
        // Act
        var paramName = sut.ParamName;
        // Assert
        paramName.ShouldBe("executor");
    }

}
