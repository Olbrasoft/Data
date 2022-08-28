namespace Olbrasoft.Data.Cqrs.Requests;

public class ByIdRequest<TResponse> : Request<TResponse>
{
    public ByIdRequest(IRequestHandler<Request<TResponse>, TResponse> handler) : base(handler)
    {
    }

    public ByIdRequest(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }
}

public class ByIdRequest : ByIdRequest<bool>
{
    public ByIdRequest(IRequestHandler<Request<bool>, bool> handler) : base(handler)
    {
    }

    public ByIdRequest(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}