namespace Data.Cqrs.Common.Tests;
public class QueryProcessorTests
{

    [Fact]
    public void IsPublic_TypeOfQueryProcessor_True()
    {
        // Arrange
        var sut = typeof(QueryProcessor);

        // Act
        var result = sut.IsPublic;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ImplementInterface_IQueryProcessor_ShouldBeTrue()
    {
        // Arrange
        var ExpectType = typeof(IQueryProcessor);
        var dis = new Mock<IMediator>();

        // Act
        var processor = new QueryProcessor(dis.Object);

        // Assert
        processor.Should().BeAssignableTo(ExpectType);
    }


    [Fact]
    public async Task ProcessAsync_WhenQueryIsNull_ThrowException()
    {
        // Arrange
        var dis = new Mock<IMediator>();
        var sut = new QueryProcessor(dis.Object);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        BaseQuery<string> query = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => sut.ProcessAsync(query, default);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }



    [Fact]
    public void QueryProcessor_WhenDispatcherIsNull_ShouldThrowExactlyNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IMediator dis = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new QueryProcessor(dis);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();

    }

    [Fact]
    public async Task ProcessAsync_MockQueryObject_CallDispatchAsyncOnDispatcher()
    {
        // Arrange
        var dis = new Mock<IMediator>();
        var query = new Mock<BaseQuery<string>>(dis.Object);
        var sut = new QueryProcessor(dis.Object);

        // Act
        await sut.ProcessAsync(query.Object);

        // Assert
        dis.Verify(p => p.MediateAsync(It.IsAny<BaseQuery<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MockProcessor_PingQueryToResultAsync_ShouldCallProcessAsyncOnce()
    {
        // Arrange
        var mockProcessor = new Mock<IQueryProcessor>();
        var query = new PingQuery(mockProcessor.Object);
        // Act
        await query.ToResultAsync();

        // Assert
        mockProcessor.Verify(m => m.ProcessAsync(query, It.IsAny<CancellationToken>()), Times.Once);
    }



}
