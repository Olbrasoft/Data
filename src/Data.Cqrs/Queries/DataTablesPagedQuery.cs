using Olbrasoft.Data.Sorting;

namespace Olbrasoft.Data.Cqrs.Queries;

public class DataTablesPagedQuery<TResult> : PagedQuery<TResult>
{
    public OrderDirection OrderByDirection { get; set; }
    public string Search { get; set; }
    public string OrderByColumnName { get; set; }

    public DataTablesPagedQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public DataTablesPagedQuery(IRequestHandler<Request<TResult>, TResult> handler) : base(handler)
    {
    }
}