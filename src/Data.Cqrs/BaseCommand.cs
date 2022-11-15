namespace Olbrasoft.Data.Cqrs;
public class BaseCommand<TResult> : BaseRequest<TResult>
{
    private CommandStatus _status;

    public ICommandExecutor? Executor { get; }

    public event EventHandler<ChangeStatusEventArgs>? StatusChanged;

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

    public BaseCommand(ICommandExecutor executor)
    {
        if (executor is null) throw new CommandExecutorNullException();

        Executor = executor;
    }

    public BaseCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    protected BaseCommand()
    {
    }

    private void OnStatusChanged(CommandStatus oldStatus)
    {
        if (StatusChanged is not null)
            StatusChanged(this, new ChangeStatusEventArgs(oldStatus, Status));
    }

}
