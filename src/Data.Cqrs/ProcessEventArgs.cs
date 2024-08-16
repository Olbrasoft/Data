namespace Olbrasoft.Data.Cqrs;

public class ProcessEventArgs(object query, object? result = null) : EventArgs
{
    public object Query { get; } = query ?? throw new QueryNullException();
    public object? Result { get; } = result;
}