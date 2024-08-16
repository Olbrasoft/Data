using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

/// <summary>
/// Base class for command handlers that interact with a database using Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">The type of the DbContext.</typeparam>
/// <typeparam name="TEntity">The type of the entityToAdd.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class DbBaseCommandHandler<TContext, TEntity, TCommand, TResult> : DbCommandHandler<TContext, TEntity, TCommand, TResult>, ICommandHandler<TCommand, TResult>
   where TContext : DbContext where TEntity : class where TCommand : BaseCommand<TResult>
{
    protected TCommand? Command { get; set; }

    protected DbBaseCommandHandler(TContext context) : base(context)
    {
    }

    protected DbBaseCommandHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

    protected DbBaseCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    protected DbBaseCommandHandler(IProjector projector, IMapper mapper, TContext context) : base(projector, mapper, context)
    {
    }


    protected void ThrowIfCommandStatusCannotBeSet(CommandStatus status)
    {
        if (TrySetCommandStatus(status))
            return;
        throw new InvalidOperationException($"Failed to set command status: {status}");
    }


    protected bool TrySetCommandStatus(CommandStatus status)
    {
        if (Command is null) return false;

        Command.Status = status;

        return true;
    }



    /// <summary>
    /// Removes one entity that matches the specified expression and saves the changes asynchronously.
    /// </summary>
    /// <param name="oneEntityPredicate">The expression to match the entity to remove. If more than one entity matches the expression, an exception is thrown</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The command status indicating the result of the operation.</returns>
    protected virtual async Task<CommandStatus> RemoveOneAndSaveAsync(Expression<Func<TEntity, bool>> oneEntityPredicate, CancellationToken token = default)
    {
        var entity = await GetOneOrNullAsync(oneEntityPredicate, token);

        if (entity is not null) return await RemoveOneAndSaveAsync(entity, token);

        TrySetCommandStatus(CommandStatus.NotFound);

        return CommandStatus.NotFound;
    }

    /// <summary>
    /// Removes one entityToAdd that matches the specified expression and saves the changes asynchronously.
    /// </summary>
    /// <param name="detachedOrUnchangedEntityForRemove">The entityToAdd to remove.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The command status indicating the result of the operation.</returns>
    protected virtual async Task<CommandStatus> RemoveOneAndSaveAsync(TEntity detachedOrUnchangedEntityForRemove, CancellationToken token = default)
    {
        var state = GetEntityState(detachedOrUnchangedEntityForRemove);

        if (state == EntityState.Detached || state == EntityState.Unchanged)
        {
            Remove(detachedOrUnchangedEntityForRemove);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Deleted);
                return CommandStatus.Deleted;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }


    /// <summary>
    /// Adds the specified entityToAdd to the context and saves the changes asynchronously.
    /// </summary>
    /// <param name="detachedEntityToAdd">The detached entityToAdd to add.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The command status indicating the result of the operation.</returns>
    protected virtual async Task<CommandStatus> AddAndSaveAsync(TEntity detachedEntityToAdd, CancellationToken token = default)
    {
        if (GetEntityState(detachedEntityToAdd) == EntityState.Detached)
        {
            await AddAsync(detachedEntityToAdd, token);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Created);
                return CommandStatus.Created;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    /// <summary>
    /// Updates the specified entityToAdd in the context and saves the changes asynchronously.
    /// </summary>
    /// <param name="unchangedEntityToUpdate">The unchanged entityToAdd to update.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The command status indicating the result of the operation.</returns>
    protected virtual async Task<CommandStatus> UpdateAndSaveAsync(TEntity unchangedEntityToUpdate, CancellationToken token = default)
    {
        Entities.Update(unchangedEntityToUpdate);

        TrySetCommandStatus(CommandStatus.Modified);

        return await SaveAsync(unchangedEntityToUpdate, token);
    }


    /// <summary>
    /// Saves the changes asynchronously for the specified entityToAdd and sets the command status accordingly.
    /// </summary>
    /// <param name="modifiedEntityToSave">The modified entityToAdd to save.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The command status indicating the result of the operation.</returns>
    protected virtual async Task<CommandStatus> SaveAsync(TEntity modifiedEntityToSave, CancellationToken token = default)
    {
        var state = GetEntityState(modifiedEntityToSave);

        if (state == EntityState.Unchanged)
        {
            TrySetCommandStatus(CommandStatus.Unchanged);
            return CommandStatus.Unchanged;
        }

        if (state == EntityState.Modified)
        {
            TrySetCommandStatus(CommandStatus.Modified);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Success);
                return CommandStatus.Success;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual EntityEntry<TEntity> Remove(TEntity entity)
    {
        var result = Entities.Remove(entity);
        TrySetCommandStatus(CommandStatus.Removed);
        return result;
    }

    protected virtual async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entityToAdd, CancellationToken cancellationToken = default)
    {
        var result = await Entities.AddAsync(entityToAdd, cancellationToken);

        TrySetCommandStatus(CommandStatus.Added);

        return result;
    }

    protected virtual EntityEntry<TEntity> Update(TEntity entityToUpdate)
    {
        var result = Entities.Update(entityToUpdate);
        TrySetCommandStatus(CommandStatus.Modified);
        return result;
    }
}

