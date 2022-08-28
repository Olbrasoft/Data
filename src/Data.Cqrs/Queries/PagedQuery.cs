using Olbrasoft.Data.Paging;

namespace Olbrasoft.Data.Cqrs.Queries;

public class PagedQuery<TResult> : Request<TResult>
{
    public IPageInfo Paging { get; set; } = new PageInfo();

    public PagedQuery(IRequestHandler<Request<TResult>, TResult> handler) : base(handler)
    {
    }

    public PagedQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}