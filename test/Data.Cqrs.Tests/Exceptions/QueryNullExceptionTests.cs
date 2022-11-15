namespace Olbrasoft.Data.Cqrs.Exceptions;
public class QueryNullExceptionTests
{
    [Fact]
    public void GetType_WithoutParams_IsPublicShouldBeTrue()
    {
        // Arrange
        var sut = new QueryNullException();
        // Act
        var type = sut.GetType();
        // Assert
        type.IsPublic.Should().BeTrue();

    }

    [Fact]
    public void GetType_WithoutParams_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(BaseQuery<>).Assembly;
        var sut = new QueryNullException();
        // Act
        var type = sut.GetType();
        // Assert
        type.Assembly.Should().BeSameAs(expected);
    }

    [Fact]
    public void QueryNullException_WithoutParams_ShouldBeAssingableAsExpected()
    {
        // Arrange
        var expected = typeof(ArgumentNullException);

        // Act
        var sut = new QueryNullException();
        // Assert
        sut.Should().BeAssignableTo(expected);

    }

    [Fact]
    public void QueryNullException_WithoutParams_ParamNameShouldBeQuery()
    {
        // Arrange
        var sut = new QueryNullException();

        // Act
        var result = sut.ParamName;

        // Assert
        result.Should().Be("query");
    }   

}
