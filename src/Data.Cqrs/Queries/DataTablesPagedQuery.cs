using Olbrasoft.Dispatching;

namespace Olbrasoft.Data.Cqrs.Queries
{
    public class DataTablesPagedQuery<TResult> : PagedQuery<TResult>
    {
        public OrderDirection OrderByDirection { get; set; }
        public string Search { get; set; } = string.Empty;

        public string OrderByColumnName { get; set; } = string.Empty;

        public DataTablesPagedQuery(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        public DataTablesPagedQuery(IRequestHandler<Request<TResult>, TResult> handler) : base(handler)
        {
        }
    }
}