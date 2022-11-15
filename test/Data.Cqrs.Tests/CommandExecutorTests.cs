namespace Olbrasoft.Data.Cqrs;
public class CommandExecutorTests
{

    [Fact]
    public void TypeOf_CommandExecutor_IsPublicShouldBeTrue()
    {
        // Arrange
        var sut = typeof(CommandExecutor);
        // Act
        var result = sut.IsPublic;
        // Assert
        result.Should().BeTrue();
    }


    [Fact]
    public void CommandExecutor_MockIDispatcher_ShouldBeAssignableOfICommandExecutor()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();

        // Act
        var sut= new CommandExecutor(dispatcher:dis.Object);

        // Assert
        sut.Should().BeAssignableTo<ICommandExecutor>();

    }

    [Fact]
    public void TypeOf_CommandExecutor_AssemblyShouldBeSameAsTypeOfICommandExecutorAssembly()
    {
        // Arrange
        var expected = typeof(ICommandExecutor).Assembly;
        // Act
        var sut = typeof(CommandExecutor).Assembly;
        // Assert
        sut.Should().BeSameAs(expected);    
    }

    [Fact]
    public void CommandExecutor_NullIDispatcher_ShouldThrowExactlyDispatcherNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IDispatcher dis = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new CommandExecutor(dis);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().ThrowExactly<DispatcherNullException>();
    }


    [Fact]
    public async void ExecuteAsync_MockCommand_ShouldBeNotThrowNotImplementationException()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var sut = new CommandExecutor(dispatcher: dis.Object);
        var mockCommand = new Mock<BaseCommand<object>>();

        // Act
        var act = () =>sut.ExecuteAsync(mockCommand.Object);
        
        // Assert
        await act.Should().NotThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task ExecuteAsync_NullCommand_ShouldBeThrowExactlyCommandNullExceptionAsync()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var sut = new CommandExecutor(dispatcher: dis.Object);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        BaseCommand<object> nullCommmand = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => sut.ExecuteAsync(nullCommmand);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        await act.Should().ThrowExactlyAsync<CommandNullException>();
    }


    [Fact]
    public async void ExecuteAsync_MockCommand_ShouldBeCallDispatchAsyncOnce()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var sut = new CommandExecutor(dispatcher:dis.Object);
        var cmd = new Mock<BaseCommand<object>>(dis.Object);

        // Act
        await sut.ExecuteAsync(cmd.Object);

        // Assert
        dis.Verify(p=>p.DispatchAsync(It.IsAny<BaseCommand<object>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void ExecuteAsync_MockCommandAs1stParam_ReisesEventExecuting()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var sut = new CommandExecutor(dispatcher: dis.Object);
        var cmd = new Mock<BaseCommand<object>>(dis.Object);
        var s = new object();
        var c = new object();

        sut.Executing += (sender, args) => { s = sender; c = args.Command; };

        // Act
        await sut.ExecuteAsync(cmd.Object);

        // Assert
        s.Should().BeSameAs(sut);
        c.Should().BeSameAs(cmd.Object);
    }

    [Fact]
    public async Task ExecuteAsync_MockCommandAs1stParam_ReisesEventExecutedAsync()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var sut = new CommandExecutor(dispatcher: dis.Object);
        var cmd = new Mock<BaseCommand<object>>(dis.Object);
        var s = new object();
        var c = new object();

        sut.Executed += (sender, args) => { s = sender; c = args.Command; };
        // Act
        await sut.ExecuteAsync(cmd.Object);

        // Assert
        s.Should().BeSameAs(sut);
        c.Should().BeSameAs(cmd.Object);

    }
}
