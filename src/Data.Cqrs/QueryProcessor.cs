namespace Olbrasoft.Data.Cqrs;

public class QueryProcessor : IQueryProcessor
{
    private readonly IDispatcher _dispatcher;
    public event EventHandler<ProcessEventArgs>? Processing;
    public event EventHandler<ProcessEventArgs>? Processed;

    public QueryProcessor(IDispatcher dispatcher)
    {
        if (dispatcher is null) throw new ArgumentNullException(nameof(dispatcher));

        _dispatcher = dispatcher;
    }

    public async Task<TResult> ProcessAsync<TResult>(BaseQuery<TResult> query, CancellationToken token = default) 
    {        
        if (query is null) throw new QueryNullException();

        OnProcessing(query);

        var result = await _dispatcher.DispatchAsync(query, token);

        OnProcessed(query, result);

        return result;
    }

    private void OnProcessing<TResult>(BaseQuery<TResult> query )
    {
        if(Processing is not null)
            Processing(this, new ProcessEventArgs(query));
    }

    private void OnProcessed<TResult>(BaseQuery<TResult> query, TResult result)
    {
        if(Processed is not null)
            Processed(this, new ProcessEventArgs(query, result));
    }
}