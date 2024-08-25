namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbBaseCommandHandlerTests
{

    //Inherits from DbBaseCommandHandler<PingLibraryDbContext, PingBook, BaseCommand<PingBook>, PingBook>
    [Fact]
    public void InheritsFromDbBaseCommandHandler_PingLibraryDbContext_PingBook_BaseCommand_PingBook()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        //Act
        PingDbBaseCommandHandler handler = new(context);

        //Assert
        handler.Should().BeAssignableTo<DbBaseCommandHandler<PingLibraryDbContext, PingBook, PingBookBaseCommand, PingBook>>();
    }

    ///add test RemoveAndSaveAsync(Expression<Func<PingBook, bool>> exp, CancellationToken token = default) new PingLibraryDbContext in memmory add five PingBook and  RemoveAndSaveAsync when id > 2 should return CommandStatus.Deleted
    [Fact]
    public async Task RemoveAndSaveAsync_ExpressionFuncPingBookBool_ShouldReturnCommandStatusDeleted()
    {
        //Arrange
        DbContextOptions<PingLibraryDbContext> options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using PingLibraryDbContext context = new(options);

        context.Database.EnsureCreated();


        await context.AddRangeAsync(
        [

            new PingBook { Id = 4, Title = "Title 4" },
            new PingBook { Id = 5, Title = "Title 5" }
        ]);


        await context.SaveChangesAsync();

        PingDbBaseCommandHandler handler = new(context);

        //Act
        int result = await handler.DeleteAsync(x => x.Id == 2);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(1);
    }

    //RemoveAndSaveAsync when id < 1 should return CommandStatus.NotFound
    [Fact]
    public async Task RemoveAndSaveAsync_ExpressionFuncPingBookBool_ShouldReturnCommandStatusNotFound()
    {
        //Arrange
        DbContextOptions<PingLibraryDbContext> options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using PingLibraryDbContext context = new(options);

        context.Database.EnsureCreated();
        await context.AddRangeAsync(
     [

            new PingBook { Id = 4, Title = "Title 4" },
            new PingBook { Id = 5, Title = "Title 5" }
     ]);

        await context.SaveChangesAsync();
        PingDbBaseCommandHandler handler = new(context);

        //Act
        int result = await handler.DeleteAsync(x => x.Id < 0);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(0);
    }

    //Add test RemoveAndSaveAsync with parameter entity and entity added should return CommandStatus.Error
    [Fact]
    public async Task RemoveAndSaveAsync_PingBook_ShouldReturnCommandStatusError()
    {
        //Arrange
        DbContextOptions<PingLibraryDbContext> options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using PingLibraryDbContext context = new(options);

        context.Database.EnsureCreated();

        await context.AddRangeAsync(
  [

            new PingBook { Id = 4, Title = "Title 4" },
            new PingBook { Id = 5, Title = "Title 5" }
  ]);

        await context.SaveChangesAsync();

        PingBook entity = new() { Id = 8, Title = "Title 8" };

        await context.AddAsync(entity);

        PingDbBaseCommandHandler handler = new(context);

        //Act
        int result = await handler.DeleteAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(0);
    }

    //Add test AddAndSaveAsync with parameter entity and entity added should return CommandStatus.Created
    [Fact]
    public async Task AddAndSaveAsync_PingBook_ShouldReturnCommandStatusCreated()
    {
        //Arrange
        DbContextOptions<PingLibraryDbContext> options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using PingLibraryDbContext context = new(options);

        context.Database.EnsureCreated();


        PingDbBaseCommandHandler handler = new(context);


        PingBook entity = new() { Title = "Title 1" };

        //Act

        int result = await handler.SaveAsync(entity);


        //Assert
        result.Should().Be(1);

    }



    //Add test UpdateAndSaveAsync with parameter entity and entity updated should return CommandStatus.Sucess
    [Fact]
    public async Task UpdateAndSaveAsync_PingBook_ShouldReturnCommandStatusSucess()
    {
        //Arrange
        DbContextOptions<PingLibraryDbContext> options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using PingLibraryDbContext context = new(options);

        context.Database.EnsureCreated();

        PingBook entity = context.Set<PingBook>().First();

        entity.Title = "Title 2";

        PingDbBaseCommandHandler handler = new(context);

        //Act
        int result = await handler.SaveAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(1);
    }




    //Constructor with IProjector parameter should set Projector property
    [Fact]
    public void Constructor_IProjector_ShouldSetProjectorProperty()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        Olbrasoft.Mapping.Mapster.MapsterProjector projector = new(new Mapster.TypeAdapterConfig());

        //Act
        PingDbBaseCommandHandler handler = new(projector, context);

        //Assert
        handler.Projector.Should().Be(projector);
    }

    //Constructor with IMapper parameter should set Mapper property
    [Fact]
    public void Constructor_IMapper_ShouldSetMapperProperty()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        Olbrasoft.Mapping.Mapster.MapsterMapper mapper = new(new MapsterMapper.Mapper());

        //Act
        PingDbBaseCommandHandler handler = new(mapper, context);

        //Assert
        handler.Mapper.Should().Be(mapper);
    }

    //Constructor with IProjector and IMapper parameter should set Projector and Mapper property
    [Fact]
    public void Constructor_IProjector_IMapper_ShouldSetProjectorAndMapperProperty()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        Olbrasoft.Mapping.Mapster.MapsterProjector projector = new(new Mapster.TypeAdapterConfig());
        Olbrasoft.Mapping.Mapster.MapsterMapper mapper = new(new MapsterMapper.Mapper());

        //Act
        PingDbBaseCommandHandler handler = new(projector, mapper, context);

        //Assert
        handler.Projector.Should().Be(projector);
        handler.Mapper.Should().Be(mapper);
    }
}