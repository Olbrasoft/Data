namespace Olbrasoft.Data.Cqrs;

public class ProcessEventArgs : EventArgs
{
    public ProcessEventArgs(object query, object? result = null)
    {
        Query = query;
        Result = result;
    }

    public object Query { get; }
    public object? Result { get; }
}