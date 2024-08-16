namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbBaseCommandHandlerTests
{

    //Inherits from DbBaseCommandHandler<PingLibraryDbContext, PingBook, BaseCommand<PingBook>, PingBook>
    [Fact]
    public void InheritsFromDbBaseCommandHandler_PingLibraryDbContext_PingBook_BaseCommand_PingBook()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        //Act
        var handler = new PingDbBaseCommandHandler(context);

        //Assert
        handler.Should().BeAssignableTo<DbBaseCommandHandler<PingLibraryDbContext, PingBook, PingBookBaseCommand, PingBook>>();
    }

    ///add test RemoveAndSaveAsync(Expression<Func<PingBook, bool>> exp, CancellationToken token = default) new PingLibraryDbContext in memmory add five PingBook and  RemoveAndSaveAsync when id > 2 should return CommandStatus.Deleted
    [Fact]
    public async Task RemoveAndSaveAsync_ExpressionFuncPingBookBool_ShouldReturnCommandStatusDeleted()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();


        await context.AddRangeAsync(
        [

            new PingBook { Id = 4, Title = "Title 4" },
            new PingBook { Id = 5, Title = "Title 5" }
        ]);


        await context.SaveChangesAsync();

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.RemoveOneAndSaveAsync(x => x.Id == 2);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.Deleted);
    }

    //RemoveAndSaveAsync when id < 1 should return CommandStatus.NotFound
    [Fact]
    public async Task RemoveAndSaveAsync_ExpressionFuncPingBookBool_ShouldReturnCommandStatusNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();
        await context.AddRangeAsync(
     [

            new PingBook { Id = 4, Title = "Title 4" },
            new PingBook { Id = 5, Title = "Title 5" }
     ]);

        await context.SaveChangesAsync();
        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.RemoveOneAndSaveAsync(x => x.Id < 0);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.NotFound);
    }

    //Add test RemoveAndSaveAsync with parameter entity and entity added should return CommandStatus.Error
    [Fact]
    public async Task RemoveAndSaveAsync_PingBook_ShouldReturnCommandStatusError()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();

        await context.AddRangeAsync(
  [

            new PingBook { Id = 4, Title = "Title 4" },
            new PingBook { Id = 5, Title = "Title 5" }
  ]);

        await context.SaveChangesAsync();

        var entity = new PingBook { Id = 8, Title = "Title 8" };

        await context.AddAsync(entity);

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.RemoveOneAndSaveAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.Error);
    }

    //Add test AddAndSaveAsync with parameter entity and entity added should return CommandStatus.Created
    [Fact]
    public async Task AddAndSaveAsync_PingBook_ShouldReturnCommandStatusCreated()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();


        var handler = new PingDbBaseCommandHandler(context);


        var entity = new PingBook { Title = "Title 1" };

        //Act

        var result = await handler.AddAndSaveAsync(entity);


        //Assert
        result.Should().Be(CommandStatus.Created);

    }

    //Add test AddAndSaveAsync with parameter entity and entity added should return CommandStatus.Error
    [Fact]
    public async Task AddAndSaveAsync_PingBook_ShouldReturnCommandStatusError()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();

        var entity = new PingBook { Title = "Title 1" };

        await context.AddAsync(entity);

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.AddAndSaveAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.Error);
    }


    //Add test Update with parameter entity and entity updated should return EntityState.Modified
    [Fact]
    public void Update_PingBook_ShouldReturnEntityStateModified()
    {
        //Arrange
        var entity = new PingBook { Title = "Title 1" };

        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase("PingDbBooks")
     .Options);

        context.Database.EnsureCreated();

        context.Books.Add(entity);

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = handler.Update(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.State.Should().Be(EntityState.Modified);
    }


    //Add test UpdateAndSaveAsync with parameter entity and entity updated should return CommandStatus.Sucess
    [Fact]
    public async Task UpdateAndSaveAsync_PingBook_ShouldReturnCommandStatusSucess()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();

        var entity = context.Set<PingBook>().First();

        entity.Title = "Title 2";

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.UpdateAndSaveAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.Success);
    }

    //Add test SaveAsync with parameter entity and entity not updated should return CommandStatus.Unchanged
    [Fact]
    public async Task SaveAsync_PingBook_ShouldReturnCommandStatusUnchanged()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();

        var entity = context.Set<PingBook>().First();


        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.SaveAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.Unchanged);
    }

    //Add test SaveAsync with parameter entity and entity Detached should return State error
    [Fact]
    public async Task SaveAsync_PingBook_ShouldReturnCommandStatusError()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<PingLibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new PingLibraryDbContext(options);

        context.Database.EnsureCreated();

        var entity = new PingBook { Id = 1, Title = "Title 1" };

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        var result = await handler.SaveAsync(entity);

        context.Database.EnsureDeleted();

        //Assert
        result.Should().Be(CommandStatus.Error);
    }

    //add test ThrowIfCommandStatusCannotBeSet with parameter CommandStatus.Created should throw exception
    [Fact]
    public void ThrowIfCommandStatusCannotBeSet_CommandStatusCreated_ShoulThrowException()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        var handler = new PingDbBaseCommandHandler(context);

        //Act
        Action act = () => handler.ThrowIfCommandStatusCannotBeSet(CommandStatus.Created);

        //Assert
        act.Should().Throw<InvalidOperationException>();
    }


    //Constructor with IProjector parameter should set Projector property
    [Fact]
    public void Constructor_IProjector_ShouldSetProjectorProperty()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());

        //Act
        var handler = new PingDbBaseCommandHandler(projector, context);

        //Assert
        handler.Projector.Should().Be(projector);
    }

    //Constructor with IMapper parameter should set Mapper property
    [Fact]
    public void Constructor_IMapper_ShouldSetMapperProperty()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        var mapper = new Olbrasoft.Mapping.Mapster.MapsterMapper(new MapsterMapper.Mapper());

        //Act
        var handler = new PingDbBaseCommandHandler(mapper, context);

        //Assert
        handler.Mapper.Should().Be(mapper);
    }

    //Constructor with IProjector and IMapper parameter should set Projector and Mapper property
    [Fact]
    public void Constructor_IProjector_IMapper_ShouldSetProjectorAndMapperProperty()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        var projector = new Olbrasoft.Mapping.Mapster.MapsterProjector(new Mapster.TypeAdapterConfig());
        var mapper = new Olbrasoft.Mapping.Mapster.MapsterMapper(new MapsterMapper.Mapper());

        //Act
        var handler = new PingDbBaseCommandHandler(projector, mapper, context);

        //Assert
        handler.Projector.Should().Be(projector);
        handler.Mapper.Should().Be(mapper);
    }
}