using Olbrasoft.Extensions;

namespace Data.Cqrs.Common.Tests;
public class BaseQueryExtensionsTests
{
    [Fact]
    public void TypeOf_QueryExtensions_IsPublicShouldBeTrue()
    {
        // Arrange
        var sut = typeof(BaseQueryExtensions);

        // Act
        var result = sut.IsPublic;


        // Assert
        result.Should().BeTrue();

    }

    [Fact]
    public void TypeOf_QueryExtensions_AssemblyShouldBeSameAsIQueryAssembly()
    {
        // Arrange
        var sut = typeof(BaseQueryExtensions);

        // Act
        var assembly = sut.Assembly;

        // Assert
        assembly.Should().BeSameAs(typeof(BaseQuery<>).Assembly);
    }

    [Fact]
    public void TypeOf_QueryExtesions_IsStaticShouldBeTrue()
    {
        // Arrange
        var sut = typeof(BaseQueryExtensions);

        // Act
        var isStatic = sut.IsStatic();

        // Assert
        isStatic.Should().BeTrue();
    }

    [Fact]
    public async Task ToResultAsync_NullQuery_ShouldBeThrowExactlyArgumentNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        BaseQuery<object> query = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        Func<Task<object>> act = () => query.ToResultAsync();
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        await act.Should().ThrowExactlyAsync<ArgumentNullException>();

    }


    [Fact]
    public async Task ToResultAsync_QueryWithMockProcessor_ShouldBeCallProcessAsyncOnce()
    {
        // Arrange
        var mockProcessor = new Mock<IQueryProcessor>();
        var query = new BaseQuery<object>(mockProcessor.Object);

        // Act
        await query.ToResultAsync();

        // Assert
        mockProcessor.Verify(p => p.ProcessAsync(query, It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact]
    public async Task ToResultAsync_MockDispatcher_VerifyCallMethodDispatchAsyncOnTheDispatcherOnce()
    {
        // Arrange
        var disMock = new Mock<IMediator>();
        var sut = new BaseQuery<object>(disMock.Object);
        // Act
        await sut.ToResultAsync();
        // Assert
        disMock.Verify(d => d.MediateAsync(sut, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task ToReslultAsync_QueryWithOutProcessorAndDispatcher_SouldBeThrowExactlyToResultExceptionWithMessageContainsProcessor()
    {
        // Arrange
        var sut = new PingQuery();

        // Act
        var act = () => sut.ToResultAsync();

        // Assert
        await act.Should().ThrowExactlyAsync<InvalidOperationException>().Where(e => e.Message.Contains("Processor"));
    }

}
