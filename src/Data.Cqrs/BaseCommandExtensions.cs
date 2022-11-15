namespace Olbrasoft.Data.Cqrs;

public static class BaseCommandExtensions
{
    public static Task<TResult> ToResultAsync<TResult>(this BaseCommand<TResult> command, CancellationToken token = default)
    {
        if (command is null) throw new CommandNullException();

        if (command.Executor is not null) return command.Executor.ExecuteAsync(command, token);

        if (command.Dispatcher is not null) return command.Dispatcher.DispatchAsync(command, token);

        throw new ToResultException(nameof(command.Executor));
    }
}