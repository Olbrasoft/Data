# Olbrasoft Data Libraries

[![Build and Publish](https://github.com/Olbrasoft/Data/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/Olbrasoft/Data/actions/workflows/publish-nuget.yml)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)

A collection of .NET libraries for data access patterns, CQRS implementation, and Entity Framework Core integration.

## 📦 NuGet Packages

### Core Libraries

#### <img align="left" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-cqrs.png" width="32" height="32" style="margin-right: 10px"/> Data.Entities.Abstractions
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Entities.Abstractions.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Entities.Abstractions/)
[![Downloads](https://img.shields.io/nuget/dt/Olbrasoft.Data.Entities.Abstractions.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Entities.Abstractions/)

Core abstractions and base types for entity definitions. Provides fundamental interfaces and base classes for domain entities.

```bash
dotnet add package Olbrasoft.Data.Entities.Abstractions
```

**Latest Version:** 1.8.0
**Frameworks:** .NET 8.0, .NET 9.0, .NET 10.0

---

#### <img align="left" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-cqrs.png" width="32" height="32" style="margin-right: 10px"/> Data.Cqrs.Common
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Cqrs.Common.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Cqrs.Common/)
[![Downloads](https://img.shields.io/nuget/dt/Olbrasoft.Data.Cqrs.Common.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Cqrs.Common/)

Common base classes and interfaces for **Command Query Responsibility Segregation (CQRS)** and **Command Query Separation (CQS)** patterns. Implements the foundation for building scalable data access layers.

```bash
dotnet add package Olbrasoft.Data.Cqrs.Common
```

**Latest Version:** 1.7.0
**Frameworks:** .NET 8.0, .NET 9.0, .NET 10.0

**Features:**
- Base query and command interfaces
- Result type abstractions
- Generic repository patterns
- Mediator pattern support via `Olbrasoft.Mediation`

---

#### <img align="left" src="https://raw.githubusercontent.com/Olbrasoft/Data/master/olbrasoft-data-cqrs-entityFrameworkCore.png" width="32" height="32" style="margin-right: 10px"/> Data.Cqrs.EntityFrameworkCore
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Cqrs.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Cqrs.EntityFrameworkCore/)
[![Downloads](https://img.shields.io/nuget/dt/Olbrasoft.Data.Cqrs.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Cqrs.EntityFrameworkCore/)

**Entity Framework Core** integration for CQRS/CQS patterns. Provides ready-to-use implementations of queries and commands backed by EF Core.

```bash
dotnet add package Olbrasoft.Data.Cqrs.EntityFrameworkCore
```

**Latest Version:** 1.8.0
**Frameworks:** .NET 8.0, .NET 9.0, .NET 10.0

**Features:**
- EF Core-based query handlers
- DbContext integration
- Transaction support
- Async/await support
- Mapping abstractions
- Multi-framework support with version-specific EF Core dependencies

**Dependencies:**
- Entity Framework Core 8.0 (.NET 8.0)
- Entity Framework Core 9.0 (.NET 9.0)
- Entity Framework Core 10.0 (.NET 10.0)
- `Olbrasoft.Data.Cqrs.Common` 1.7.0
- `Olbrasoft.Mapping.Abstractions` 7.3.2

---

### Legacy Packages

The following packages are maintained for backward compatibility:

#### Data.Paging
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Paging.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Paging/)

Pagination abstractions and utilities.

#### Data.Sorting
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Sorting.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Sorting/)

Sorting helpers and extensions.

#### Data.Sorting.Extensions
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Sorting.Extensions.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Sorting.Extensions/)

Additional sorting extension methods.

#### Paging.X.PagedList
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Paging.X.PagedList.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Paging.X.PagedList/)

PagedList implementation for pagination.

#### Paging.X.PagedList.AspNetCore.Mvc
[![NuGet](https://img.shields.io/nuget/v/Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/Olbrasoft.Data.Paging.X.PagedList.AspNetCore.Mvc/)

ASP.NET Core MVC pagination helpers with Bootstrap 4 support.

## 🚀 Quick Start

### Basic CQRS Example

```csharp
using Olbrasoft.Data.Cqrs;
using Olbrasoft.Mediation;

// Define a query
public class GetUserByIdQuery : IQuery<UserDto>
{
    public int UserId { get; set; }
}

// Define a query handler
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly YourDbContext _context;

    public GetUserByIdQueryHandler(YourDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Id == query.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        return user?.MapToDto(); // Use your preferred mapping
    }
}

// Usage with mediator
var mediator = serviceProvider.GetRequiredService<IMediator>();
var user = await mediator.SendAsync(new GetUserByIdQuery { UserId = 123 });
```

### Entity Framework Core Integration

```csharp
using Olbrasoft.Data.Cqrs.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Your DbContext
public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
}

// Query handler using EF Core base classes
public class GetUsersQueryHandler : QueryHandler<GetUsersQuery, List<UserDto>>
{
    public GetUsersQueryHandler(MyDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override async Task<List<UserDto>> HandleAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var users = await Context.Users
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);

        return Mapper.Map<List<UserDto>>(users);
    }
}
```

## 📋 Multi-Targeting Support

All core packages support multiple .NET versions:

| Package | .NET 8.0 | .NET 9.0 | .NET 10.0 |
|---------|----------|----------|-----------|
| Data.Entities.Abstractions | ✅ | ✅ | ✅ |
| Data.Cqrs.Common | ✅ | ✅ | ✅ |
| Data.Cqrs.EntityFrameworkCore | ✅ | ✅ | ✅ |

Each target framework gets the appropriate version of Entity Framework Core:
- .NET 8.0 → EF Core 8.0.8
- .NET 9.0 → EF Core 9.0.5
- .NET 10.0 → EF Core 10.0.0

## 🏗️ Architecture

These libraries implement:

- **CQRS Pattern**: Separate read (queries) and write (commands) operations
- **CQS Pattern**: Command-Query Separation principle
- **Repository Pattern**: Data access abstraction
- **Mediator Pattern**: Decoupled request/response handling
- **Unit of Work**: Transaction management with EF Core

## 📖 Documentation

### Core Concepts

**Queries** - Read-only operations that return data without side effects:
```csharp
public interface IQuery<TResult> : IRequest<TResult> { }
```

**Commands** - Write operations that modify state:
```csharp
public interface ICommand : IRequest { }
public interface ICommand<TResult> : IRequest<TResult> { }
```

**Handlers** - Process queries and commands:
```csharp
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
```

### Dependency Injection

```csharp
// ASP.NET Core Program.cs
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString)); // or UseSqlServer, etc.

// Register mediator and handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register mapping
builder.Services.AddAutoMapper(typeof(Program).Assembly);
```

## 🔧 Requirements

- **.NET 8.0** SDK or later (.NET 9.0 or 10.0 recommended)
- **Entity Framework Core** (for Data.Cqrs.EntityFrameworkCore)
- **Olbrasoft.Mediation** package (for CQRS/mediator support)

## 🛠️ Building from Source

```bash
git clone https://github.com/Olbrasoft/Data.git
cd Data
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

## 📦 CI/CD

This repository uses **GitHub Actions** for automated builds and NuGet publishing:

- Automatic builds on every push to `master`
- Automatic NuGet package publishing to NuGet.org
- Multi-framework builds (net8.0, net9.0, net10.0)
- Symbol packages (.snupkg) for debugging

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

- **Author**: Jiří Tůma
- **Company**: Olbrasoft
- **Repository**: [https://github.com/Olbrasoft/Data](https://github.com/Olbrasoft/Data)
- **Issues**: [https://github.com/Olbrasoft/Data/issues](https://github.com/Olbrasoft/Data/issues)

## 🙏 Acknowledgments

- Built with [Entity Framework Core](https://github.com/dotnet/efcore)
- Uses [Olbrasoft.Mediation](https://www.nuget.org/packages/Olbrasoft.Mediation/) for mediator pattern
- Inspired by CQRS and Clean Architecture principles

---

**Copyright © 2022-2025 Olbrasoft**
