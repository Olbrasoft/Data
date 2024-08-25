using Olbrasoft.Mediation.Abstractions;


namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

/// <summary>
/// Represents a base class for handling database requests using Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">The type of the database context</typeparam>
/// <typeparam name="TEntity">The type of the entity</typeparam>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResult">The type of the result</typeparam>
/// <param name="context">The database context</param>
public abstract class DbRequestHandler<TContext, TEntity, TRequest, TResult>(TContext context) : IRequestHandler<TRequest, TResult>
    where TEntity : class where TContext : DbContext where TRequest : IRequest<TResult>
{

    /// <summary>
    /// The entities in the database
    /// </summary>
    private DbSet<TEntity>? _entities;

    /// <summary>
    /// The database context
    /// </summary>
    protected virtual TContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));


    /// <summary>
    /// The entities in the database
    /// </summary>
    protected DbSet<TEntity> Entities { get => _entities is null ? Context.Set<TEntity>() : _entities; private set => _entities = value; }

    /// <summary>
    /// The projector used for projecting entities
    /// </summary>
    protected IProjector? Projector { get; }


    /// <summary>
    /// Represents a base class for handling database requests using Entity Framework Core.
    /// </summary>
    /// <param name="projector">The projector used for projecting entities</param>
    /// <param name="context">The database context</param>
    /// <exception cref="ArgumentNullException">Thrown when the projector is null</exception>
    public DbRequestHandler(IProjector projector, TContext context) : this(context)
    => Projector = projector ?? throw new ArgumentNullException(nameof(projector));

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="condition">lambda condition</param>
    /// <returns>Queryable where</returns>
    protected IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> condition) => Entities.Where(condition);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by ascending</typeparam>
    /// <param name="columnSelector">Selector property/column for ascending order</param>
    /// <returns>Ascending selection</returns>
    protected IOrderedQueryable<TEntity> OrderBy<TMember>(Expression<Func<TEntity, TMember>> columnSelector) => Entities.OrderBy(columnSelector);

    /// <summary>
    /// Sort by column descending，OrderByDescending(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by descending</typeparam>
    /// <param name="columnSelector">Selector property/column for descending order</param>
    /// <returns>Descending selection</returns>
    protected IOrderedQueryable<TEntity> OrderByDescending<TMember>(Expression<Func<TEntity, TMember>> columnSelector)
        => Entities.OrderByDescending(columnSelector);

    #region ExistsAsync

    /// <summary>
    /// Checks if any entity exists in the database.
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>True if any entity exists, otherwise false</returns>
    protected Task<bool> AnyAsync(CancellationToken token = default) => Entities.AnyAsync(token);

    /// <summary>
    /// Checks if any entity exists in the database that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter the entities</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>True if any entity exists, otherwise false</returns>
    protected Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        => Entities.AnyAsync(predicate, token);

    #endregion

    #region GetEnumerableAsync

    /// <summary>
    /// Retrieves a collection of entities from the database and projects them to the specified destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination</typeparam>
    /// <param name="token">Cancellation token</param>
    /// <returns>Collection of projected entities</returns>
    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token = default)
       where TDestination : new()
      => await ProjectTo<TDestination>(Entities).ToArrayAsync(token);


    /// <summary>
    /// Retrieves a collection of entities from the specified queryable and projects them to the specified destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination</typeparam>
    /// <param name="queryable">The queryable to retrieve entities from</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Collection of projected entities</returns>
    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(IQueryable queryable, CancellationToken token = default)
     where TDestination : new()
     => await ProjectTo<TDestination>(queryable).ToArrayAsync(token);


    ///<summary>
    /// Retrieves a collection of entities from the specified queryable.
    /// </summary>
    /// <param name="queryable">The queryable to retrieve entities from</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Collection of entities</returns>
    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(IQueryable<TEntity> queryable, CancellationToken token = default)
     => await queryable.ToArrayAsync(token);

    /// <summary>
    /// Retrieves a collection of entities from the database that match the specified condition and projects them to the specified destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination</typeparam>
    /// <param name="expression">The condition to filter the entities</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Collection of projected entities</returns>
    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
     where TDestination : new()
      => await ProjectTo<TDestination>(Where(expression)).ToArrayAsync(token);

    /// <summary>
    /// Retrieves a collection of entities from the database that match the specified condition.
    /// </summary>
    /// <param name="expression">The condition to filter the entities</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Collection of entities</returns>
    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token)
     => await Where(expression).ToArrayAsync(token);

    #endregion GetEnumerableAsync


    protected virtual async Task<TForeignEntity[]> GetArrayAsync<TForeignEntity>(Expression<Func<TForeignEntity, bool>> condition, CancellationToken token = default) where TForeignEntity : class
    {
        return await Context.Set<TForeignEntity>().Where(condition).ToArrayAsync(token);
    }




    #region GetOneOrNullAsync

    /// <summary>
    /// Retrieves a single entity from the specified queryable or null if no entity is found.
    /// </summary>
    /// <param name="queryable">The queryable to retrieve the entity from</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The retrieved entity or null</returns>
    protected Task<TEntity?> GetOneOrNullAsync(IQueryable<TEntity> queryable, CancellationToken token = default)
          => queryable.SingleOrDefaultAsync(token);

    /// <summary>
    /// Retrieves a single entity from the specified queryable and projects it to the specified destination type or null if no entity is found.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination</typeparam>
    /// <param name="queryable">The queryable to retrieve the entity from</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The projected entity or null</returns>
    protected Task<TDestination?> GetOneOrNullAsync<TDestination>(IQueryable<TEntity> queryable, CancellationToken token = default) where TDestination : new()
          => ProjectTo<TDestination>(queryable).SingleOrDefaultAsync(token);

    /// <summary>
    /// Retrieves a single entity from the database that matches the specified condition or null if no entity is found.
    /// </summary>
    /// <param name="expression">The condition to filter the entity</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The retrieved entity or null</returns>
    protected Task<TEntity?> GetOneOrNullAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token)
    => GetOneOrNullAsync(Where(expression), token);


    /// <summary>
    /// Retrieves a single entity from the database that matches the specified condition and projects it to the specified destination type or null if no entity is found.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination</typeparam>
    /// <param name="condition">The condition to filter the entity</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The projected entity or null</returns>
    protected Task<TDestination?> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> condition, CancellationToken token = default)
    where TDestination : new()
         => GetOneOrNullAsync<TDestination>(Where(condition), token);

    #endregion GetOneOrNullAsync



    /// <summary>
    /// Handles the database request and returns the result.
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The result of the request</returns>
    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken token);



    /// <summary>
    /// Projects the source queryable to the specified destination type using the configured projector.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination</typeparam>
    /// <param name="source">The source queryable</param>
    /// <returns>The projected queryable</returns>
    /// <exception cref="NullReferenceException">Thrown when the projector is null</exception>
    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
       => Projector is null ? throw new NullReferenceException(nameof(Projector)) : Projector.ProjectTo<TDestination>(source);


    protected virtual IQueryable<TDestination> ProjectTo<TDestination>(Expression<Func<TEntity, bool>> condition)
        => ProjectTo<TDestination>(Where(condition));

}
