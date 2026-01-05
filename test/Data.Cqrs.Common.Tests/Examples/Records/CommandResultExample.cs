namespace Olbrasoft.Data.Cqrs.Examples.Records;

/// <summary>
/// Example demonstrating CommandResult&lt;T&gt; pattern with immutable record commands.
/// Shows how to maintain status tracking while keeping commands fully immutable.
/// </summary>
public class CommandResultExample
{
    #region Commands - Immutable Records

    /// <summary>
    /// Immutable command using CommandResult&lt;T&gt; for status tracking.
    /// No mutable state - status is part of the result.
    /// </summary>
    public record CreateProductCommand(
        string Name,
        decimal Price,
        int Stock
    ) : ICommand<CommandResult<int>>;

    /// <summary>
    /// Immutable command for updates.
    /// </summary>
    public record UpdateProductPriceCommand(
        int ProductId,
        decimal NewPrice
    ) : ICommand<CommandResult<ProductDto>>;

    /// <summary>
    /// Immutable command for deletion.
    /// </summary>
    public record DeleteProductCommand(int ProductId) : ICommand<CommandResult<bool>>;

    #endregion

    #region DTOs

    public record ProductDto(int Id, string Name, decimal Price, int Stock);

    #endregion

    #region Entities

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    #endregion

    #region Repository

    public class ProductRepository
    {
        private readonly List<Product> _products = new();
        private int _nextId = 1;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Add(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }

        public bool Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing == null) return false;

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            return true;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }

        public bool Exists(string name) => _products.Any(p => p.Name == name);
    }

    #endregion

    #region Handlers - Demonstrating CommandResult Pattern

    /// <summary>
    /// Handler demonstrating success, conflict, and validation scenarios.
    /// </summary>
    public class CreateProductHandler : ICommandHandler<CreateProductCommand, CommandResult<int>>
    {
        private readonly ProductRepository _repository;

        public CreateProductHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<CommandResult<int>> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(command.Name))
                return Task.FromResult(CommandResult<int>.Failure("Product name is required", CommandStatus.Conflict));

            if (command.Price <= 0)
                return Task.FromResult(CommandResult<int>.Failure("Price must be greater than zero", CommandStatus.Conflict));

            if (command.Stock < 0)
                return Task.FromResult(CommandResult<int>.Failure("Stock cannot be negative", CommandStatus.Conflict));

            // Business rule - no duplicates
            if (_repository.Exists(command.Name))
                return Task.FromResult(CommandResult<int>.Conflict($"Product '{command.Name}' already exists"));

            // Create product
            var product = new Product
            {
                Name = command.Name,
                Price = command.Price,
                Stock = command.Stock
            };

            _repository.Add(product);

            // Return success with Created status
            return Task.FromResult(CommandResult<int>.Created(product.Id));
        }
    }

    /// <summary>
    /// Handler demonstrating update with NotFound and Unchanged scenarios.
    /// </summary>
    public class UpdateProductPriceHandler : ICommandHandler<UpdateProductPriceCommand, CommandResult<ProductDto>>
    {
        private readonly ProductRepository _repository;

        public UpdateProductPriceHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<CommandResult<ProductDto>> HandleAsync(UpdateProductPriceCommand command, CancellationToken cancellationToken)
        {
            // Validation
            if (command.NewPrice <= 0)
                return Task.FromResult(CommandResult<ProductDto>.Failure("Price must be greater than zero", CommandStatus.Conflict));

            // Get entity
            var product = _repository.GetById(command.ProductId);
            if (product == null)
                return Task.FromResult(CommandResult<ProductDto>.NotFound($"Product with ID {command.ProductId} not found"));

            // Check if price actually changed
            if (product.Price == command.NewPrice)
            {
                var unchangedDto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);
                return Task.FromResult(CommandResult<ProductDto>.Unchanged(unchangedDto));
            }

            // Update price
            product.Price = command.NewPrice;
            _repository.Update(product);

            // Return modified result
            var dto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);
            return Task.FromResult(CommandResult<ProductDto>.Modified(dto));
        }
    }

    /// <summary>
    /// Handler demonstrating deletion with NotFound scenario.
    /// </summary>
    public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, CommandResult<bool>>
    {
        private readonly ProductRepository _repository;

        public DeleteProductHandler(ProductRepository repository)
        {
            _repository = repository;
        }

        public Task<CommandResult<bool>> HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = _repository.GetById(command.ProductId);
            if (product == null)
                return Task.FromResult(CommandResult<bool>.NotFound($"Product with ID {command.ProductId} not found"));

            var deleted = _repository.Delete(command.ProductId);

            // Return deleted result
            return Task.FromResult(CommandResult<bool>.Deleted(deleted));
        }
    }

    #endregion

    #region Tests - Success Scenarios

    [Fact]
    public async Task CreateProduct_Success_ReturnsCreatedStatus()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new CreateProductCommand("Laptop", 1200.00m, 10);
        var handler = new CreateProductHandler(repository);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Created);
        result.Data.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateProductPrice_Success_ReturnsModifiedStatus()
    {
        // Arrange
        var repository = new ProductRepository();
        var createCommand = new CreateProductCommand("Laptop", 1200.00m, 10);
        var createHandler = new CreateProductHandler(repository);
        var createResult = await createHandler.HandleAsync(createCommand, CancellationToken.None);

        var updateCommand = new UpdateProductPriceCommand(createResult.Data!, 1500.00m);
        var updateHandler = new UpdateProductPriceHandler(repository);

        // Act
        var result = await updateHandler.HandleAsync(updateCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Modified);
        result.Data!.Price.Should().Be(1500.00m);
    }

    [Fact]
    public async Task DeleteProduct_Success_ReturnsDeletedStatus()
    {
        // Arrange
        var repository = new ProductRepository();
        var createCommand = new CreateProductCommand("Laptop", 1200.00m, 10);
        var createHandler = new CreateProductHandler(repository);
        var createResult = await createHandler.HandleAsync(createCommand, CancellationToken.None);

        var deleteCommand = new DeleteProductCommand(createResult.Data!);
        var deleteHandler = new DeleteProductHandler(repository);

        // Act
        var result = await deleteHandler.HandleAsync(deleteCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Deleted);
        result.Data.Should().BeTrue();
    }

    #endregion

    #region Tests - Error Scenarios

    [Fact]
    public async Task CreateProduct_DuplicateName_ReturnsConflict()
    {
        // Arrange
        var repository = new ProductRepository();
        var handler = new CreateProductHandler(repository);

        // Create first product
        var command1 = new CreateProductCommand("Laptop", 1200.00m, 10);
        await handler.HandleAsync(command1, CancellationToken.None);

        // Try to create duplicate
        var command2 = new CreateProductCommand("Laptop", 1500.00m, 5);

        // Act
        var result = await handler.HandleAsync(command2, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Conflict);
        result.ErrorMessage.Should().Contain("already exists");
    }

    [Fact]
    public async Task CreateProduct_InvalidPrice_ReturnsConflict()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new CreateProductCommand("Laptop", -100.00m, 10);
        var handler = new CreateProductHandler(repository);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Conflict);
        result.ErrorMessage.Should().Contain("greater than zero");
    }

    [Fact]
    public async Task UpdateProductPrice_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new UpdateProductPriceCommand(999, 1500.00m);
        var handler = new UpdateProductPriceHandler(repository);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.NotFound);
        result.ErrorMessage.Should().Contain("not found");
    }

    [Fact]
    public async Task UpdateProductPrice_SamePrice_ReturnsUnchanged()
    {
        // Arrange
        var repository = new ProductRepository();
        var createCommand = new CreateProductCommand("Laptop", 1200.00m, 10);
        var createHandler = new CreateProductHandler(repository);
        var createResult = await createHandler.HandleAsync(createCommand, CancellationToken.None);

        var updateCommand = new UpdateProductPriceCommand(createResult.Data!, 1200.00m);
        var updateHandler = new UpdateProductPriceHandler(repository);

        // Act
        var result = await updateHandler.HandleAsync(updateCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.Unchanged);
        result.Data!.Price.Should().Be(1200.00m);
    }

    [Fact]
    public async Task DeleteProduct_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new DeleteProductCommand(999);
        var handler = new DeleteProductHandler(repository);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(CommandStatus.NotFound);
    }

    #endregion

    #region Functional Programming Patterns

    [Fact]
    public async Task CommandResult_OnSuccess_ExecutesAction()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new CreateProductCommand("Laptop", 1200.00m, 10);
        var handler = new CreateProductHandler(repository);

        var productId = 0;

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);
        result.OnSuccess(id => productId = id);

        // Assert
        productId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CommandResult_OnFailure_ExecutesAction()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new CreateProductCommand("", 1200.00m, 10); // Invalid name
        var handler = new CreateProductHandler(repository);

        var errorMessage = "";

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);
        result.OnFailure(error => errorMessage = error);

        // Assert
        errorMessage.Should().Contain("required");
    }

    [Fact]
    public async Task CommandResult_Map_TransformsSuccessfulResult()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new CreateProductCommand("Laptop", 1200.00m, 10);
        var handler = new CreateProductHandler(repository);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);
        var mappedResult = result.Map(id => $"Product ID: {id}");

        // Assert
        mappedResult.IsSuccess.Should().BeTrue();
        mappedResult.Data.Should().StartWith("Product ID:");
    }

    [Fact]
    public async Task CommandResult_ChainedOperations()
    {
        // Arrange
        var repository = new ProductRepository();
        var command = new CreateProductCommand("Laptop", 1200.00m, 10);
        var handler = new CreateProductHandler(repository);

        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);
        result
            .OnSuccess(_ => successCalled = true)
            .OnFailure(_ => failureCalled = true);

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    #endregion

    #region Command Immutability Tests

    [Fact]
    public void Commands_AreFullyImmutable()
    {
        // Arrange
        var originalCommand = new CreateProductCommand("Laptop", 1200.00m, 10);

        // Act - Use 'with' expression to create modified copy
        var modifiedCommand = originalCommand with { Name = "Desktop", Price = 1500.00m };

        // Assert
        originalCommand.Name.Should().Be("Laptop");
        originalCommand.Price.Should().Be(1200.00m);

        modifiedCommand.Name.Should().Be("Desktop");
        modifiedCommand.Price.Should().Be(1500.00m);
    }

    [Fact]
    public void Commands_HaveValueBasedEquality()
    {
        // Arrange
        var command1 = new CreateProductCommand("Laptop", 1200.00m, 10);
        var command2 = new CreateProductCommand("Laptop", 1200.00m, 10);
        var command3 = new CreateProductCommand("Desktop", 1500.00m, 5);

        // Act & Assert
        command1.Should().Be(command2); // Same values = equal
        command1.Should().NotBe(command3); // Different values = not equal
    }

    #endregion

    /// <summary>
    /// PATTERN BENEFITS:
    ///
    /// ✅ Commands are fully immutable (C# records work perfectly)
    /// ✅ Status tracking maintained via CommandResult&lt;T&gt;
    /// ✅ Clean separation of concerns (status in result, not command)
    /// ✅ Matches industry best practices (MediatR, Azure CQRS)
    /// ✅ Type-safe and compiler-verified
    /// ✅ Easy to test and reason about
    /// ✅ Supports functional programming patterns (Map, OnSuccess, OnFailure)
    /// ✅ Works with existing ICommand&lt;T&gt; interface
    ///
    /// COMPARISON WITH BaseCommand&lt;T&gt;:
    ///
    /// BaseCommand&lt;T&gt; (old):
    /// - ❌ Mutable state (Status property)
    /// - ❌ Cannot use with records
    /// - ❌ Events and side effects
    /// - ✅ Built-in status tracking
    ///
    /// CommandResult&lt;T&gt; (new):
    /// - ✅ Fully immutable
    /// - ✅ Works with records
    /// - ✅ No side effects
    /// - ✅ Status part of result
    /// - ✅ Clean, functional approach
    /// </summary>
    [Fact]
    public void PatternBenefits_Documentation()
    {
        var command = new CreateProductCommand("Laptop", 1200.00m, 10);
        var result = CommandResult<int>.Created(1);

        command.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }
}
