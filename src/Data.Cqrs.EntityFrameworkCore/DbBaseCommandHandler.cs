using Olbrasoft.Data.Entities.Abstractions;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

/// <summary>
/// Base class for command handlers that interact with a database using Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">The type of the DbContext.</typeparam>
/// <typeparam name="TEntity">The type of the entityToAdd.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class DbBaseCommandHandler<TContext, TEntity, TCommand, TResult> : DbCommandHandler<TContext, TEntity, TCommand, TResult>, ICommandHandler<TCommand, TResult>
   where TContext : DbContext where TEntity : BaseEnity where TCommand : ICommand<TResult>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="DbBaseCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="context">The DbContext instance.</param>
    protected DbBaseCommandHandler(TContext context) : base(context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbBaseCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="projector">The projector instance.</param>
    /// <param name="context">The DbContext instance.</param>
    protected DbBaseCommandHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbBaseCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="mapper">The mapper instance.</param>
    /// <param name="context">The DbContext instance.</param>
    protected DbBaseCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbBaseCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="projector">The projector instance.</param>
    /// <param name="mapper">The mapper instance.</param>
    /// <param name="context">The DbContext instance.</param>
    protected DbBaseCommandHandler(IProjector projector, IMapper mapper, TContext context) : base(projector, mapper, context)
    {
    }

    /// <summary>
    /// Saves the entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning the number of affected rows.</returns>
    protected virtual Task<int> SaveAsync(TEntity entity, CancellationToken token)
    {
        return base.UpdateAsync(entity, token);
    }

    /// <summary>
    /// Deletes the entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning the number of affected rows.</returns>
    protected virtual Task<int> DeleteAsync(TEntity entity, CancellationToken token)
    {
        Context.Remove(entity);
        return Context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Deletes entities that match the specified condition asynchronously.
    /// </summary>
    /// <param name="condition">The condition to match.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning the number of affected rows.</returns>
    protected virtual Task<int> DeleteAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token)
    {
        Context.RemoveRange(Where(condition));
        return Context.SaveChangesAsync(token);
    }
}

