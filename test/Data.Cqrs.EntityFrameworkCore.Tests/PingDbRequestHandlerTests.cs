namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbRequestHandlerTests : IDisposable
{

    //private static field DbContextOptions<PingLibraryDbContext>
    private static readonly DbContextOptions<PingLibraryDbContext> _options = new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase("PingBook")
     .Options;

    PingLibraryDbContext _context;

    public PingLibraryDbContext CreateContext()
    {

        var context = new PingLibraryDbContext(_options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;


    }

    public PingDbRequestHandlerTests()
    {
        _context = new PingLibraryDbContext(_options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _context.Books.Add(new PingBook { Title = "Book1" });
        _context.Books.Add(new PingBook { Title = "Book2" });
        _context.Books.Add(new PingBook { Title = "Book3" });

        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }



    //ExistsAsync with out predicate return true
    [Fact]
    public async Task ShouldReturnTrueWithOutPredicate()
    {
        //Arrange
        var context = CreateContext();

        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = await handler.ExistsAsync();

        //Assert
        Assert.True(result);
    }



    // //ExistsAsync return false
    [Fact]
    public async Task ShouldReturnFalse()
    {
        //Arrange

        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = await handler.ExistsAsync(x => x.Id == 5);

        //Assert
        Assert.False(result);

    }


    ////ExistsAsync return true
    [Fact]
    public async Task ShouldReturnTrue()
    {
        //Arrange

        var handler = new PingDbRequestHandler(_context);

        //Act
        var result = await handler.ExistsAsync(x => x.Id == 1);

        //Assert
        Assert.True(result);
    }

    //GetOrderByDescending return first book title Book3
    [Fact]
    public void ShouldReturnBook3()
    {
        //Arrange
        var handler = new PingDbRequestHandler(_context);

        //Act
        var result = handler.GetOrderByDescending(x => x.Id).First().Title;

        //Assert
        Assert.Equal("Book3", result);
    }


    //GetOrderBy return first book title aaa
    [Fact]
    public void ShouldReturnBook1()
    {
        //Arrange
        var handler = new PingDbRequestHandler(_context);

        _context.Books.Add(new PingBook { Title = "aaa" });
        _context.SaveChanges();

        //Act
        var result = handler.GetOrderBy(x => x.Title).First().Title;

        _context.Books.RemoveRange(_context.Books.Where(x => x.Title == "aaa"));
        _context.SaveChanges();

        //Assert
        Assert.Equal("aaa", result);
    }


    //GetWhere return first book title Book2
    [Fact]
    public void GetWher_ShouldReturnBook1()
    {
        //Arrange
        var handler = new PingDbRequestHandler(_context);

        //Act
        var result = handler.GetWhere(x => x.Title == "Book2").First().Title;

        //Assert
        Assert.Equal("Book2", result);
    }


    //GetOneOrNullAsync return first book title Book1
    [Fact]
    public async Task GetOneOrNullAsync_WithQueryable_ShouldReturnBook1()
    {
        //Arrange
        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = (await handler.GetOneOrNullAsync(handler.GetWhere(x => x.Title == "Book1"), default))?.Title;

        //Assert
        result.Should().Be("Book1");

    }

    //GetOneOrNullAsync return null
    [Fact]
    public async Task GetOneOrNullAsync_WithQueryable_ShouldReturnNull()
    {
        //Arrange
        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = await handler.GetOneOrNullAsync(handler.GetWhere(x => x.Title == "Book5"), default);

        //Assert
        result.Should().BeNull();
    }

    //GetOneOrNullAsync with expression return first book title Book3
    [Fact]
    public async Task GetOneOrNullAsync_WithExpression_ShouldReturnBook3()
    {
        //Arrange
        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = (await handler.GetOneOrNullAsync(x => x.Id == 3, default))?.Title;

        //Assert
        result.Should().Be("Book3");
    }


    //GetOneOrNullAsync with expression return null
    [Fact]
    public async Task GetOneOrNullAsync_WithExpression_ShouldReturnNull()
    {
        //Arrange
        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = await handler.GetOneOrNullAsync(x => x.Id == 5, default);

        //Assert
        result.Should().BeNull();
    }

    //GetEnumerableAsync return 2 books
    [Fact]
    public async Task GetEnumerableAsync_ShouldReturn2Books()
    {
        //Arrange
        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = await handler.GetEnumerableAsync(handler.GetWhere(x => x.Id > 1), default);

        //Assert
        result.Should().HaveCount(2);
    }


    //GetEnumerableAsync with expression return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithExpression_ShouldReturn2Books()
    {
        //Arrange
        var handler = new PingDbRequestHandler(CreateContext());

        //Act
        var result = await handler.GetEnumerableAsync(x => x.Id > 1, default);

        //Assert
        result.Should().HaveCount(2);
    }

    //GetEnumerableAsync with TDestination and With expression return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithTDestinationAndExpression_ShouldReturn2Books()
    {
        //Arrange
        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var handler = new PingDbRequestHandler(projector, CreateContext());

        //Act
        var result = await handler.GetEnumerableAsync<PingDtoBook>(x => x.Id > 1, default);

        //Assert
        result.Should().HaveCount(2);
    }

    //GetOneOrNullAsync with TDestination return first book title Book1
    [Fact]
    public async Task GetOneOrNullAsync_WithTDestination_ShouldReturnBook1()
    {
        //Arrange
        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var handler = new PingDbRequestHandler(projector, CreateContext());

        //Act
        var result = (await handler.GetOneOrNullAsync<PingDtoBook>(handler.GetWhere(x => x.Title == "Book1"), default))?.Title;

        //Assert
        result.Should().Be("Book1");
    }



    //GetOneOrNullAsync with TDestination and expression return first book title Book3
    [Fact]
    public async Task GetOneOrNullAsync_WithTDestinationAndExpression_ShouldReturnBook3()
    {
        //Arrange
        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var handler = new PingDbRequestHandler(projector, CreateContext());

        //Act
        var result = (await handler.GetOneOrNullAsync<PingDtoBook>(x => x.Id == 3, default))?.Title;

        //Assert
        result.Should().Be("Book3");
    }

    //GetEnumerableAsync with TDestination return 3 books
    [Fact]
    public async Task GetEnumerableAsync_WithTDestination_ShouldReturn3Books()
    {
        //Arrange
        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var handler = new PingDbRequestHandler(projector, CreateContext());

        //Act
        var result = await handler.GetEnumerableAsync<PingDtoBook>(handler.GetWhere(x => x.Id > 0), default);

        //Assert
        result.Should().HaveCount(3);
    }

    //GetEnumerableAsync with TDestination and queryable return 3 books
    [Fact]
    public async Task GetEnumerableAsync_WithTDestinationAndQueryable_ShouldReturn3Books()
    {
        //Arrange
        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var handler = new PingDbRequestHandler(projector, CreateContext());

        //Act
        var result = await handler.GetEnumerableAsync<PingDtoBook>(default);

        //Assert
        result.Should().HaveCount(3);
    }


    //GetOneOrNullAsync with TDestination return null
    [Fact]
    public async Task GetOneOrNullAsync_WithTDestination_ShouldReturnNull()
    {
        //Arrange
        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var contextOptions = new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase(new Guid().ToString())
     .Options;

        using var context = new PingLibraryDbContext(contextOptions);

        context.Database.EnsureCreated();

        var handler = new PingDbRequestHandler(projector, context);

        //Act
        var result = await handler.GetOneOrNullAsync<PingDtoBook>(handler.GetWhere(x => x.Id == 1000), default);

        context.Database.EnsureDeleted();


        //Assert
        result.Should().BeNull();



    }
}
