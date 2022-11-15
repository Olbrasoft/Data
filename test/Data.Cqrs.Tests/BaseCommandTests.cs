namespace Olbrasoft.Data.Cqrs;
public class BaseCommandTests
{
    [Fact]
    public void BaseCommand_WhenCommandExecurorIsNull_ShouldThrowCommandExecutorNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        ICommandExecutor commandExecutor = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new BaseCommand<object>(commandExecutor);
#pragma warning restore CS8604 // Possible null reference argument.
                              // Assert
        act.ShouldThrow<CommandExecutorNullException>();
    }

    [Fact]
    public void BaseCommand_WithExecutor_ExecutorShouldBeSameAsExpected()
    {
        // Arrange
        var expected = new Mock<ICommandExecutor>().Object;
        // Act
        var sut = new BaseCommand<object>(expected);
        // Assert
        sut.Executor.ShouldBeSameAs(expected);
    }

    [Fact]
    public void BaseCommand_WithDispatcher_DispatcherShouldBeSameAsExpected()
    {
        // Arrange
        var expected = new Mock<IDispatcher>().Object;

        // Act
        var sut = new BaseCommand<object>(expected);
        // Assert
        sut.Dispatcher.ShouldBeSameAs(expected);
    }

}
