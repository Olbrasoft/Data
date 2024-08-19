namespace Olbrasoft.Data.Cqrs.FreeSql;

/// <summary>
/// Represents a base class for database command handlers.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class DbCommandHandler<TContext, TEntity, TCommand, TResult> : DbRequestHandler<TContext, TEntity, TCommand, TResult>
   where TEntity : class where TContext : DbContext where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for the specified entity type.
    /// </summary>
    protected virtual DbSet<TEntity> Entities => Context.Set<TEntity>();


    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    protected DbCommandHandler(TContext context) : base(context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="mapper">The mapper instance.</param>
    /// <param name="context">The database context.</param>
    protected DbCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for the specified entity type.
    /// </summary>
    /// <typeparam name="TForeignEntity">The type of the entity.</typeparam>
    /// <returns>The <see cref="DbSet{TEntity}"/> for the specified entity type.</returns>
    protected DbSet<TForeignEntity> GetSet<TForeignEntity>() where TForeignEntity : class => Context.Set<TForeignEntity>();

    /// <summary>
    /// Saves a single entity asynchronously.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The task result is true if the entity was saved successfully, otherwise false.</returns>
    protected virtual async Task<bool> SaveOneEntityAsync(CancellationToken token) => await Context.SaveChangesAsync(token) == 1;

    /// <summary>
    /// Throws an exception if the command is null or cancellation is requested.
    /// </summary>
    /// <param name="command">The command object.</param>
    /// <param name="token">The cancellation token.</param>
    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        if (command is not null)
            token.ThrowIfCancellationRequested();
        else
            throw new ArgumentNullException(nameof(command));
    }

    /// <summary>
    /// Maps the source object to the specified destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="source">The source object to map from.</param>
    /// <returns>The mapped destination object.</returns>
    protected virtual TDestination MapTo<TDestination>(object source)
    {
        return GetMapper().MapTo<TDestination>(source);
    }

    /// <summary>
    /// Maps the command object to a new entity.
    /// </summary>
    /// <param name="command">The command object to map from.</param>
    /// <returns>The mapped new entity.</returns>
    protected virtual TEntity MapCommandToNewEntity(TCommand command) => GetMapper().MapSourceToNewDestination<TEntity>(command);

    /// <summary>
    /// Execute a mapping from the command to the existing entity.
    /// </summary>
    /// <param name="command">Command object to map from</param>
    /// <param name="entity">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the <paramref name="entity"/> object and returns.</returns>
    protected virtual TEntity MapCommandToExistingEntity(TCommand command, TEntity entity)
    {
        GetMapper().MapSourceToExistingDestination(command, entity);
        return entity;
    }

    /// <summary>
    /// Gets the mapper instance.
    /// </summary>
    /// <returns>The mapper instance.</returns>
    protected virtual IMapper GetMapper() => Mapper is null ? throw new NullReferenceException(nameof(Mapper)) : Mapper;
}

