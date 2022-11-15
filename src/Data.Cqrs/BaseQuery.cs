namespace Olbrasoft.Data.Cqrs;

public class BaseQuery<TResult> : BaseRequest<TResult>
{
    public IQueryProcessor? Processor { get; }

    public BaseQuery(IQueryProcessor processor)
    {
        if (processor is null) throw new QueryProcessorNullException();

        Processor = processor;
    }

    public BaseQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    protected BaseQuery()
    {
    }
}