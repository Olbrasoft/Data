namespace Olbrasoft.Data.Cqrs.FreeSql;
public abstract class BaseDbCommandHandler<TContext, TEntity, TCommand, TResult> : DbCommandHandler<TContext, TEntity, TCommand, TResult>
     where TEntity : class where TContext : DbContext where TCommand : BaseCommand<TResult>
{

    protected virtual TCommand? Command { get; set; }


    protected BaseDbCommandHandler(TContext context) : base(context)
    {
    }

    protected BaseDbCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    protected override async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token = default)
    {
        var result = await Select.AnyAsync(condition, token);

        if (!result) TrySetCommandStatus(CommandStatus.NotFound);

        return result;
    }


    protected virtual bool TrySetCommandStatus(CommandStatus status)
    {
        if (Command is null) return false;

        Command.Status = status;

        return true;
    }


    protected virtual void UseAutoChangeCommandStatus(TCommand command)
    {
        Command = command;
    }


    public async Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        await Entities.AddAsync(entity, token);

        TrySetCommandStatus(CommandStatus.Added);
    }


    protected virtual async Task<CommandStatus> AddAndSaveAsync(TEntity detachedEntity, CancellationToken token = default)
    {
        await AddAsync(detachedEntity, token);

        if (await SaveOneEntityAsync(token))
        {
            TrySetCommandStatus(CommandStatus.Created);
            return CommandStatus.Created;
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }


    protected virtual async Task<CommandStatus> UpdateAndSaveOneAsync(TEntity unchangedEntity, CancellationToken token = default)
    {
        await Entities.UpdateAsync(unchangedEntity, token);

        TrySetCommandStatus(CommandStatus.Modified);

        return await SaveOneAsync(unchangedEntity, token);
    }


    protected virtual async Task<CommandStatus> SaveOneAsync(TEntity modifiedEntity, CancellationToken token = default)
    {
        TrySetCommandStatus(CommandStatus.Modified);

        if (await SaveOneEntityAsync(token))
        {
            TrySetCommandStatus(CommandStatus.Success);
            return CommandStatus.Success;
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }


    protected virtual async Task<CommandStatus> RemoveAndSaveOneAsync(Expression<Func<TEntity, bool>> oneEntityCondition, CancellationToken token = default)
    {
        if (await Entities.RemoveAsync(oneEntityCondition, token) == 1)
        {
            TrySetCommandStatus(CommandStatus.Removed);

            if (await Context.SaveChangesAsync(token) == 0)
            {
                TrySetCommandStatus(CommandStatus.Deleted);
                return CommandStatus.Deleted;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }


    protected virtual async Task<CommandStatus> RemoveAndSaveOneAsync(TEntity detachedOrUnchangedEntity, CancellationToken token = default)
    {

        Entities.Remove(detachedOrUnchangedEntity);

        TrySetCommandStatus(CommandStatus.Removed);

        if (await SaveOneEntityAsync(token))
        {
            TrySetCommandStatus(CommandStatus.Deleted);
            return CommandStatus.Deleted;
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

}
