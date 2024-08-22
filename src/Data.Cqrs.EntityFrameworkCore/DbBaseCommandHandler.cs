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

    protected virtual Task<int> SaveAsync(TEntity entity , CancellationToken token)
    {
        return base.UpdateAsync(entity, token);
    }

    protected virtual Task<int> DeleteAsync(TEntity entity, CancellationToken token)
    {


        Context.Remove(entity);
        return Context.SaveChangesAsync(token);
    }
}

