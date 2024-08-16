namespace Olbrasoft.Data.Cqrs;


/// <summary>
/// Event arguments for the status change event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ChangeStatusEventArgs"/> class.
/// </remarks>
/// <param name="oldStatus">The old status.</param>
/// <param name="newStatus">The new status.</param>
public class ChangeStatusEventArgs(CommandStatus oldStatus, CommandStatus newStatus) : EventArgs
{
    /// <summary>
    /// Gets or sets the old status.
    /// </summary>
    public CommandStatus OldStatus { get; set; } = oldStatus;

    /// <summary>
    /// Gets or sets the new status.
    /// </summary>
    public CommandStatus NewStatus { get; set; } = newStatus;
}
