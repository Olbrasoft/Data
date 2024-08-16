namespace Data.Cqrs.Common.Tests;

internal class Ping : BaseRequest<string>
{
    public Ping()
    {
    }

    public Ping(IMediator mediator) : base(mediator)
    {
    }
}