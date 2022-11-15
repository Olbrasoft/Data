namespace Olbrasoft.Data.Cqrs.Exceptions;
public class ToResultExceptionTests
{
    [Fact]
    public void TypeOf_ToResultAsync_isPublicShouldBeTrue()
    {
        // Arrange
        var type = typeof(ToResultException);
        // Act
        var isPublic = type.IsPublic;
        // Assert
        isPublic.ShouldBeTrue();

    }

    [Fact]
    public void AssemblyOf_ToResultException_ShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(ICommandExecutor).Assembly;
        // Act
        var sut = typeof(ToResultException).Assembly;
        // Assert
        sut.ShouldBeSameAs(expected);
    }


    [Fact]
    public void ToResultException_WithParamName_ShouldBeAssingableToExpeced()
    {
        // Arrange
        var expected = typeof(InvalidOperationException);
        // Act
        var sut = new ToResultException(paramName: "sameParamName");

        // Assert
        sut.ShouldBeAssignableTo(expected);

    }

    [Fact]
    public void ToResultException_WithAwesomeName_MessageShouldBeContainsDispatcherAndAwesomeName()
    {
        // Arrange
        var sut = new ToResultException(paramName: "sameParamName");

        // Act
        var message = sut.Message;
        // Assert
        message.Should().Contain("dispatcher").And.Contain("sameParamName");
    }
}
