namespace Data.Cqrs.Common.Tests;
public class PingQuery : BaseQuery<string>
{
    public PingQuery(IQueryProcessor processor) : base(processor)
    {
    }

    public PingQuery(IMediator mediator) : base(mediator)
    {
    }
    public PingQuery()
    {

    }
}
