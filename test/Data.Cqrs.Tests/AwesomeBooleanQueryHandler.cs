namespace Olbrasoft.Data.Cqrs;

internal class AwesomeBooleanQueryHandler : QueryHandler<IRequest<bool>>
{
    public AwesomeBooleanQueryHandler()
    {
    }

    public override Task<bool> HandleAsync(IRequest<bool> query, CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }
}