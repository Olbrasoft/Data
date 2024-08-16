namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public abstract class DbQueryHandler<TContext, TEntity, TQuery, TResult> : DbRequestHandler<TContext, TEntity, TQuery, TResult>, IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    where TContext : DbContext where TEntity : class
{
    protected DbQueryHandler(TContext context) : base(context)
    {
    }

    protected DbQueryHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        token.ThrowIfCancellationRequested();
    }
}
