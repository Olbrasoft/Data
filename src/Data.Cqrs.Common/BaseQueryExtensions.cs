namespace Olbrasoft.Data.Cqrs;

public static class BaseQueryExtensions
{
    public static Task<TResult> ToResultAsync<TResult>(this BaseQuery<TResult> query, CancellationToken token = default) => query is null
            ? throw new ArgumentNullException(nameof(query))
            : query.Processor is not null
                ? query.Processor.ProcessAsync(query, token)
                : query.Mediator is not null
                    ? query.Mediator.MediateAsync(query, token)
                    : throw new InvalidOperationException($"{nameof(query.Mediator)} and {nameof(query.Processor)} is null.");
}