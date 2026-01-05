namespace Olbrasoft.Data.Cqrs.Tests;

/// <summary>
/// Tests for CommandResult&lt;T&gt; record demonstrating immutable command pattern.
/// </summary>
public class CommandResultTests
{
    #region Factory Methods Tests

    [Fact]
    public void Success_CreatesSuccessfulResult()
    {
        // Arrange
        var data = 42;

        // Act
        var result = CommandResult<int>.Success(data);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Data.Should().Be(42);
        result.Status.Should().Be(CommandStatus.Success);
        result.ErrorMessage.Should().BeEmpty();
        result.ProcessedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Success_WithCustomStatus_UsesProvidedStatus()
    {
        // Arrange & Act
        var result = CommandResult<int>.Success(42, CommandStatus.Accepted);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Accepted);
    }

    [Fact]
    public void Created_CreatesResultWithCreatedStatus()
    {
        // Arrange & Act
        var result = CommandResult<int>.Created(100);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(100);
        result.Status.Should().Be(CommandStatus.Created);
    }

    [Fact]
    public void Deleted_CreatesResultWithDeletedStatus()
    {
        // Arrange & Act
        var result = CommandResult<int>.Deleted(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(1);
        result.Status.Should().Be(CommandStatus.Deleted);
    }

    [Fact]
    public void Deleted_WithoutData_CreatesResultWithNullData()
    {
        // Arrange & Act
        var result = CommandResult<int?>.Deleted();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Status.Should().Be(CommandStatus.Deleted);
    }

    [Fact]
    public void Modified_CreatesResultWithModifiedStatus()
    {
        // Arrange & Act
        var result = CommandResult<string>.Modified("updated");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("updated");
        result.Status.Should().Be(CommandStatus.Modified);
    }

    [Fact]
    public void Unchanged_CreatesResultWithUnchangedStatus()
    {
        // Arrange & Act
        var result = CommandResult<int?>.Unchanged();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Status.Should().Be(CommandStatus.Unchanged);
    }

    [Fact]
    public void Failure_CreatesFailedResult()
    {
        // Arrange
        var errorMessage = "Something went wrong";

        // Act
        var result = CommandResult<int>.Failure(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().Be(0); // Default value
        result.Status.Should().Be(CommandStatus.Error);
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void Failure_WithCustomStatus_UsesProvidedStatus()
    {
        // Arrange & Act
        var result = CommandResult<int>.Failure("Validation failed", CommandStatus.Conflict);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Conflict);
        result.ErrorMessage.Should().Be("Validation failed");
    }

    [Fact]
    public void NotFound_CreatesResultWithNotFoundStatus()
    {
        // Arrange & Act
        var result = CommandResult<int>.NotFound("Entity not found");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.NotFound);
        result.ErrorMessage.Should().Be("Entity not found");
    }

    [Fact]
    public void Conflict_CreatesResultWithConflictStatus()
    {
        // Arrange & Act
        var result = CommandResult<int>.Conflict("Duplicate entry");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Conflict);
        result.ErrorMessage.Should().Be("Duplicate entry");
    }

    #endregion

    #region Functional Methods Tests

    [Fact]
    public void OnSuccess_ExecutesActionWhenSuccessful()
    {
        // Arrange
        var executed = false;
        var result = CommandResult<int>.Success(42);

        // Act
        result.OnSuccess(data =>
        {
            executed = true;
            data.Should().Be(42);
        });

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_DoesNotExecuteActionWhenFailed()
    {
        // Arrange
        var executed = false;
        var result = CommandResult<int>.Failure("Error");

        // Act
        result.OnSuccess(_ => executed = true);

        // Assert
        executed.Should().BeFalse();
    }

    [Fact]
    public void OnSuccess_ReturnsResultForChaining()
    {
        // Arrange
        var result = CommandResult<int>.Success(42);

        // Act
        var chainedResult = result.OnSuccess(_ => { });

        // Assert
        chainedResult.Should().BeSameAs(result);
    }

    [Fact]
    public void OnFailure_ExecutesActionWhenFailed()
    {
        // Arrange
        var executed = false;
        var result = CommandResult<int>.Failure("Error message");

        // Act
        result.OnFailure(error =>
        {
            executed = true;
            error.Should().Be("Error message");
        });

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_DoesNotExecuteActionWhenSuccessful()
    {
        // Arrange
        var executed = false;
        var result = CommandResult<int>.Success(42);

        // Act
        result.OnFailure(_ => executed = true);

        // Assert
        executed.Should().BeFalse();
    }

    [Fact]
    public void OnFailure_ReturnsResultForChaining()
    {
        // Arrange
        var result = CommandResult<int>.Failure("Error");

        // Act
        var chainedResult = result.OnFailure(_ => { });

        // Assert
        chainedResult.Should().BeSameAs(result);
    }

    [Fact]
    public void OnSuccessAndOnFailure_CanBeChained()
    {
        // Arrange
        var successExecuted = false;
        var failureExecuted = false;
        var result = CommandResult<int>.Success(42);

        // Act
        result
            .OnSuccess(_ => successExecuted = true)
            .OnFailure(_ => failureExecuted = true);

        // Assert
        successExecuted.Should().BeTrue();
        failureExecuted.Should().BeFalse();
    }

    [Fact]
    public void Map_TransformsDataWhenSuccessful()
    {
        // Arrange
        var result = CommandResult<int>.Success(42);

        // Act
        var mappedResult = result.Map(x => x.ToString());

        // Assert
        mappedResult.IsSuccess.Should().BeTrue();
        mappedResult.Data.Should().Be("42");
        mappedResult.Status.Should().Be(CommandStatus.Success);
    }

    [Fact]
    public void Map_PreservesFailureState()
    {
        // Arrange
        var result = CommandResult<int>.Failure("Error", CommandStatus.NotFound);

        // Act
        var mappedResult = result.Map(x => x.ToString());

        // Assert
        mappedResult.IsFailure.Should().BeTrue();
        mappedResult.Data.Should().BeNull();
        mappedResult.ErrorMessage.Should().Be("Error");
        mappedResult.Status.Should().Be(CommandStatus.NotFound);
    }

    [Fact]
    public void Map_PreservesProcessedAtTimestamp()
    {
        // Arrange
        var result = CommandResult<int>.Success(42);
        var originalTime = result.ProcessedAt;

        // Act
        var mappedResult = result.Map(x => x.ToString());

        // Assert
        mappedResult.ProcessedAt.Should().Be(originalTime);
    }

    #endregion

    #region Record Behavior Tests

    [Fact]
    public void CommandResult_IsRecord()
    {
        // Arrange
        var result1 = CommandResult<int>.Success(42);
        var result2 = CommandResult<int>.Success(42);

        // Act & Assert - Records have value-based equality
        result1.Should().NotBeSameAs(result2); // Different instances
        // Note: Equality won't work due to ProcessedAt being different
    }

    [Fact]
    public void CommandResult_WithExpression_CreatesModifiedCopy()
    {
        // Arrange
        var original = CommandResult<int>.Success(42);

        // Act
        var modified = original with { Data = 100 };

        // Assert
        original.Data.Should().Be(42);
        modified.Data.Should().Be(100);
        modified.Status.Should().Be(original.Status);
        modified.IsSuccess.Should().Be(original.IsSuccess);
    }

    [Fact]
    public void CommandResult_IsImmutable()
    {
        // Arrange
        var result = CommandResult<int>.Success(42);

        // Act & Assert - Properties are init-only
        // result.Data = 100; // Won't compile!
        // result.IsSuccess = false; // Won't compile!
        // result.Status = CommandStatus.Error; // Won't compile!

        result.Data.Should().Be(42);
    }

    #endregion

    #region Integration with Commands

    public record TestCommand(int Value) : ICommand<CommandResult<string>>;

    public class TestCommandHandler : ICommandHandler<TestCommand, CommandResult<string>>
    {
        public Task<CommandResult<string>> HandleAsync(TestCommand request, CancellationToken cancellationToken)
        {
            if (request.Value < 0)
                return Task.FromResult(CommandResult<string>.Failure("Value cannot be negative"));

            if (request.Value == 0)
                return Task.FromResult(CommandResult<string>.NotFound("Value is zero"));

            var result = $"Value is {request.Value}";
            return Task.FromResult(CommandResult<string>.Success(result));
        }
    }

    [Fact]
    public async Task ImmutableCommand_WithCommandResult_HandlesSuccess()
    {
        // Arrange
        var command = new TestCommand(42);
        var handler = new TestCommandHandler();

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("Value is 42");
        result.Status.Should().Be(CommandStatus.Success);
    }

    [Fact]
    public async Task ImmutableCommand_WithCommandResult_HandlesValidationFailure()
    {
        // Arrange
        var command = new TestCommand(-1);
        var handler = new TestCommandHandler();

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Value cannot be negative");
        result.Status.Should().Be(CommandStatus.Error);
    }

    [Fact]
    public async Task ImmutableCommand_WithCommandResult_HandlesNotFound()
    {
        // Arrange
        var command = new TestCommand(0);
        var handler = new TestCommandHandler();

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Value is zero");
        result.Status.Should().Be(CommandStatus.NotFound);
    }

    [Fact]
    public void ImmutableCommand_IsFullyImmutable()
    {
        // Arrange
        var command = new TestCommand(42);

        // Act - Use 'with' to create modified copy
        var modifiedCommand = command with { Value = 100 };

        // Assert
        command.Value.Should().Be(42); // Original unchanged
        modifiedCommand.Value.Should().Be(100); // New instance modified
    }

    #endregion
}
