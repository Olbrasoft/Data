using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs
{
    public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IRequest<TResult>
    {
        private readonly IProjector _projector;

        protected QueryHandler(IProjector projector) => _projector = projector;

        public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken token);

        protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source) => _projector.ProjectTo<TDestination>(source);
    }

    public abstract class QueryHandler<TQuery> : IRequestHandler<TQuery, bool> where TQuery : IRequest<bool>
    {
        public abstract Task<bool> HandleAsync(TQuery query, CancellationToken token);
    }
}