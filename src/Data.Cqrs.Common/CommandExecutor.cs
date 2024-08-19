

namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// Executes commands by sending them to the mediator.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CommandExecutor"/> class.
/// </remarks>
/// <param name="mediator">The mediator instance.</param>
public class CommandExecutor(IMediator mediator) : ICommandExecutor
{
    private readonly IMediator _mediator = mediator is not null ? mediator : throw new ArgumentNullException(nameof(mediator));

    /// <summary>
    /// Executes the specified command asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the command result.</typeparam>
    /// <param name="command">The command to execute.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The result of the command execution.</returns>
    public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken token = default)
        => command is not null ? _mediator.MediateAsync(command, token) : throw new ArgumentNullException(nameof(command));


}
