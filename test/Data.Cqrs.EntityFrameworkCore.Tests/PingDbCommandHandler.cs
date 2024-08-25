namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingDbCommandHandler : DbCommandHandler<PingLibraryDbContext, PingBook, BaseCommand<string>, string>
{
    public new PingLibraryDbContext Context
    {
        get { return base.Context; }
    }

    public new IProjector? Projector
    {
        get { return base.Projector; }
    }

    public new IMapper? Mapper
    {
        get { return base.Mapper; }
    }

    public PingDbCommandHandler(PingLibraryDbContext context) : base(context)
    {
    }

    public PingDbCommandHandler(IProjector projector, PingLibraryDbContext context) : base(projector, context)
    {
    }

    public PingDbCommandHandler(IMapper mapper, PingLibraryDbContext context) : base(mapper, context)
    {
    }

    public PingDbCommandHandler(IProjector projector, IMapper mapper, PingLibraryDbContext context) : base(projector, mapper, context)
    {
    }


    public new async Task<bool> SaveOneEntityAsync(CancellationToken token = default) => await base.SaveOneEntityAsync(token);

    public new async Task<int> SaveChangesAsync(CancellationToken token = default) => await base.SaveChangesAsync(token);


    public new EntityState GetEntityState(object entity) => base.GetEntityState(entity);


    public override Task<string> HandleAsync(BaseCommand<string> request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public static void CallThrowIfCommandIsNullOrCancellationRequested(BaseCommand<string> command, CancellationToken token) => ThrowIfCommandIsNullOrCancellationRequested(command, token);

    public new PingBook CreateEntity(BaseCommand<string> command) => base.CreateEntity(command);



    public new PingBook MapCommandToExistingEntity(BaseCommand<string> command, PingBook entity) => base.MapCommandToExistingEntity(command, entity);

    public new TDestination MapTo<TDestination>(object source) => base.MapTo<TDestination>(source);

    public new Task<int> InsertAsync(PingBook entity, CancellationToken token) => base.InsertAsync(entity, token);

}

