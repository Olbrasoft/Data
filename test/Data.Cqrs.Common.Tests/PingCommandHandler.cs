namespace Data.Cqrs.Common.Tests;
public class PingCommandHandler : ICommandHandler<PingCommand, object>
{
    public Task<object> HandleAsync(PingCommand request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
