namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// Represents an interface for a query that returns a result.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
public interface IQuery<out TResult> : IRequest<TResult>
{
}