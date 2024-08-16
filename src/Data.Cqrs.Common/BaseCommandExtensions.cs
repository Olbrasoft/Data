namespace Olbrasoft.Data.Cqrs;

public static class BaseCommandExtensions
{
    public static Task<TResult> ToResultAsync<TResult>(this BaseCommand<TResult> command, CancellationToken token = default) => command is not null
            ? command.Executor is not null
                ? command.Executor.ExecuteAsync(command, token)
                : command.Mediator is not null
                ? command.Mediator.MediateAsync(command, token)
                : throw new InvalidOperationException($"{nameof(command.Mediator)} and {nameof(command.Executor)} is null.")
            : throw new ArgumentNullException(nameof(command));
}