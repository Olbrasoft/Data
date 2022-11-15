namespace Olbrasoft.Data.Cqrs;
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
        var dis = new Mock<IDispatcher>();

        // Act
        var processor = new QueryProcessor(dis.Object);

        // Assert
        processor.Should().BeAssignableTo(ExpectType);
    }


    [Fact]
    public async void ProcessAsync_WhenQueryIsNull_ThrowException()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var sut = new QueryProcessor(dis.Object);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        BaseQuery<string> query = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => sut.ProcessAsync(query, default);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        await act.Should().ThrowAsync<QueryNullException>();
    }



    [Fact]
    public void QueryProcessor_WhenDispatcherIsNull_ShouldThrowExactlyNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IDispatcher dis = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new QueryProcessor(dis);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().ThrowExactly<DispatcherNullException>();

    }

    [Fact]
    public async void ProcessAsync_MockQueryObject_CallDispatchAsyncOnDispatcher()
    {
        // Arrange
        var dis = new Mock<IDispatcher>();
        var query = new Mock<BaseQuery<string>>(dis.Object);
        var sut = new QueryProcessor(dis.Object);

        // Act
        await sut.ProcessAsync(query.Object);

        // Assert
        dis.Verify(p => p.DispatchAsync(It.IsAny<BaseQuery<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void MockProcessor_PingQueryToResultAsync_ShouldCallProcessAsyncOnce()
    {
        // Arrange
        var mockProcessor = new Mock<IQueryProcessor>();
        var query = new PingQuery(mockProcessor.Object);
        // Act
        await query.ToResultAsync();

        // Assert
        mockProcessor.Verify(m => m.ProcessAsync(Its.EquivalentTo(query), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void ProcessAsync_MockQuery_ReisesEventProcessing()
    {
        // Arrange
        var mockDis = new Mock<IDispatcher>();
        var sut = new QueryProcessor(mockDis.Object);
        var mockQuery = new Mock<BaseQuery<string>>(sut);
        
        object s = new object();
        object q = new object();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        sut.Processing += (sender, arg) => { s = sender; q = arg.Query; };
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        await sut.ProcessAsync(mockQuery.Object);

        // Assert
        s.Should().BeSameAs(sut);
        q.Should().BeSameAs(mockQuery.Object);

    }

    [Fact]
    public async void ProcessAsync_MockQuery_ReisesEventProcessed()
    {
        // Arrange
        var mockDis = new Mock<IDispatcher>();
        var sut = new QueryProcessor(mockDis.Object);
        var mockQuery = new Mock<BaseQuery<string>>(sut);

        object s = new object();
        object q = new object();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        sut.Processed += (sender, args) => { s = sender; q = args.Query; };
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        await sut.ProcessAsync(mockQuery.Object);

        // Assert
        s.Should().BeSameAs(sut);
        q.Should().BeSameAs(mockQuery.Object);
    }
}
