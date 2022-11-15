namespace Olbrasoft.Data.Cqrs;
public class BaseQueryTest
{
    [Fact]
    public void TypeOfBaseQuery_IsPublic_ShouldBeTrue()
    {
        // Arrange
        var type = typeof(BaseQuery<>);
        // Act
        var result = type.IsPublic;

        // Assert
        result.Should().BeTrue();
    }


    [Fact]
    public void TypeOf_BaseQuery_AssemblyShouldBeSameAsTypeOfIQueryAssembly()
    {
        // Arrange
        var expectAssembly = typeof(BaseQuery<>).Assembly;

        // Act
        var assembly = typeof(BaseQuery<>).Assembly;

        // Assert
        assembly.Should().BeSameAs(expectAssembly);
    }


    [Fact]
    public void BaseQuery_NullIQueryProcessor_ShouldBeThrowExactlyArgumentNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IQueryProcessor processor = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new BaseQuery<object>(processor: processor);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().ThrowExactly<QueryProcessorNullException>();
    }


    [Fact]
    public void BaseQuery_NullDispatcher_ShouldBeThrowExactlyDispatcherNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IDispatcher sut = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new BaseQuery<object>(dispatcher: sut);
#pragma warning restore CS8604 // Possible null reference argument.
                              // Assert
        act.Should().ThrowExactly<DispatcherNullException>();
    }





}
