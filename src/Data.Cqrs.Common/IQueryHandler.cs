namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// Represents a handler for query requests.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResult">The type of the query result.</typeparam>
public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{

}
