namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public abstract class DbQueryHandler<TQuery, TResult, TContext, TEntity> : QueryHandler<TQuery, TResult> where TQuery : IRequest<TResult> where TContext : DbContext where TEntity : class
{
    protected TContext Context { get; private set; }
    protected IQueryable<TEntity> Entities { get; private set; }

    protected DbQueryHandler(IProjector projector, TContext context) : base(projector)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Entities ??= Context.Set<TEntity>();
    }
}

public abstract class DbQueryHandler<TQuery, TContext, TEntity> : QueryHandler<TQuery> where TQuery : IRequest<bool> where TContext : DbContext where TEntity : class
{
    protected TContext Context { get; private set; }
    protected IQueryable<TEntity> Entities { get; private set; }

    protected DbQueryHandler(TContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Entities ??= Context.Set<TEntity>();
    }
}