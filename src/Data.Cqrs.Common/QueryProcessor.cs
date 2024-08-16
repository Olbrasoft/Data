namespace Olbrasoft.Data.Cqrs;

public class QueryProcessor(IMediator mediator) : IQueryProcessor
{
    private readonly IMediator _mediator = mediator is not null ? mediator : throw new ArgumentNullException(nameof(mediator));

    public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken token = default)
        => query is null ? throw new ArgumentNullException(nameof(query)) : _mediator.MediateAsync(query, token);
}