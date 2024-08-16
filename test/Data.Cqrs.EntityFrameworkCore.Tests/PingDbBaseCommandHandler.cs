using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Data.Cqrs.EntityFrameworkCore.Tests;

internal class PingDbBaseCommandHandler : DbBaseCommandHandler<PingLibraryDbContext, PingBook, PingBookBaseCommand, PingBook>
{

    public new IProjector? Projector { get { return base.Projector; } }
    public new IMapper? Mapper { get { return base.Mapper; } }

    public PingDbBaseCommandHandler(PingLibraryDbContext context) : base(context)
    {
    }



    public PingDbBaseCommandHandler(IProjector projector, PingLibraryDbContext context) : base(projector, context)
    {
    }

    public PingDbBaseCommandHandler(IMapper mapper, PingLibraryDbContext context) : base(mapper, context)
    {
    }

    public PingDbBaseCommandHandler(IProjector projector, IMapper mapper, PingLibraryDbContext context) : base(projector, mapper, context)
    {
    }

    public override Task<PingBook> HandleAsync(PingBookBaseCommand request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public new void ThrowIfCommandStatusCannotBeSet(CommandStatus status) => base.ThrowIfCommandStatusCannotBeSet(status);


    public new async Task<CommandStatus> RemoveOneAndSaveAsync(Expression<Func<PingBook, bool>> exp, CancellationToken token = default) => await base.RemoveOneAndSaveAsync(exp, token);


    public new async Task<CommandStatus> RemoveOneAndSaveAsync(PingBook detachedOrUnchangedEntity, CancellationToken token = default) => await base.RemoveOneAndSaveAsync(detachedOrUnchangedEntity, token);

    public new async Task<CommandStatus> AddAndSaveAsync(PingBook detachedEntity, CancellationToken token = default) => await base.AddAndSaveAsync(detachedEntity, token);

    public new async Task<CommandStatus> UpdateAndSaveAsync(PingBook unchangedEntity, CancellationToken token = default) => await base.UpdateAndSaveAsync(unchangedEntity, token);

    public new EntityEntry<PingBook> Update(PingBook entity) => base.Update(entity);

    public new EntityState GetEntityState(object entity) => base.GetEntityState(entity);


    public new async Task<CommandStatus> SaveAsync(PingBook modifiedEntity, CancellationToken token = default) => await base.SaveAsync(modifiedEntity, token);
}