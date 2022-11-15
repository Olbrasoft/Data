namespace Olbrasoft.Data.Cqrs;

public class CommandExecutor : ICommandExecutor
{
    private readonly IDispatcher _dispatcher;
    public event EventHandler<ExecuteEventArgs>? Executing;
    public event EventHandler<ExecuteEventArgs>? Executed;

    public CommandExecutor(IDispatcher dispatcher)
    {
        if (dispatcher is null) throw new DispatcherNullException();

        _dispatcher = dispatcher;
    }

    public async Task<TResult> ExecuteAsync<TResult>(BaseCommand<TResult> command, CancellationToken token = default)
    {
        if (command is null) throw new CommandNullException();

        OnExecuting(command);

        var result = await _dispatcher.DispatchAsync(command, token);

        OnExecuted(command, result);

        return result;
    }

    private void OnExecuting<TResult>(BaseCommand<TResult> command)
    {
        if (Executing is not null)
            Executing(this, new ExecuteEventArgs(command));
    }

    private void OnExecuted<TResult>(BaseCommand<TResult> command, TResult result)
    {
        if (Executed is not null)
            Executed(this, new ExecuteEventArgs(command, result));
    }
}