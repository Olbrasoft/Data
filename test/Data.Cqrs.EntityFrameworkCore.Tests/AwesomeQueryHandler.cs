namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

internal class AwesomeQueryHandler : DbQueryHandler<Request<object>, object, DbContext, AwesomeEntity>
{
    public new DbContext Context => base.Context;

    public new IQueryable<AwesomeEntity> Entities => base.Entities;

    public AwesomeQueryHandler(IProjector projector, DbContext context) : base(projector, context)
    {
    }

    public override Task<object> HandleAsync(Request<object> query, CancellationToken token)
    {
        throw new System.NotImplementedException();
    }
}