using System.Linq.Expressions;

namespace Data.Cqrs.EntityFrameworkCore.Tests
{
    internal class PingDbBaseCommandHandler : DbBaseCommandHandler<PingLibraryDbContext, PingBook, PingBookBaseCommand, PingBook>
    {
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

        public new IProjector? Projector { get { return base.Projector; } }
        public new IMapper? Mapper { get { return base.Mapper; } }

        public override Task<PingBook> HandleAsync(PingBookBaseCommand request, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public new async Task<int> DeleteAsync(Expression<Func<PingBook, bool>> exp, CancellationToken token = default)
        {
            return await base.DeleteAsync(exp, token);
        }

        public new async Task<int> DeleteAsync(PingBook detachedOrUnchangedEntity, CancellationToken token = default)
        {
            return await base.DeleteAsync(detachedOrUnchangedEntity, token);
        }

        public new async Task<int> SaveAsync(PingBook detachedEntity, CancellationToken token = default)
        {
            return await base.SaveAsync(detachedEntity, token);
        }

        public new async Task<int> UpdateAsync(PingBook unchangedEntity, CancellationToken token = default)
        {
            return await base.UpdateAsync(unchangedEntity, token);
        }

        public new EntityState GetEntityState(object entity)
        {
            return base.GetEntityState(entity);
        }
    }
}