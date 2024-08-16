namespace Olbrasoft.Data.Cqrs;


/// <summary>
/// Represents a command that returns a result.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
public interface ICommand<out TResult> : IRequest<TResult>
{
}
