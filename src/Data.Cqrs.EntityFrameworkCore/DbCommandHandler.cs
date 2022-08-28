namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public abstract class DbCommandHandler<TCommand, TResult, TContext, TEntity> : CommandHandler<TCommand, TResult>
    where TCommand : IRequest<TResult> where TContext : DbContext where TEntity : class
{
    protected TContext Context { get; private set; }

    protected DbSet<TEntity> Entities { get; private set; }

    protected DbCommandHandler(IMapper mapper, TContext context) : base(mapper)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));

        Entities = Context.Set<TEntity>();
    }
}

public abstract class DbCommandHandler<TCommand, TContext, TEntity> : DbCommandHandler<TCommand, bool, TContext, TEntity>
    where TCommand : IRequest<bool> where TContext : DbContext where TEntity : class
{
    protected DbCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }
}