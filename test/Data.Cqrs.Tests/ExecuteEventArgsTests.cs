namespace Olbrasoft.Data.Cqrs;
public class ExecuteEventArgsTests
{
    [Fact]
    public void TypeOf_ExecuteEventArgs_IsPublicShouldBeTrue()
    {
        // Arrange
        var sut = typeof(ExecuteEventArgs);
        // Act
        var isPublic = sut.IsPublic;
        // Assert
        isPublic.Should().BeTrue();

    }

    [Fact]
    public void TypeOf_ExecuteEventArgs_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(ICommandExecutor).Assembly;
        // Act
        var sut = typeof(ExecuteEventArgs).Assembly;

        // Assert
        sut.Should().BeSameAs(expected);    
    }
    [Fact]
    public void ExecuteEventArgs_CommandAndResult_ShouldBeAssingableToExpected()
    {
        // Arrange
        var expected = typeof(EventArgs);
        // Act
        var sut = new ExecuteEventArgs(command: new object(), result: new object());

        // Assert
        sut.Should().BeAssignableTo(expected);    
    }

    [Fact]
    public void ExecuteEventArgs_ExpectedAs1stParam_CommandShouldBeSameAsExpected()
    {
        // Arrange
        var expected = new object();
        // Act
        var sut = new ExecuteEventArgs(expected);
        // Assert
        sut.Command.Should().BeSameAs(expected);
    }

    [Fact]
    public void ExecuteEventArgs_ExpectedAs2stParam_ResultShouldBeSameAsExpected()
    {
        // Arrange
        var expected = new object();

        // Act
        var sut = new ExecuteEventArgs(new object(), expected);
        // Assert
        sut.Result.Should().BeSameAs(expected); 
    }

}
