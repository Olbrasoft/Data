# Data - Claude Code Documentation

## Repository Overview

**Location:** `/home/jirka/Olbrasoft/Data/`
**Type:** .NET Libraries Collection (CQRS, Entity Framework Core, Paging, Sorting)
**Target Frameworks:** .NET 8.0, 9.0, 10.0
**NuGet Packages:** Multiple packages under Olbrasoft.Data.* namespace

## Purpose

Collection of .NET libraries for data access patterns, CQRS implementation, and Entity Framework Core integration. Provides abstractions and implementations for CQRS/CQS patterns with EF Core support.

## Current State Analysis

### Projects Structure

```
Data/
├── src/
│   ├── Data.Entities.Abstractions/        # Entity base types and interfaces
│   ├── Data.Cqrs.Common/                  # CQRS base classes and interfaces
│   ├── Data.Cqrs.EntityFrameworkCore/     # EF Core integration for CQRS
│   ├── Data.Cqrs.FreeSql/                 # FreeSql integration
│   ├── Data.Paging/                       # Pagination abstractions
│   ├── Data.Sorting/                      # Sorting helpers
│   ├── Data.Sorting.Extensions/           # Sorting extension methods
│   ├── Data.Paging.X.PagedList/          # PagedList implementation
│   ├── Data.Paging.X.PagedList.AspNetCore.Mvc/ # ASP.NET Core MVC helpers
│   ├── Data.Common/                       # Common utilities
│   └── Data.SqlClient/                    # SQL Client helpers
└── test/
    └── [corresponding test projects]
```

### Core CQRS Components (Data.Cqrs.Common)

#### 1. Query Interfaces and Classes

- **IQuery&lt;TResult>** - Interface for queries (inherits `IRequest<TResult>` from Mediation)
- **BaseQuery&lt;TResult>** - Abstract base class for queries
  - Inherits from `BaseRequest<TResult>` (from Mediation)
  - Optional `IQueryProcessor` dependency
  - **Limitation:** Abstract class - cannot be used with records

#### 2. Command Interfaces and Classes

- **ICommand&lt;TResult>** - Interface for commands (inherits `IRequest<TResult>` from Mediation)
- **BaseCommand&lt;TResult>** - Abstract base class for commands
  - Inherits from `BaseRequest<TResult>` (from Mediation)
  - Has `CommandStatus` property with change events
  - Optional `ICommandExecutor` dependency
  - **Limitation:** Abstract class with state - cannot be used with records

#### 3. Handler Interfaces

- **IQueryHandler&lt;TQuery, TResult>** - Query handler interface
- **ICommandHandler&lt;TCommand, TResult>** - Command handler interface

#### 4. Processors and Executors

- **QueryProcessor** - Processes queries via mediator
- **CommandExecutor** - Executes commands via mediator

### Entity Framework Core Integration (Data.Cqrs.EntityFrameworkCore)

- Provides EF Core-based query and command handlers
- DbContext integration
- Transaction support
- Mapping abstractions via `Olbrasoft.Mapping.Abstractions`
- Multi-framework support with version-specific EF Core dependencies:
  - .NET 8.0 → EF Core 8.0.8
  - .NET 9.0 → EF Core 9.0.5
  - .NET 10.0 → EF Core 10.0.0

### Current Limitations - Record Support

#### What Works

- **Records implementing interfaces directly** should work:
  - `IQuery<TResult>`
  - `ICommand<TResult>`
- Inherits same technical compatibility as Mediation library

#### What Doesn't Work

1. **BaseQuery&lt;TResult>** - Abstract class
   - Cannot be inherited by records
   - Has optional constructor parameters
   - Commonly used in examples

2. **BaseCommand&lt;TResult>** - Abstract class with state
   - Has `CommandStatus` property
   - Has event `StatusChanged`
   - Mutable state conflicts with record immutability
   - Not suitable for record pattern

#### Why Records Aren't Supported Yet

1. **Documentation Gap:**
   - README shows only class-based examples inheriting from BaseQuery/BaseCommand
   - No examples of records implementing IQuery/ICommand directly
   - All quick start examples use base classes

2. **Testing Gap:**
   - Tests use mocks of `BaseQuery<T>` and `BaseCommand<T>`
   - Example: `new Mock<BaseQuery<string>>(dis.Object)`
   - No tests verify record-based queries/commands work

3. **Design Consideration:**
   - `BaseCommand<T>` has mutable state (Status property, events)
   - Records are immutable by design
   - Commands might need different approach for records

### Technical Analysis - Record Compatibility

**Records CAN work for queries:**
```csharp
// This should work (but untested):
public record GetUserByIdQuery(int UserId) : IQuery<UserDto>;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken token)
    {
        // Implementation
    }
}
```

**Records have limitations for commands:**
```csharp
// This works but loses BaseCommand benefits (status tracking, events):
public record CreateUserCommand(string Name, string Email) : ICommand<int>;

// Alternative: Keep class for stateful commands
public class CreateUserCommand : BaseCommand<int>
{
    public string Name { get; init; }
    public string Email { get; init; }
}
```

## Required Changes for Record Support

### 1. Add Record Tests
- Test record queries with EF Core integration
- Test record commands (consider immutability implications)
- Test with QueryProcessor and CommandExecutor
- Verify handler registration works
- Test generic record queries/commands

### 2. Update Documentation
- Add record examples for queries (recommended use case)
- Document class vs record for commands (explain trade-offs)
- Show direct `IQuery<T>`/`ICommand<T>` implementation
- Update README.md with record examples
- Document when to use BaseQuery/BaseCommand vs direct interfaces

### 3. Add Code Examples
- Create sample record-based queries
- Show EF Core integration with records
- Demonstrate immutable query patterns
- Show command patterns (both record and class approaches)

### 4. Consider API Extensions
- Add extension methods for record-friendly patterns
- Consider immutable command pattern helpers
- Evaluate if BaseCommand pattern needs record-compatible alternative

### 5. Update Data.Cqrs.EntityFrameworkCore
- Ensure EF Core handlers work with record queries
- Test mapping from entities to record DTOs
- Verify all integrations support records

## Dependencies

### Data.Cqrs.Common
- **Olbrasoft.Mediation** (required)
- **Microsoft.Extensions.DependencyInjection.Abstractions**

### Data.Cqrs.EntityFrameworkCore
- **Olbrasoft.Data.Cqrs.Common** 1.7.0
- **Olbrasoft.Mapping.Abstractions** 7.3.2
- **Entity Framework Core** (version based on target framework)

## Build & Test Commands

```bash
# Navigate to repository
cd ~/Olbrasoft/Data

# Build main solution
dotnet build Data.sln

# Build CQRS solution
dotnet build Data.Cqrs.sln

# Run all tests
dotnet test

# Build specific project
dotnet build src/Data.Cqrs.Common/Data.Cqrs.Common.csproj
```

## GitHub Repository

- **URL:** https://github.com/Olbrasoft/Data
- **CI/CD:** GitHub Actions
  - publish-nuget.yml - Build and publish to NuGet.org
  - Multi-framework builds (net8.0, net9.0, net10.0)

## Notes for Implementation

- Use xUnit for all tests (NOT NUnit)
- Use FluentAssertions for assertions (existing pattern)
- Follow existing test patterns in Data.Cqrs.Common.Tests/
- Consider immutability implications for commands vs queries
- Queries are perfect for records (immutable, value-based)
- Commands need evaluation (status tracking vs immutability)
- Update version in .csproj after changes
- CI/CD will auto-publish to NuGet on version bump
- Test with all three target frameworks (.NET 8, 9, 10)

## Key Differences from Mediation

- **More complex state:** Commands have status tracking
- **EF Core integration:** Must work with DbContext
- **Mapping layer:** Record DTOs need proper mapping support
- **Transaction support:** Ensure records work in transactional scenarios
