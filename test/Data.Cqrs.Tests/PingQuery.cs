namespace Olbrasoft.Data.Cqrs;
public class PingQuery : BaseQuery<string>
{
    public PingQuery(IQueryProcessor processor) : base(processor)
    {
    }

    public PingQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
    public PingQuery()
    {

    }
}
