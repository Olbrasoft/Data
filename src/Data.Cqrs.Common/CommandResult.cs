namespace Olbrasoft.Data.Cqrs;

/// <summary>
/// Represents the result of a command execution with status information.
/// Designed for use with immutable record-based commands.
/// </summary>
/// <typeparam name="T">The type of the result data.</typeparam>
/// <remarks>
/// This pattern enables immutable commands (C# records) while maintaining
/// status tracking capabilities. Status is part of the result, not the command itself.
///
/// Example usage:
/// <code>
/// public record CreateUserCommand(string Name, string Email) : ICommand&lt;CommandResult&lt;int&gt;&gt;;
///
/// public class CreateUserHandler : ICommandHandler&lt;CreateUserCommand, CommandResult&lt;int&gt;&gt;
/// {
///     public async Task&lt;CommandResult&lt;int&gt;&gt; HandleAsync(CreateUserCommand command, CancellationToken ct)
///     {
///         try
///         {
///             var userId = await CreateUserAsync(command.Name, command.Email);
///             return CommandResult&lt;int&gt;.Success(userId);
///         }
///         catch (ValidationException ex)
///         {
///             return CommandResult&lt;int&gt;.Failure(ex.Message, CommandStatus.Conflict);
///         }
///     }
/// }
/// </code>
/// </remarks>
public record CommandResult<T>
{
    /// <summary>
    /// Gets the result data. Null if the command failed.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Gets a value indicating whether the command succeeded.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Gets the status of the command execution.
    /// </summary>
    public CommandStatus Status { get; init; }

    /// <summary>
    /// Gets the error message if the command failed.
    /// </summary>
    public string ErrorMessage { get; init; }

    /// <summary>
    /// Gets the timestamp when the command was processed.
    /// </summary>
    public DateTime ProcessedAt { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandResult{T}"/> record.
    /// </summary>
    /// <param name="data">The result data.</param>
    /// <param name="isSuccess">Whether the command succeeded.</param>
    /// <param name="status">The command status.</param>
    /// <param name="errorMessage">The error message if failed.</param>
    /// <param name="processedAt">The timestamp when processed.</param>
    public CommandResult(
        T? data,
        bool isSuccess,
        CommandStatus status,
        string errorMessage = "",
        DateTime processedAt = default)
    {
        Data = data;
        IsSuccess = isSuccess;
        Status = status;
        ErrorMessage = errorMessage ?? string.Empty;
        ProcessedAt = processedAt == default ? DateTime.UtcNow : processedAt;
    }

    /// <summary>
    /// Creates a successful command result.
    /// </summary>
    /// <param name="data">The result data.</param>
    /// <param name="status">The success status (defaults to Success).</param>
    /// <returns>A successful command result.</returns>
    public static CommandResult<T> Success(T data, CommandStatus status = CommandStatus.Success)
    {
        return new CommandResult<T>(
            data: data,
            isSuccess: true,
            status: status,
            errorMessage: string.Empty,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a successful command result for creation operations.
    /// </summary>
    /// <param name="data">The created entity data.</param>
    /// <returns>A successful command result with Created status.</returns>
    public static CommandResult<T> Created(T data)
    {
        return new CommandResult<T>(
            data: data,
            isSuccess: true,
            status: CommandStatus.Created,
            errorMessage: string.Empty,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a successful command result for deletion operations.
    /// </summary>
    /// <param name="data">Optional data about the deleted entity.</param>
    /// <returns>A successful command result with Deleted status.</returns>
    public static CommandResult<T> Deleted(T? data = default)
    {
        return new CommandResult<T>(
            data: data,
            isSuccess: true,
            status: CommandStatus.Deleted,
            errorMessage: string.Empty,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a successful command result for update operations.
    /// </summary>
    /// <param name="data">The updated entity data.</param>
    /// <returns>A successful command result with Modified status.</returns>
    public static CommandResult<T> Modified(T data)
    {
        return new CommandResult<T>(
            data: data,
            isSuccess: true,
            status: CommandStatus.Modified,
            errorMessage: string.Empty,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a command result indicating no changes were made.
    /// </summary>
    /// <param name="data">Optional data.</param>
    /// <returns>A successful command result with Unchanged status.</returns>
    public static CommandResult<T> Unchanged(T? data = default)
    {
        return new CommandResult<T>(
            data: data,
            isSuccess: true,
            status: CommandStatus.Unchanged,
            errorMessage: string.Empty,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a failed command result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="status">The error status (defaults to Error).</param>
    /// <returns>A failed command result.</returns>
    public static CommandResult<T> Failure(string errorMessage, CommandStatus status = CommandStatus.Error)
    {
        return new CommandResult<T>(
            data: default,
            isSuccess: false,
            status: status,
            errorMessage: errorMessage,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a failed command result for not found scenarios.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed command result with NotFound status.</returns>
    public static CommandResult<T> NotFound(string errorMessage)
    {
        return new CommandResult<T>(
            data: default,
            isSuccess: false,
            status: CommandStatus.NotFound,
            errorMessage: errorMessage,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Creates a failed command result for conflict scenarios.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed command result with Conflict status.</returns>
    public static CommandResult<T> Conflict(string errorMessage)
    {
        return new CommandResult<T>(
            data: default,
            isSuccess: false,
            status: CommandStatus.Conflict,
            errorMessage: errorMessage,
            processedAt: DateTime.UtcNow
        );
    }

    /// <summary>
    /// Gets a value indicating whether the command failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Executes an action if the command succeeded.
    /// Note: Action is only called when Data is not null.
    /// For successful results with null data (e.g., Deleted, Unchanged),
    /// the action will not execute.
    /// </summary>
    /// <param name="action">The action to execute with the data.</param>
    /// <returns>This command result for chaining.</returns>
    public CommandResult<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess && Data != null)
            action(Data);

        return this;
    }

    /// <summary>
    /// Executes an action if the command failed.
    /// </summary>
    /// <param name="action">The action to execute with the error message.</param>
    /// <returns>This command result for chaining.</returns>
    public CommandResult<T> OnFailure(Action<string> action)
    {
        if (IsFailure)
            action(ErrorMessage);

        return this;
    }

    /// <summary>
    /// Maps the result data to a new type if successful.
    /// Note: For successful results with null data (e.g., Deleted, Unchanged),
    /// the mapper will not be called and the result will preserve the success
    /// state with default(TNew) as data.
    /// </summary>
    /// <typeparam name="TNew">The new result type.</typeparam>
    /// <param name="mapper">The mapping function.</param>
    /// <returns>A new command result with mapped data.</returns>
    public CommandResult<TNew> Map<TNew>(Func<T, TNew> mapper)
    {
        if (IsSuccess && Data != null)
        {
            return new CommandResult<TNew>(
                data: mapper(Data),
                isSuccess: true,
                status: Status,
                errorMessage: ErrorMessage,
                processedAt: ProcessedAt
            );
        }

        // Preserve original success/failure state even if Data is null
        return new CommandResult<TNew>(
            data: default,
            isSuccess: IsSuccess,
            status: Status,
            errorMessage: ErrorMessage,
            processedAt: ProcessedAt
        );
    }
}
