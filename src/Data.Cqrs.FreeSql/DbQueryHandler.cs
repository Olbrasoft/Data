namespace Olbrasoft.Data.Cqrs.FreeSql;

public abstract class DbQueryHandler<TContext, TEntity, TQuery, TResult> : DbRequestHandler<TContext, TEntity, TQuery, TResult>
   where TEntity : class where TContext : DbContext where TQuery : IQuery<TResult>
{
    protected DbQueryHandler(TContext context) : base(context)
    {
    }

    protected DbQueryHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is not null) token.ThrowIfCancellationRequested();
        else
            throw new ArgumentNullException(nameof(query));
    }
}


/// <summary>
/// Represents an abstract base class for handling database query operations.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <param name="context">The database context.</param>
/// <seealso cref="DbQueryHandler{TContext, TEntity, TQuery, TResult}"/>
public abstract class DbQueryHandler<TContext, TEntity, TQuery>(TContext context) : DbQueryHandler<TContext, TEntity, TQuery, bool>(context)
    where TEntity : class
    where TContext : DbContext
    where TQuery : IQuery<bool>
{
}
