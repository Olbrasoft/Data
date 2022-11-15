namespace Olbrasoft.Data.Cqrs;
public class ChangeStatusEventArgs : EventArgs
{
    public CommandStatus OldStatus { get; set; }
    public CommandStatus NewStatus { get; set; }

    public ChangeStatusEventArgs(CommandStatus oldStatus, CommandStatus newStatus)
    {
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}
