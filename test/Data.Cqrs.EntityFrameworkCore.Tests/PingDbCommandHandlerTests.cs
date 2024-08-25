namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbCommandHandlerTests
{
    //Constructor with PingLibraryDbContext se Property Context
    [Fact]
    public void Constructor_PingLibraryDbContext_ShouldSetContext()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);

        //Act
        PingDbCommandHandler handler = new(context);

        //Assert
        context.Should().BeSameAs(handler.Context);
    }

    //Constructor with IProjector and PingLibraryDbContext set Property Projector and Context
    [Fact]
    public void Constructor_IProjector_PingLibraryDbContext_ShouldSetProjectorAndContext()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        IProjector projector = new Mock<IProjector>().Object;

        //Act
        PingDbCommandHandler handler = new(projector, context);

        //Assert
        projector.Should().BeSameAs(handler.Projector);
        context.Should().BeSameAs(handler.Context);
    }

    //Constructor with IMapper and PingLibraryDbContext set Property Mapper and Context
    [Fact]
    public void Constructor_IMapper_PingLibraryDbContext_ShouldSetMapperAndContext()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        IMapper mapper = new Mock<IMapper>().Object;

        //Act
        PingDbCommandHandler handler = new(mapper, context);

        //Assert
        mapper.Should().BeSameAs(handler.Mapper);
        context.Should().BeSameAs(handler.Context);
    }

    //Constructor with IProjector, IMapper and PingLibraryDbContext throw ArgumentNullException when IMapper is null
    [Fact]
    public void Constructor_IProjector_IMapper_PingLibraryDbContext_ShouldThrowArgumentNullExceptionWhenMapperIsNull()
    {
        //Arrange
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        IProjector projector = new Mock<IProjector>().Object;

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
        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        IProjector projector = new Mock<IProjector>().Object;
        IMapper mapper = new Mock<IMapper>().Object;

        //Act
        PingDbCommandHandler handler = new(projector, mapper, context);

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
        Mock<PingLibraryDbContext> context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        PingDbCommandHandler handler = new(context.Object);

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
        Mock<PingLibraryDbContext> context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        PingDbCommandHandler handler = new(context.Object);

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

    //CreateEntity verify call Mapper.MapSourceToNewDestination
    [Fact]
    public void MapCommandToNewEntity_ShouldCallMapperMapSourceToNewDestination()
    {
        //Arrange
        BaseCommand<string> command = new Mock<BaseCommand<string>>().Object;

        Mock<IMapper> mapper = new();
        PingDbCommandHandler handler = new(mapper.Object, new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options));

        //Act
        handler.CreateEntity(command);
        //Assert
        mapper.Verify(m => m.MapTo<PingBook>(command), Times.Once);
    }

    //MapCommandToExistingEntity verify call Mappet.Map(source, destination)
    [Fact]
    public void MapCommandToExistingEntity_ShouldCallMapperMapSourceToExistingDestination()
    {
        //Arrange
        BaseCommand<string> command = new Mock<BaseCommand<string>>().Object;
        PingBook entity = new Mock<PingBook>().Object;

        Mock<IMapper> mapper = new();
        PingDbCommandHandler handler = new(mapper.Object, new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options));

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
        object source = new Mock<object>().Object;

        Mock<IMapper> mapper = new();
        PingDbCommandHandler handler = new(mapper.Object, new PingLibraryDbContext(new DbContextOptionsBuilder<PingLibraryDbContext>().Options));

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
        PingBook entity = new();

        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase("PingDbBooks")
     .Options);


        PingDbCommandHandler handler = new(context);

        //Act
        EntityState result = handler.GetEntityState(entity);

        //Assert
        result.Should().Be(EntityState.Detached);
    }

    //GetEntityState retur EntityState.Added
    [Fact]
    public void GetEntityState_ShouldReturnEntityStateAdded()
    {
        //Arrange
        PingBook entity = new();



        PingLibraryDbContext context = new(new DbContextOptionsBuilder<PingLibraryDbContext>()
     .UseInMemoryDatabase("PingDbBooks")
     .Options);
        context.Books.Add(entity);

        PingDbCommandHandler handler = new(context);

        //Act
        EntityState result = handler.GetEntityState(entity);

        //Assert
        result.Should().Be(EntityState.Added);
    }

    //InsertAsync verify call AddAsync on Context and verify call SaveChangesAsync on Context
    [Fact]
    public async Task InsertAsync_ShouldCallAddAsyncAndSaveChangesAsyncOnContext()
    {
        //Arrange
        PingBook entity = new();
        Mock<PingLibraryDbContext> context = new(new DbContextOptionsBuilder<PingLibraryDbContext>().Options);
        PingDbCommandHandler handler = new(context.Object);

        //Act
        await handler.InsertAsync(entity, default);

        //Assert
        context.Verify(c => c.Add(entity), Times.Once);
        context.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }


}
