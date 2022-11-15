namespace Olbrasoft.Data.Cqrs;

public class ExecuteEventArgs : EventArgs
{
    public ExecuteEventArgs(object command, object? result = null )
    {
        Command = command;
        Result = result;
    }

    public object Command { get; }
    public object? Result { get; }
}