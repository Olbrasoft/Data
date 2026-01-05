namespace Olbrasoft.Data.Cqrs.Examples.Records;

/// <summary>
/// Example comparing record-based vs class-based CQRS implementations.
/// Shows trade-offs, advantages, and when to use each approach.
/// </summary>
public class RecordVsClassComparisonExample
{
    #region Record-Based Query (Recommended)

    /// <summary>
    /// Record-based query - concise, immutable, value-based equality.
    /// ✓ Recommended approach for queries
    /// </summary>
    public record GetProductRecordQuery(int ProductId) : IQuery<ProductDto>;

    /// <summary>
    /// Record DTO - perfect for read-only data transfer.
    /// </summary>
    public record ProductDto(int Id, string Name, decimal Price, int Stock);

    #endregion

    #region Class-Based Query (Traditional)

    /// <summary>
    /// Class-based query inheriting from BaseQuery.
    /// Has optional IQueryProcessor dependency injection.
    /// </summary>
    public class GetProductClassQuery : BaseQuery<ProductDto>
    {
        public int ProductId { get; set; }

        // Required parameterless constructor for some scenarios
        public GetProductClassQuery() { }

        public GetProductClassQuery(int productId)
        {
            ProductId = productId;
        }

        // Can optionally use IQueryProcessor
        public GetProductClassQuery(IQueryProcessor processor, int productId) : base(processor)
        {
            ProductId = productId;
        }
    }

    #endregion

    #region Record-Based Command (Simple)

    /// <summary>
    /// Record-based command - immutable, no status tracking.
    /// ✓ Use for simple, stateless commands
    /// ✗ No Status property, no events
    /// </summary>
    public record UpdateProductPriceRecordCommand(int ProductId, decimal NewPrice) : ICommand<bool>;

    #endregion

    #region Class-Based Command (Stateful)

    /// <summary>
    /// Class-based command inheriting from BaseCommand.
    /// Has Status property and StatusChanged event.
    /// ✓ Use for commands that need status tracking
    /// </summary>
    public class UpdateProductPriceClassCommand : BaseCommand<bool>
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }

        // Required parameterless constructor
        public UpdateProductPriceClassCommand() { }

        public UpdateProductPriceClassCommand(int productId, decimal newPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
        }

        // Can use ICommandExecutor for automatic status tracking
        public UpdateProductPriceClassCommand(ICommandExecutor executor, int productId, decimal newPrice)
            : base(executor)
        {
            ProductId = productId;
            NewPrice = newPrice;
        }
    }

    #endregion

    #region Shared Entity and Repository

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class ProductRepository
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200.00m, Stock = 10 },
            new Product { Id = 2, Name = "Mouse", Price = 25.00m, Stock = 50 }
        };

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public bool UpdatePrice(int id, decimal newPrice)
        {
            var product = GetById(id);
            if (product == null) return false;

            product.Price = newPrice;
            return true;
        }
    }

    #endregion

    #region Query Handlers

    public class GetProductRecordHandler : IQueryHandler<GetProductRecordQuery, ProductDto>
    {
        private readonly ProductRepository _repository;

        public GetProductRecordHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<ProductDto> HandleAsync(GetProductRecordQuery query, CancellationToken cancellationToken)
        {
            var product = _repository.GetById(query.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product {query.ProductId} not found");

            var dto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);
            return Task.FromResult(dto);
        }
    }

    public class GetProductClassHandler : IQueryHandler<GetProductClassQuery, ProductDto>
    {
        private readonly ProductRepository _repository;

        public GetProductClassHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<ProductDto> HandleAsync(GetProductClassQuery query, CancellationToken cancellationToken)
        {
            var product = _repository.GetById(query.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product {query.ProductId} not found");

            var dto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);
            return Task.FromResult(dto);
        }
    }

    #endregion

    #region Command Handlers

    public class UpdateProductPriceRecordHandler : ICommandHandler<UpdateProductPriceRecordCommand, bool>
    {
        private readonly ProductRepository _repository;

        public UpdateProductPriceRecordHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> HandleAsync(UpdateProductPriceRecordCommand command, CancellationToken cancellationToken)
        {
            var result = _repository.UpdatePrice(command.ProductId, command.NewPrice);
            return Task.FromResult(result);
        }
    }

    public class UpdateProductPriceClassHandler : ICommandHandler<UpdateProductPriceClassCommand, bool>
    {
        private readonly ProductRepository _repository;

        public UpdateProductPriceClassHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> HandleAsync(UpdateProductPriceClassCommand command, CancellationToken cancellationToken)
        {
            // Class-based command has Status property
            // command.Status can be tracked/updated if needed

            var result = _repository.UpdatePrice(command.ProductId, command.NewPrice);
            return Task.FromResult(result);
        }
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public async Task RecordQuery_VsClassQuery_BothWork()
    {
        // Arrange
        var repository = new ProductRepository();

        var recordQuery = new GetProductRecordQuery(1);
        var recordHandler = new GetProductRecordHandler(repository);

        var classQuery = new GetProductClassQuery(1);
        var classHandler = new GetProductClassHandler(repository);

        // Act
        var recordResult = await recordHandler.HandleAsync(recordQuery, CancellationToken.None);
        var classResult = await classHandler.HandleAsync(classQuery, CancellationToken.None);

        // Assert - Both produce same result
        recordResult.Should().Be(classResult);
        recordResult.Name.Should().Be("Laptop");
    }

    [Fact]
    public void RecordQuery_Conciseness()
    {
        // Record - One line, primary constructor
        var recordQuery = new GetProductRecordQuery(1);

        // Class - More verbose, needs property initialization
        var classQuery = new GetProductClassQuery { ProductId = 1 };
        // Or with constructor
        var classQuery2 = new GetProductClassQuery(1);

        recordQuery.ProductId.Should().Be(1);
        classQuery.ProductId.Should().Be(1);
        classQuery2.ProductId.Should().Be(1);
    }

    [Fact]
    public void RecordQuery_Immutability()
    {
        // Record - Immutable by default
        var recordQuery = new GetProductRecordQuery(1);
        // recordQuery.ProductId = 2; // Won't compile!

        var modified = recordQuery with { ProductId = 2 };
        recordQuery.ProductId.Should().Be(1);
        modified.ProductId.Should().Be(2);

        // Class - Mutable by default
        var classQuery = new GetProductClassQuery(1);
        classQuery.ProductId = 2; // Allowed - can lead to bugs
        classQuery.ProductId.Should().Be(2);
    }

    [Fact]
    public void RecordQuery_Equality()
    {
        // Records - Value-based equality
        var record1 = new GetProductRecordQuery(1);
        var record2 = new GetProductRecordQuery(1);
        record1.Should().Be(record2); // Equal by value

        // Classes - Reference equality (unless overridden)
        var class1 = new GetProductClassQuery(1);
        var class2 = new GetProductClassQuery(1);
        class1.Should().NotBe(class2); // Different instances (BaseQuery doesn't override Equals)
    }

    [Fact]
    public async Task RecordCommand_VsClassCommand_BothWork()
    {
        // Arrange
        var repository = new ProductRepository();

        var recordCommand = new UpdateProductPriceRecordCommand(1, 1500.00m);
        var recordHandler = new UpdateProductPriceRecordHandler(repository);

        // Reset for class command test
        var repository2 = new ProductRepository();
        var classCommand = new UpdateProductPriceClassCommand(1, 1500.00m);
        var classHandler = new UpdateProductPriceClassHandler(repository2);

        // Act
        var recordResult = await recordHandler.HandleAsync(recordCommand, CancellationToken.None);
        var classResult = await classHandler.HandleAsync(classCommand, CancellationToken.None);

        // Assert - Both work
        recordResult.Should().BeTrue();
        classResult.Should().BeTrue();

        repository.GetById(1)!.Price.Should().Be(1500.00m);
        repository2.GetById(1)!.Price.Should().Be(1500.00m);
    }

    [Fact]
    public void ClassCommand_HasStatusTracking()
    {
        // Class command has Status property
        var classCommand = new UpdateProductPriceClassCommand(1, 1500.00m);

        classCommand.Status.Should().Be(CommandStatus.Default); // Default status
        classCommand.Status = CommandStatus.Success;
        classCommand.Status.Should().Be(CommandStatus.Success);

        // Record command has no Status property
        _ = new UpdateProductPriceRecordCommand(1, 1500.00m);
        // recordCommand.Status - Property doesn't exist
    }

    #endregion

    #region Decision Guide

    /// <summary>
    /// DECISION GUIDE:
    ///
    /// USE RECORDS WHEN:
    /// ✓ Implementing queries (always recommended)
    /// ✓ Creating DTOs for read-only data
    /// ✓ Want immutability by default
    /// ✓ Need value-based equality
    /// ✓ Prefer concise syntax
    /// ✓ Don't need status tracking
    ///
    /// USE CLASSES WHEN:
    /// ✓ Need mutable state
    /// ✓ Want status tracking (BaseCommand)
    /// ✓ Need StatusChanged events
    /// ✓ Require IQueryProcessor/ICommandExecutor injection
    /// ✓ Working with legacy code expecting classes
    ///
    /// PERFORMANCE:
    /// - Records and classes have similar performance
    /// - Records may have slight overhead for equality comparisons
    /// - For CQRS, the difference is negligible
    ///
    /// COMPATIBILITY:
    /// ✓ Both work with IQuery<T> and ICommand<T>
    /// ✓ Both work with handlers
    /// ✓ Both work with mediator pattern
    /// ✓ Both work with EF Core
    ///
    /// MIGRATION:
    /// - Can mix records and classes in same codebase
    /// - Gradually migrate queries to records
    /// - Keep classes for stateful commands if needed
    /// </summary>
    [Fact]
    public void DecisionGuide_Documentation()
    {
        // This test documents the decision criteria
        var recordQuery = new GetProductRecordQuery(1);
        var classCommand = new UpdateProductPriceClassCommand(1, 100m);

        recordQuery.Should().NotBeNull();
        classCommand.Should().NotBeNull();
        classCommand.Status.Should().Be(CommandStatus.Default);
    }

    #endregion
}
