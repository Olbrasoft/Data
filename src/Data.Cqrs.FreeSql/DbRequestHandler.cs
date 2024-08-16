namespace Olbrasoft.Data.Cqrs.FreeSql;

/// <summary>
/// Represents a base class for handling database requests.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <remarks>
/// Initializes a new instance of the DbRequestHandler class with the specified TContext instance.
/// </remarks>
/// <param name="context">The TContext instance used for database operations.</param>
public abstract class DbRequestHandler<TContext, TEntity, TRequest, TResult>(TContext context) : IRequestHandler<TRequest, TResult>
    where TEntity : class where TContext : DbContext where TRequest : IRequest<TResult>
{

    /// <summary>
    /// Gets or sets the IMapper instance used for object mapping.
    /// </summary>
    protected virtual IMapper? Mapper { get; }

    /// <summary>
    /// Gets the TContext instance used for database operations.
    /// </summary>
    protected virtual TContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Gets the ISelect<TEntity> instance for querying entities.
    /// </summary>
    protected virtual ISelect<TEntity> Select => GetSelect();

    /// <summary>
    /// Initializes a new instance of the DbRequestHandler class with the specified IMapper and TContext instances.
    /// </summary>
    /// <param name="mapper">The IMapper instance used for object mapping.</param>
    /// <param name="context">The TContext instance used for database operations.</param>
    public DbRequestHandler(IMapper mapper, TContext context) : this(context)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the specified request and returns the result.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The result of handling the request.</returns>
    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken token);

    /// <summary>
    /// Represents the select keyword from Structured Query Language.
    /// </summary>
    /// <returns>The ISelect<TEntity> instance for querying entities.</returns>
    private ISelect<TEntity> GetSelect() => Context.Set<TEntity>().Select;

    /// <summary>
    /// Gets the ISelect<TForeignEntity> instance for querying foreign entities.
    /// </summary>
    /// <typeparam name="TForeignEntity">The type of the foreign entity.</typeparam>
    /// <returns>The ISelect<TForeignEntity> instance for querying foreign entities.</returns>
    protected virtual ISelect<TForeignEntity> GetSelect<TForeignEntity>() where TForeignEntity : class => Context.Set<TForeignEntity>().Select;

    /// <summary>
    /// Creates a new ISelect<TEntity> instance with the specified condition.
    /// </summary>
    /// <param name="condition">The condition to filter the entities.</param>
    /// <returns>The new ISelect<TEntity> instance with the specified condition.</returns>
    protected virtual ISelect<TEntity> GetWhere(Expression<Func<TEntity, bool>> condition) => Select.Where(condition);

    /// <summary>
    /// Creates a new ISelect<TEntity> instance with the specified column selector for ascending order.
    /// </summary>
    /// <typeparam name="TMember">The type of the property/column to sort by ascending.</typeparam>
    /// <param name="columnSelector">The selector for the property/column to sort by ascending.</param>
    /// <returns>The new ISelect<TEntity> instance with the specified column selector for ascending order.</returns>
    protected virtual ISelect<TEntity> GetOrderBy<TMember>(Expression<Func<TEntity, TMember>> columnSelector) => Select.OrderBy(columnSelector);

    /// <summary>
    /// Creates a new ISelect<TEntity> instance with the specified column selector for descending order.
    /// </summary>
    /// <typeparam name="TMember">The type of the property/column to sort by descending.</typeparam>
    /// <param name="columnSelector">The selector for the property/column to sort by descending.</param>
    /// <returns>The new ISelect<TEntity> instance with the specified column selector for descending order.</returns>
    protected virtual ISelect<TEntity> GetOrderByDescending<TMember>(Expression<Func<TEntity, TMember>> columnSelector)
        => Select.OrderByDescending(columnSelector);

    /// <summary>
    /// Checks if any entity exists in the database.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    /// <returns>True if any entity exists, otherwise false.</returns>
    protected virtual Task<bool> ExistsAsync(CancellationToken token = default) => Select.AnyAsync(token);


    /// <summary>
    /// Checks if any entity exists in the database based on the specified condition.
    /// </summary>
    /// <param name="condition">The condition to filter the entities.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>True if any entity exists, otherwise false.</returns>
    protected virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token = default) => Select.AnyAsync(condition, token);



    #region GetEnumerableAsync

    /// <summary>
    /// Retrieves a list of entities asynchronously and maps them to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entities to.</typeparam>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of mapped entities.</returns>
    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
       where TDestination : new()
      => await Select.ToListAsync<TDestination>(token);

    /// <summary>
    /// Retrieves a list of entities asynchronously using the specified select statement and maps them to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entities to.</typeparam>
    /// <param name="select">The select statement to use.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of mapped entities.</returns>
    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<TEntity> select, CancellationToken token)
      where TDestination : new()
      => await select.ToListAsync<TDestination>(token);

    /// <summary>
    /// Retrieves a list of entities asynchronously based on the specified condition and maps them to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entities to.</typeparam>
    /// <param name="condition">The condition to filter the entities.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of mapped entities.</returns>
    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> condition, CancellationToken token)
     where TDestination : new()
      => await GetWhere(condition).ToListAsync<TDestination>(token);

    /// <summary>
    /// Retrieves a list of entities asynchronously using the specified select statement.
    /// </summary>
    /// <param name="select">The select statement to use.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of entities.</returns>
    protected virtual async Task<IEnumerable<TEntity>> GetEnumerableAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToListAsync(token);

    /// <summary>
    /// Retrieves a list of entities asynchronously based on the specified condition.
    /// </summary>
    /// <param name="condition">The condition to filter the entities.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of entities.</returns>
    protected virtual async Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token)
      => await GetWhere(condition).ToListAsync(token);

    /// <summary>
    /// Retrieves a list of entities asynchronously and maps them to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entities to.</typeparam>
    /// <param name="mapTo">The mapping expression.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of mapped entities.</returns>
    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, TDestination>> mapTo, CancellationToken token)
      where TDestination : new() => await Select.ToListAsync(mapTo, token);


    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, TDestination>> mapTo, ISelect<TEntity> select, CancellationToken token)
    where TDestination : new() => await select.ToListAsync(mapTo, token);



    #endregion GetEnumerableAsync

    #region GetOneOrNullAsync 

    /// <summary>
    /// Retrieves a single entity asynchronously based on the specified condition.
    /// </summary>
    /// <param name="condition">The condition to filter the entity.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    protected virtual async Task<TEntity> GetOneOrNullAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token)
      => await GetOneOrNullAsync(GetWhere(condition), token);

    /// <summary>
    /// Retrieves a single entity asynchronously based on the specified condition and maps it to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entity to.</typeparam>
    /// <param name="condition">The condition to filter the entity.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    protected virtual async Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> condition, CancellationToken token)
        where TDestination : new()
      => await GetOneOrNullAsync<TDestination>(GetWhere(condition), token);

    /// <summary>
    /// Retrieves a single entity asynchronously using the specified select statement.
    /// </summary>
    /// <param name="select">The select statement to use.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    protected virtual async Task<TEntity> GetOneOrNullAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToOneAsync(token);

    /// <summary>
    /// Retrieves a single entity asynchronously using the specified select statement and maps it to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entity to.</typeparam>
    /// <param name="select">The select statement to use.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    protected virtual async Task<TDestination> GetOneOrNullAsync<TDestination>(ISelect<TEntity> select, CancellationToken token) where TDestination : new()
        => await select.ToOneAsync<TDestination>(token);

    /// <summary>
    /// Retrieves a single entity asynchronously using the specified select statement and maps it to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entity to.</typeparam>
    /// <param name="select">The select statement to use.</param>
    /// <param name="mapTo">The mapping expression.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    protected virtual Task<TDestination> GetOneOrNullAsync<TDestination>(ISelect<TEntity> select, Expression<Func<TEntity, TDestination>> mapTo, CancellationToken token)
        where TDestination : new()
        => select.ToOneAsync(mapTo, token);

    /// <summary>
    /// Retrieves a list of entities asynchronously using the specified select statement and maps them to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entities to.</typeparam>
    /// <param name="select">The select statement to use.</param>
    /// <param name="mapTo">The mapping expression.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of mapped entities.</returns>
    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<TEntity> select, Expression<Func<TEntity, TDestination>> mapTo, CancellationToken token)
    where TDestination : new()
        => await select.ToListAsync(mapTo, token);

    /// <summary>
    /// Retrieves a list of entities asynchronously based on the specified condition and maps them to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entities to.</typeparam>
    /// <param name="condition">The condition to filter the entities.</param>
    /// <param name="mapTo">The mapping expression.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A list of mapped entities.</returns>
    protected virtual async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TDestination>> mapTo, CancellationToken token)
        where TDestination : new() => await GetWhere(condition).ToListAsync(mapTo, token);

    /// <summary>
    /// Retrieves a single entity asynchronously based on the specified condition and maps it to the specified type.
    /// </summary>
    /// <typeparam name="TDestination">The type to map the entity to.</typeparam>
    /// <param name="condition">The condition to filter the entity.</param>
    /// <param name="mapTo">The mapping expression.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    protected virtual Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TDestination>> mapTo, CancellationToken token)
       where TDestination : new() => GetOneOrNullAsync(GetWhere(condition), mapTo, token);

    #endregion GetOneOrNullAsync
}
