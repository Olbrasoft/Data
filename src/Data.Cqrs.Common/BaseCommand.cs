namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// Represents a base class for commands in the CQRS pattern.
/// </summary>
/// <typeparam name="TResult">The type of the command result.</typeparam>
public class BaseCommand<TResult> : BaseRequest<TResult>, ICommand<TResult>
{
    private CommandStatus _status;

    public ICommandExecutor? Executor { get; }

    public event EventHandler<ChangeStatusEventArgs>? StatusChanged;

    /// <summary>
    /// Gets or sets the status of the command.
    /// </summary>
    public CommandStatus Status
    {
        get => _status;

        set
        {
            var oldStatus = _status;
            _status = value;
            OnStatusChanged(oldStatus);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseCommand{TResult}"/> class with a command executor.
    /// </summary>
    /// <param name="executor">The command executor.</param>
    public BaseCommand(ICommandExecutor executor)
    {
        if (executor is not null) Executor = executor;
        else
            throw new ArgumentNullException(nameof(executor));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseCommand{TResult}"/> class with a mediator.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public BaseCommand(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseCommand{TResult}"/> class.
    /// </summary>
    protected BaseCommand()
    {
    }

    /// <summary>
    /// Handles the status change of the command.
    /// </summary>
    /// <param name="oldStatus">The old status of the command.</param>
    private void OnStatusChanged(CommandStatus oldStatus)
    {
        if (StatusChanged is not null)
            StatusChanged(this, new ChangeStatusEventArgs(oldStatus, Status));
    }
}
