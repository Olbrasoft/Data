namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbCommandHandlerTests
{
    //Constructor with PingLibraryDbContext se Property Context
    [Fact]
    public void Constructor_PingLibraryDbContext_ShouldSetContext()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        //Act
        var handler = new PingDbCommandHandler(context);

        //Assert
        context.Should().BeSameAs(handler.Context);
    }

    //Constructor with IProjector and PingLibraryDbContext set Property Projector and Context
    [Fact]
    public void Constructor_IProjector_PingLibraryDbContext_ShouldSetProjectorAndContext()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        var projector = new Mock<IProjector>().Object;

        //Act
        var handler = new PingDbCommandHandler(projector, context);

        //Assert
        projector.Should().BeSameAs(handler.Projector);
        context.Should().BeSameAs(handler.Context);
    }

    //Constructor with IMapper and PingLibraryDbContext set Property Mapper and Context
    [Fact]
    public void Constructor_IMapper_PingLibraryDbContext_ShouldSetMapperAndContext()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        var mapper = new Mock<IMapper>().Object;

        //Act
        var handler = new PingDbCommandHandler(mapper, context);

        //Assert
        mapper.Should().BeSameAs(handler.Mapper);
        context.Should().BeSameAs(handler.Context);
    }

    //Constructor with IProjector, IMapper and PingLibraryDbContext throw ArgumentNullException when IMapper is null
    [Fact]
    public void Constructor_IProjector_IMapper_PingLibraryDbContext_ShouldThrowArgumentNullExceptionWhenMapperIsNull()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        var projector = new Mock<IProjector>().Object;

        //Act
        Action action = () => new PingDbCommandHandler(projector, null!, context);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'mapper')");
    }

    //Constructor with IProjector, IMapper and PingLibraryDbContext set Property Projector, Mapper and Context
    [Fact]
    public void Constructor_IProjector_IMapper_PingLibraryDbContext_ShouldSetProjectorMapperAndContext()
    {
        //Arrange
        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        var projector = new Mock<IProjector>().Object;
        var mapper = new Mock<IMapper>().Object;

        //Act
        var handler = new PingDbCommandHandler(projector, mapper, context);

        //Assert
        projector.Should().BeSameAs(handler.Projector);
        mapper.Should().BeSameAs(handler.Mapper);
        context.Should().BeSameAs(handler.Context);
    }

    //SaveOneEntityAsync verify call SaveChangesAsync on Context
    [Fact]
    public async Task SaveOneEntityAsync_ShouldCallSaveChangesAsyncOnContext()
    {
        //Arrange
        var context = new Mock<PingLibraryDbContext>(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        var handler = new PingDbCommandHandler(context.Object);

        //Act
        await handler.SaveOneEntityAsync();

        //Assert
        context.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }


    //SaveChangesAsync return int  verify call SaveChangesAsync on Context
    [Fact]
    public async Task SaveChangesAsync_ShouldReturnIntAndCallSaveChangesAsyncOnContext()
    {
        //Arrange
        var context = new Mock<PingLibraryDbContext>(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        var handler = new PingDbCommandHandler(context.Object);

        //Act
        int result = await handler.SaveChangesAsync();

        //Assert
        result.Should().Be(0);
        context.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }


    //ThrowIfCommandIsNullOrCancellationRequested throw ArgumentNullException when command is null
    [Fact]
    public void ThrowIfCommandIsNullOrCancellationRequested_ShouldThrowArgumentNullExceptionWhenCommandIsNull()
    {
        //Arrange
        //Act
        Action action = () => PingDbCommandHandler.CallThrowIfCommandIsNullOrCancellationRequested(null!, default);

        //Assert

        action.Should().Throw<ArgumentNullException>();
    }

    //MapCommandToNewEntity verify call Mapper.MapSourceToNewDestination
    [Fact]
    public void MapCommandToNewEntity_ShouldCallMapperMapSourceToNewDestination()
    {
        //Arrange
        var command = new Mock<BaseCommand<string>>().Object;

        var mapper = new Mock<IMapper>();
        var handler = new PingDbCommandHandler(mapper.Object, new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options));

        //Act
        handler.MapCommandToNewEntity(command);
        //Assert
        mapper.Verify(m => m.MapTo<PingBook>(command), Times.Once);
    }

    //MapCommandToExistingEntity verify call Mappet.Map(source, destination)
    [Fact]
    public void MapCommandToExistingEntity_ShouldCallMapperMapSourceToExistingDestination()
    {
        //Arrange
        var command = new Mock<BaseCommand<string>>().Object;
        var entity = new Mock<PingBook>().Object;

        var mapper = new Mock<IMapper>();
        var handler = new PingDbCommandHandler(mapper.Object, new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options));

        //Act
        handler.MapCommandToExistingEntity(command, entity);
        //Assert
        mapper.Verify(m => m.Map(command, entity), Times.Once);
    }

    //MapTo verify call Mapper.MapTo
    [Fact]
    public void MapTo_ShouldCallMapperMapTo()
    {
        //Arrange
        var source = new Mock<object>().Object;

        var mapper = new Mock<IMapper>();
        var handler = new PingDbCommandHandler(mapper.Object, new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options));

        //Act
        handler.MapTo<object>(source);
        //Assert
        mapper.Verify(m => m.MapTo<object>(source), Times.Once);
    }

    //GetEntityState retur EntityState.Detached
    [Fact]
    public void GetEntityState_ShouldReturnEntityStateDetached()
    {
        //Arrange
        var entity = new PingBook();

        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase("PingDbBooks")
     .Options);


        var handler = new PingDbCommandHandler(context);

        //Act
        var result = handler.GetEntityState(entity);

        //Assert
        result.Should().Be(EntityState.Detached);
    }

    //GetEntityState retur EntityState.Added
    [Fact]
    public void GetEntityState_ShouldReturnEntityStateAdded()
    {
        //Arrange
        var entity = new PingBook();



        var context = new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase("PingDbBooks")
     .Options);
        context.Books.Add(entity);

        var handler = new PingDbCommandHandler(context);

        //Act
        var result = handler.GetEntityState(entity);

        //Assert
        result.Should().Be(EntityState.Added);
    }




}
