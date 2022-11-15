namespace Olbrasoft.Data.Cqrs;
public class BaseCommandExtensionsTests
{
    [Fact]
    public async void ToResultAsync_WhenCommandIsNull_ShouldThrowAsyncCommandNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        BaseCommand<object> baseCommand = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => BaseCommandExtensions.ToResultAsync(baseCommand);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        await act.ShouldThrowAsync<CommandNullException>();
    }

    [Fact]
    public async void ToResultAsync_WhenCommandWithExcutor_VerifyCallMethodExecuteAsyncOnTheExecutorOnce()
    {
        // Arrange
        var execMock = new Mock<ICommandExecutor>();
        var sut = new BaseCommand<object>(execMock.Object);
        // Act
        await sut.ToResultAsync();
        // Assert
        execMock.Verify(e => e.ExecuteAsync(Its.EquivalentTo(sut), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void ToResultAsync_WhenCommandWithDispatcher_VerifyCallMethodDispatchAsyncOnTheDispatcherOnce()
    {
        // Arrange
        var disMock = new Mock<IDispatcher>();
        var sut = new BaseCommand<object>(disMock.Object);
        // Act
        await sut.ToResultAsync();
        // Assert
        disMock.Verify(d => d.DispatchAsync(Its.EquivalentTo(sut), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void ToResultAsync_WhenComandHaveDispatcherAndExecutorIsNull_ShouldThrowAsyncToResultException()
    {
        // Arrange
        var sut = new PingCommand();
        // Act
        var act = () => sut.ToResultAsync();
        // Assert
        await act.ShouldThrowAsync<ToResultException>();
    }

    [Fact]
    public async void ToResultAsync_WhenCommandHaveDispatcherAndExecutorIsNull_ShouldThrowAsyncToresultExceptionAndMessageContainExecutor()
    {
        // Arrange
        var sut = new PingCommand();
        // Act
        var act = () => sut.ToResultAsync();
        // Assert
        await act.Should().ThrowExactlyAsync<ToResultException>().Where(ex => ex.Message.Contains("Executor"));
    }
}
