namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// The query processor processes the query.
/// </summary>
public interface IQueryProcessor
{
    /// <summary>
    /// Evaluate query returns task of QueryResult.
    /// </summary>
    /// <typeparam name="TResult">The exact query result type</typeparam>
    /// <param name="query">The query to process.</param>
    /// <param name="token">Token to handle any cancellation of the operation.</param>
    /// <returns>Task of result</returns>
    Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken token);
}
