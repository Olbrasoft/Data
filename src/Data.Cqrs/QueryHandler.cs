using System.Linq;

namespace Olbrasoft.Data.Cqrs;

public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IRequest<TResult>
{
    private readonly IProjector _projector;

    protected QueryHandler(IProjector projector)
    {
        _projector = projector ?? throw new ArgumentNullException(nameof(projector));
    }

    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken token = default);

    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
    {
        return source is null
            ? throw new ArgumentNullException(nameof(source))
            : _projector.ProjectTo<TDestination>(source);
    }
}

public abstract class QueryHandler<TQuery> : IRequestHandler<TQuery, bool> where TQuery : IRequest<bool>
{
    public abstract Task<bool> HandleAsync(TQuery query, CancellationToken token = default);
}