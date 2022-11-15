namespace Olbrasoft.Data.Cqrs;
public class ProcessEventArgsTests
{
    [Fact]
    public void TypeOf_ProcessEventArgs_IsPublicShouldBeTrue()
    {
        // Arrange
        var sut = typeof(ProcessEventArgs);
        // Act
        var result = sut.IsPublic;
        // Assert
        result.Should().BeTrue();

    }

    [Fact]
    public void TypeOf_ProcessEventArgs_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(IQueryProcessor).Assembly;
        
        // Act
        var sut = typeof(ProcessEventArgs);

        // Assert
        sut.Assembly.Should().BeSameAs(expected);   
    }

    [Fact]
    public void ProcessEventArgs_WithQueryAndResult_ShouldBeAssingableToExpected()
    {
        // Arrange
        var expected = typeof(EventArgs);

        // Act
        var sut = new ProcessEventArgs(query: new object(), result: new object());
        
        // Assert
        sut.Should().BeAssignableTo(expected);
    }

    [Fact]
    public void ProcessEventArgs_WithQuery_QueryShouldBeSameAsQuery()
    {
        // Arrange
        var query = new object();

        // Act
        var sut = new ProcessEventArgs(query);
        // Assert
        sut.Query.Should().BeSameAs(query);
        sut.Query.Should().NotBeSameAs(new object());
    }

    [Fact]
    public void ProcessEventArgs_ResultAs2stParam_ResultShouldBeSameAsResult()
    {
        // Arrange
        var result = new object();
        // Act
        var sut = new ProcessEventArgs(new object(), result);
        // Assert
        sut.Result.Should().BeSameAs(result);
    }
}
