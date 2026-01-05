# Research Findings: Immutable Command Pattern for C# Records

**Issue:** #10 - Consider immutable command pattern alternative to BaseCommand<T>
**Date:** 2026-01-05
**Status:** Research Phase

## Executive Summary

This document presents research findings on immutable command patterns compatible with C# records as an alternative to the current `BaseCommand<T>` implementation in Data.Cqrs.Common. The research evaluates four distinct approaches, analyzes how popular CQRS libraries handle this challenge, and provides recommendations based on industry best practices.

**Key Finding:** Most modern CQRS implementations (MediatR, Azure patterns) favor immutable commands without built-in status tracking, relying instead on:
- Result-based status reporting
- Event sourcing for audit trails
- External status tracking services
- No mutable state in command objects

## Current State Analysis

### BaseCommand<T> Implementation

The current `BaseCommand<T>` has inherent mutability:

```csharp
public class BaseCommand<TResult> : BaseRequest<TResult>, ICommand<TResult>
{
    private CommandStatus _status;
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

    // Three constructors: ICommandExecutor, IMediator, parameterless
}
```

**Mutable State:**
- `Status` property (get/set)
- `StatusChanged` event
- Private `_status` field

**Compatibility Issues:**
- ❌ Cannot be used with C# records (records are immutable)
- ❌ Cannot inherit from abstract class with mutable state
- ❌ Requires parameterless constructor (not record-friendly)

### CommandStatus Enum

Uses HTTP status code-based values:

```csharp
public enum CommandStatus
{
    Default = 0,
    Success = 200,
    Created = 201,
    Accepted = 202,
    Deleted = 204,
    Modified = 302,
    Unchanged = 304,
    NotFound = 404,
    Conflict = 409,
    Removed = 410,
    Added = 206,
    Error = 500
}
```

**Observation:** HTTP-style status codes suggest REST API integration, which could be better handled at the API layer rather than domain layer.

## Research Findings

### 1. MediatR Approach

**Library:** [MediatR](https://github.com/jbogard/MediatR) (Most popular .NET mediator library, 11k+ GitHub stars)

**Key Findings:**
- ✅ **Fully supports C# records**
- ✅ **No built-in status tracking**
- ✅ **Immutable commands by design**
- ✅ **Uses `IRequest<TResponse>` interface**

**Example:**
```csharp
// MediatR command (immutable record)
public record CreateUserCommand(string Name, string Email) : IRequest<int>;

// Handler returns result directly
public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    public async Task<int> HandleAsync(CreateUserCommand request, CancellationToken ct)
    {
        // Implementation
        return userId;
    }
}
```

**Status Handling:**
- Status is part of the result, not the command
- No events or status tracking in command objects
- Clean separation of concerns

**Community Adoption:**
- Widely used in production applications
- Recommended by Microsoft in eShopOnContainers reference architecture
- Works seamlessly with records since C# 9

### 2. Azure CQRS Pattern

**Source:** [Microsoft Azure Architecture Center - CQRS Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)

**Key Findings:**
- Commands are **data transfer objects** (DTOs)
- Commands should be **immutable**
- Status tracking handled via:
  - Event sourcing (chronological event log)
  - Separate audit/tracking services
  - Result objects with status information

**Pattern Recommendation:**
```csharp
// Command (immutable)
public record UpdateProductPriceCommand(int ProductId, decimal NewPrice);

// Result with status
public record CommandResult<T>(
    T Value,
    bool Success,
    string ErrorMessage = "",
    DateTime ProcessedAt = default
);
```

**Event Sourcing Integration:**
- Commands generate events
- Events form audit trail
- State reconstructed from events
- Natural fit with immutability

### 3. Event Sourcing Libraries

**Analyzed Libraries:**
- [EventSourcing.NetCore](https://github.com/oskardudycz/EventSourcing.NetCore) (3.6k+ stars)
- [Equinox](https://github.com/jet/equinox)
- [EventFlow](https://github.com/eventflow/EventFlow)

**Common Patterns:**
1. **Commands are pure DTOs** - No business logic, no state
2. **Events represent state changes** - Immutable records
3. **Aggregates handle commands** - Return events or errors
4. **Read models built from events** - Separate from write model

**Example:**
```csharp
// Command - immutable record
public record ProcessOrderCommand(Guid OrderId, List<OrderItem> Items);

// Events - immutable records
public record OrderCreatedEvent(Guid OrderId, DateTime CreatedAt);
public record OrderItemAddedEvent(Guid OrderId, OrderItem Item);
public record OrderProcessedEvent(Guid OrderId, decimal TotalAmount);

// Aggregate handles command, returns events
public class Order
{
    public IEnumerable<object> Handle(ProcessOrderCommand command)
    {
        yield return new OrderCreatedEvent(command.OrderId, DateTime.UtcNow);

        foreach(var item in command.Items)
            yield return new OrderItemAddedEvent(command.OrderId, item);

        yield return new OrderProcessedEvent(command.OrderId, CalculateTotal());
    }
}
```

### 4. Industry Best Practices

**From Martin Fowler's CQRS Article:**
- Commands should be **task-based** (imperative names: "Ship Order", not "Set Status")
- Commands can **fail** - use result objects to communicate success/failure
- **Eventual consistency** - status may not be immediately available
- **Event Collaboration** - services communicate via events, not shared state

**From Domain-Driven Design:**
- Commands are **messages**, not objects with behavior
- **Immutability** reduces bugs and makes code easier to reason about
- **Value objects** (like records) for commands and events

## Option Analysis

### Option 1: Accept the Trade-off (Current Approach)

**Approach:**
- Keep `BaseCommand<T>` for stateful commands
- Support `ICommand<T>` with records for simple commands
- Document when to use each

**Pros:**
- ✅ No breaking changes
- ✅ Simple to implement
- ✅ Backward compatible
- ✅ Both patterns available

**Cons:**
- ❌ Two patterns to maintain and document
- ❌ Confusing for developers (which to use?)
- ❌ Encourages mutable state in domain layer
- ❌ Goes against modern CQRS best practices

**Verdict:** **Not Recommended** - Leads to inconsistency and confusion

### Option 2: Immutable Status Pattern (External Tracking)

**Approach:**
- Commands stay immutable (records)
- Status tracked externally via `ICommandContext` service
- Handler updates status through context

**Example:**
```csharp
// Command - immutable record
public record ProcessOrderCommand(int OrderId) : ICommand<OrderResult>;

// External status tracking
public interface ICommandContext
{
    Task SetStatusAsync<TCommand>(TCommand command, CommandStatus status);
    Task<CommandStatus> GetStatusAsync<TCommand>(TCommand command);
}

// Handler uses context
public class ProcessOrderHandler : ICommandHandler<ProcessOrderCommand, OrderResult>
{
    private readonly ICommandContext _context;

    public async Task<OrderResult> HandleAsync(ProcessOrderCommand command, CancellationToken ct)
    {
        await _context.SetStatusAsync(command, CommandStatus.Processing);

        // Process order...

        await _context.SetStatusAsync(command, CommandStatus.Completed);
        return result;
    }
}
```

**Pros:**
- ✅ Commands remain immutable (records work)
- ✅ Status tracking still available when needed
- ✅ Centralized status management
- ✅ Can add telemetry/logging easily

**Cons:**
- ❌ Additional infrastructure (ICommandContext service)
- ❌ More complex than simple result-based approach
- ❌ Requires command identity (how to track specific instance?)
- ❌ State stored outside domain model

**Verdict:** **Considered but not recommended** - Too complex for questionable benefit

### Option 3: Result-Based Status (Recommended)

**Approach:**
- Commands are immutable (records)
- Status returned as part of result
- No mutable state anywhere

**Example:**
```csharp
// Command - immutable record
public record ProcessOrderCommand(int OrderId) : ICommand<CommandResult<OrderSummary>>;

// Result with status
public record CommandResult<T>(
    T? Data,
    bool Success,
    CommandStatus Status,
    string ErrorMessage = "",
    DateTime ProcessedAt = default
)
{
    public static CommandResult<T> Succeeded(T data, CommandStatus status = CommandStatus.Success)
        => new(data, true, status, "", DateTime.UtcNow);

    public static CommandResult<T> Failed(string error, CommandStatus status = CommandStatus.Error)
        => new(default, false, status, error, DateTime.UtcNow);
}

// Handler returns result with status
public class ProcessOrderHandler : ICommandHandler<ProcessOrderCommand, CommandResult<OrderSummary>>
{
    public async Task<CommandResult<OrderSummary>> HandleAsync(ProcessOrderCommand command, CancellationToken ct)
    {
        try
        {
            var summary = await ProcessOrderAsync(command.OrderId, ct);
            return CommandResult<OrderSummary>.Succeeded(summary, CommandStatus.Success);
        }
        catch (NotFoundException)
        {
            return CommandResult<OrderSummary>.Failed("Order not found", CommandStatus.NotFound);
        }
        catch (Exception ex)
        {
            return CommandResult<OrderSummary>.Failed(ex.Message, CommandStatus.Error);
        }
    }
}
```

**Pros:**
- ✅ Commands fully immutable (records work perfectly)
- ✅ Status part of result (clear, explicit)
- ✅ Matches industry best practices (MediatR, Azure patterns)
- ✅ No additional infrastructure needed
- ✅ Easy to test
- ✅ Clean separation of concerns
- ✅ Works with existing `ICommand<TResult>` interface

**Cons:**
- ⚠️ Breaking change for existing code using `BaseCommand<T>`
- ⚠️ No `StatusChanged` event (but can add via behaviors/pipelines)
- ⚠️ Requires wrapping all results

**Verdict:** **RECOMMENDED** - Best balance of simplicity and functionality

### Option 4: Event-Sourced Commands

**Approach:**
- Commands remain immutable (records)
- Emit events during processing
- Events form audit trail and status history

**Example:**
```csharp
// Command - immutable record
public record ProcessOrderCommand(int OrderId) : ICommand<OrderResult>;

// Events - immutable records
public record CommandStartedEvent(Guid CommandId, Type CommandType, DateTime StartedAt);
public record CommandCompletedEvent(Guid CommandId, DateTime CompletedAt);
public record CommandFailedEvent(Guid CommandId, Exception Error, DateTime FailedAt);

// Handler emits events
public class ProcessOrderHandler : ICommandHandler<ProcessOrderCommand, OrderResult>
{
    private readonly IEventPublisher _events;

    public async Task<OrderResult> HandleAsync(ProcessOrderCommand command, CancellationToken ct)
    {
        var commandId = Guid.NewGuid();

        await _events.PublishAsync(new CommandStartedEvent(commandId, typeof(ProcessOrderCommand), DateTime.UtcNow));

        try
        {
            var result = await ProcessOrderAsync(command.OrderId, ct);
            await _events.PublishAsync(new CommandCompletedEvent(commandId, DateTime.UtcNow));
            return result;
        }
        catch (Exception ex)
        {
            await _events.PublishAsync(new CommandFailedEvent(commandId, ex, DateTime.UtcNow));
            throw;
        }
    }
}
```

**Pros:**
- ✅ Commands immutable (records work)
- ✅ Complete audit trail
- ✅ Can reconstruct status history
- ✅ Decoupled event-driven architecture
- ✅ Scales well for complex scenarios

**Cons:**
- ❌ Requires event infrastructure (publish/subscribe)
- ❌ More complex to implement
- ❌ Eventual consistency challenges
- ❌ Overkill for simple CRUD operations

**Verdict:** **Good for complex domains** - But adds significant complexity

## Performance Considerations

### Memory Allocation

**Records:**
- Shallow copy on `with` expression
- Minimal overhead vs classes
- Value-based equality slightly slower than reference equality

**Wrapping Results:**
- Additional object allocation per command
- Negligible impact in most scenarios
- Can be optimized with pooling if needed

### Benchmark Comparison (Estimated)

| Approach | Allocation | Speed | Complexity |
|----------|------------|-------|------------|
| BaseCommand (mutable) | Low | Fastest | Low |
| Result-Based (immutable) | Medium | Fast | Low |
| External Context | Medium-High | Medium | High |
| Event-Sourced | High | Slower | Very High |

**Conclusion:** Result-based approach has acceptable performance for 99% of use cases.

## Migration Strategy

### Phase 1: Introduce CommandResult<T>

1. Add `CommandResult<T>` record
2. Keep `BaseCommand<T>` for backward compatibility
3. Document new pattern in README

### Phase 2: Provide Examples

1. Add record-based command examples (Issue #9 ✅ completed)
2. Show result-based status pattern
3. Migration guide for existing code

### Phase 3: Deprecate BaseCommand<T> (Optional)

1. Mark `BaseCommand<T>` as `[Obsolete]` with guidance
2. Provide automated migration tool
3. Remove in next major version

### Breaking Change Impact

**Low Risk:**
- New projects can use records immediately
- Existing projects can migrate gradually
- Both patterns can coexist

## Recommendations

### Primary Recommendation: Option 3 (Result-Based Status)

**Implement:**
1. Add `CommandResult<T>` record to Data.Cqrs.Common
2. Update documentation to recommend records for new code
3. Keep `BaseCommand<T>` for backward compatibility (mark as legacy)
4. Provide migration examples

**Rationale:**
- ✅ Matches industry best practices (MediatR, Azure patterns)
- ✅ Works perfectly with C# records
- ✅ Simple to understand and implement
- ✅ No additional infrastructure needed
- ✅ Easy migration path

### Secondary Recommendation: Event-Sourced Commands for Complex Domains

For applications with complex audit requirements:
- Use Option 4 (Event-Sourced Commands)
- Integrate with event store (EventStore, Marten, etc.)
- Build read models from events

**When to use:**
- Financial systems
- Healthcare applications
- Systems requiring full audit trails
- High-compliance environments

### Not Recommended

- ❌ Option 1 (Accept Trade-off) - Creates confusion
- ❌ Option 2 (External Context) - Too complex without clear benefit

## Next Steps

1. **Prototype CommandResult<T>** - Create implementation in feature branch
2. **Add Tests** - Demonstrate usage with records
3. **Update Documentation** - README, migration guide
4. **Community Feedback** - Create discussion in GitHub
5. **Performance Benchmarks** - Validate assumptions
6. **Implementation** - Merge to main branch

## References

1. [MediatR GitHub Repository](https://github.com/jbogard/MediatR)
2. [Microsoft Azure - CQRS Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
3. [Microsoft Azure - Event Sourcing Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/event-sourcing)
4. [Martin Fowler - CQRS](https://martinfowler.com/bliki/CQRS.html)
5. [EventSourcing.NetCore](https://github.com/oskardudycz/EventSourcing.NetCore)
6. [Understanding CQRS with MediatR .NET](https://nishanc.medium.com/understanding-cqrs-pattern-using-net-core-mediatr-3658263cfb16)

## Appendices

### Appendix A: BaseCommand<T> Usage Analysis

**Current Usage in Data.Cqrs.Common.Tests:**
- No direct usage found in tests
- Tests use mocks of `BaseCommand<T>`
- Primarily used through `ICommand<T>` interface

**Conclusion:** Low usage in existing codebase makes migration easier

### Appendix B: Community Survey (Placeholder)

**Questions for Community:**
1. Do you currently use `BaseCommand<T>`?
2. Do you need status tracking in command objects?
3. Would you prefer result-based status?
4. Are you using C# records for commands?

**Survey Results:** TBD

---

**Document Status:** Draft
**Last Updated:** 2026-01-05
**Next Review:** After community feedback
