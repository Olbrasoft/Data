namespace Data.Cqrs.EntityFrameworkCore.Tests;

/// <summary>
/// Tests for C# record support with CQRS pattern and EF Core.
/// Verifies that records implementing IQuery&lt;T&gt; and ICommand&lt;T&gt; work correctly with EF Core handlers.
/// </summary>
public class RecordQueryCommandTests : IDisposable
{
    #region Test Entities and DTOs

    // Entity for testing
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // Record DTO for query results
    public record ProductDto(int Id, string Name, decimal Price);

    #endregion

    #region Test Record Queries

    // Simple record query with primary constructor
    public record GetProductByIdQuery(int ProductId) : IQuery<Product?>;

    // Record query with multiple properties
    public record GetProductsByCategoryQuery(string Category, bool ActiveOnly) : IQuery<List<Product>>;

    // Record query with nested record result
    public record SearchProductsQuery(string SearchTerm, int MaxResults) : IQuery<List<ProductDto>>;

    #endregion

    #region Test Record Commands

    // Simple record command for creating entity
    public record CreateProductCommand(string Name, decimal Price, string Category) : ICommand<int>;

    // Record command for updating entity
    public record UpdateProductPriceCommand(int ProductId, decimal NewPrice) : ICommand<bool>;

    // Record command with nested record
    public record CreateProductWithDetailsCommand(
        string Name,
        decimal Price,
        ProductDetails Details
    ) : ICommand<int>;

    public record ProductDetails(string Category, bool IsActive);

    #endregion

    #region Test DbContext

    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(e => e.Id);
            modelBuilder.Entity<Product>().Property(e => e.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(e => e.Category).IsRequired().HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }

    #endregion

    #region Query Handlers

    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product?>
    {
        private readonly ProductDbContext _context;

        public GetProductByIdQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(new object[] { query.ProductId }, cancellationToken);
        }
    }

    public class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, List<Product>>
    {
        private readonly ProductDbContext _context;

        public GetProductsByCategoryQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> HandleAsync(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var dbQuery = _context.Products.Where(p => p.Category == query.Category);

            if (query.ActiveOnly)
            {
                dbQuery = dbQuery.Where(p => p.IsActive);
            }

            return await dbQuery.ToListAsync(cancellationToken);
        }
    }

    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, List<ProductDto>>
    {
        private readonly ProductDbContext _context;

        public SearchProductsQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> HandleAsync(SearchProductsQuery query, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(query.SearchTerm))
                .Take(query.MaxResults)
                .Select(p => new ProductDto(p.Id, p.Name, p.Price))
                .ToListAsync(cancellationToken);
        }
    }

    #endregion

    #region Command Handlers

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly ProductDbContext _context;

        public CreateProductCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Price = command.Price,
                Category = command.Category,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }

    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, bool>
    {
        private readonly ProductDbContext _context;

        public UpdateProductPriceCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleAsync(UpdateProductPriceCommand command, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { command.ProductId }, cancellationToken);

            if (product == null)
                return false;

            product.Price = command.NewPrice;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    public class CreateProductWithDetailsCommandHandler : ICommandHandler<CreateProductWithDetailsCommand, int>
    {
        private readonly ProductDbContext _context;

        public CreateProductWithDetailsCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(CreateProductWithDetailsCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Price = command.Price,
                Category = command.Details.Category,
                IsActive = command.Details.IsActive
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }

    #endregion

    #region Test Infrastructure

    private readonly ProductDbContext _context;

    public RecordQueryCommandTests()
    {
        // Use unique database per test class instance (best practice)
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: $"ProductTestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ProductDbContext(options);
        _context.Database.EnsureCreated();

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics", IsActive = true },
            new Product { Id = 2, Name = "Mouse", Price = 29.99m, Category = "Electronics", IsActive = true },
            new Product { Id = 3, Name = "Keyboard", Price = 79.99m, Category = "Electronics", IsActive = false },
            new Product { Id = 4, Name = "Desk", Price = 299.99m, Category = "Furniture", IsActive = true },
            new Product { Id = 5, Name = "Chair", Price = 199.99m, Category = "Furniture", IsActive = true }
        };

        _context.Products.AddRange(products);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #endregion

    #region Basic Record Query Tests

    [Fact]
    public void Record_Query_Should_Implement_IQuery_Interface()
    {
        // Arrange
        var query = new GetProductByIdQuery(1);

        // Act
        var implementsInterface = query is IQuery<Product?>;

        // Assert
        Assert.True(implementsInterface);
    }

    [Fact]
    public void Record_Query_With_Primary_Constructor_Should_Have_Correct_Property_Values()
    {
        // Arrange & Act
        var query = new GetProductByIdQuery(42);

        // Assert
        Assert.Equal(42, query.ProductId);
    }

    [Fact]
    public void Record_Query_With_Multiple_Properties_Should_Preserve_All_Values()
    {
        // Arrange & Act
        var query = new GetProductsByCategoryQuery("Electronics", true);

        // Assert
        Assert.Equal("Electronics", query.Category);
        Assert.True(query.ActiveOnly);
    }

    [Fact]
    public void Record_Query_Should_Support_Value_Equality()
    {
        // Arrange
        var query1 = new GetProductByIdQuery(1);
        var query2 = new GetProductByIdQuery(1);
        var query3 = new GetProductByIdQuery(2);

        // Assert
        Assert.Equal(query1, query2); // Value equality
        Assert.NotEqual(query1, query3);
    }

    [Fact]
    public void Record_Query_Should_Be_Immutable()
    {
        // Arrange
        var original = new GetProductByIdQuery(1);

        // Act - Records are immutable, this creates a new instance
        var modified = original with { ProductId = 2 };

        // Assert
        Assert.Equal(1, original.ProductId);
        Assert.Equal(2, modified.ProductId);
        Assert.NotSame(original, modified);
    }

    #endregion

    #region Basic Record Command Tests

    [Fact]
    public void Record_Command_Should_Implement_ICommand_Interface()
    {
        // Arrange
        var command = new CreateProductCommand("Test", 100m, "Test");

        // Act
        var implementsInterface = command is ICommand<int>;

        // Assert
        Assert.True(implementsInterface);
    }

    [Fact]
    public void Record_Command_With_Primary_Constructor_Should_Preserve_Values()
    {
        // Arrange & Act
        var command = new CreateProductCommand("Laptop", 999.99m, "Electronics");

        // Assert
        Assert.Equal("Laptop", command.Name);
        Assert.Equal(999.99m, command.Price);
        Assert.Equal("Electronics", command.Category);
    }

    [Fact]
    public void Record_Command_Should_Support_Value_Equality()
    {
        // Arrange
        var command1 = new CreateProductCommand("Test", 100m, "Category");
        var command2 = new CreateProductCommand("Test", 100m, "Category");
        var command3 = new CreateProductCommand("Different", 100m, "Category");

        // Assert
        Assert.Equal(command1, command2);
        Assert.NotEqual(command1, command3);
    }

    [Fact]
    public void Record_Command_With_Nested_Record_Should_Preserve_Structure()
    {
        // Arrange & Act
        var details = new ProductDetails("Electronics", true);
        var command = new CreateProductWithDetailsCommand("Laptop", 999.99m, details);

        // Assert
        Assert.Equal("Laptop", command.Name);
        Assert.Equal(999.99m, command.Price);
        Assert.Equal("Electronics", command.Details.Category);
        Assert.True(command.Details.IsActive);
    }

    #endregion

    #region Query Handler Tests with EF Core

    [Fact]
    public async Task Record_Query_Handler_Should_Retrieve_Product_By_Id()
    {
        // Arrange
        var handler = new GetProductByIdQueryHandler(_context);
        var query = new GetProductByIdQuery(1);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal(999.99m, result.Price);
    }

    [Fact]
    public async Task Record_Query_Handler_Should_Return_Null_For_NonExistent_Product()
    {
        // Arrange
        var handler = new GetProductByIdQueryHandler(_context);
        var query = new GetProductByIdQuery(999);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Record_Query_With_Multiple_Properties_Should_Filter_Correctly()
    {
        // Arrange
        var handler = new GetProductsByCategoryQueryHandler(_context);
        var query = new GetProductsByCategoryQuery("Electronics", true);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count); // Laptop and Mouse (Keyboard is inactive)
        Assert.All(result, p => Assert.Equal("Electronics", p.Category));
        Assert.All(result, p => Assert.True(p.IsActive));
    }

    [Fact]
    public async Task Record_Query_Without_ActiveOnly_Filter_Should_Return_All_Products()
    {
        // Arrange
        var handler = new GetProductsByCategoryQueryHandler(_context);
        var query = new GetProductsByCategoryQuery("Electronics", false);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count); // All electronics including inactive Keyboard
    }

    [Fact]
    public async Task Record_Query_Returning_Record_DTO_Should_Map_Correctly()
    {
        // Arrange
        var handler = new SearchProductsQueryHandler(_context);
        var query = new SearchProductsQuery("Laptop", 10);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Laptop", result[0].Name);
        Assert.Equal(999.99m, result[0].Price);
    }

    [Fact]
    public async Task Record_Query_Should_Respect_MaxResults_Limit()
    {
        // Arrange
        var handler = new SearchProductsQueryHandler(_context);
        var query = new SearchProductsQuery("", 2); // Empty search returns all, but limited to 2

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
    }

    #endregion

    #region Command Handler Tests with EF Core

    [Fact]
    public async Task Record_Command_Handler_Should_Create_Product()
    {
        // Arrange
        var handler = new CreateProductCommandHandler(_context);
        var command = new CreateProductCommand("New Product", 49.99m, "Electronics");

        // Act
        var productId = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(productId > 0);

        var createdProduct = await _context.Products.FindAsync(productId);
        Assert.NotNull(createdProduct);
        Assert.Equal("New Product", createdProduct.Name);
        Assert.Equal(49.99m, createdProduct.Price);
        Assert.Equal("Electronics", createdProduct.Category);
    }

    [Fact]
    public async Task Record_Command_Handler_Should_Update_Product_Price()
    {
        // Arrange
        var handler = new UpdateProductPriceCommandHandler(_context);
        var command = new UpdateProductPriceCommand(1, 1099.99m);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        var updatedProduct = await _context.Products.FindAsync(1);
        Assert.NotNull(updatedProduct);
        Assert.Equal(1099.99m, updatedProduct.Price);
    }

    [Fact]
    public async Task Record_Command_Handler_Should_Return_False_For_NonExistent_Product()
    {
        // Arrange
        var handler = new UpdateProductPriceCommandHandler(_context);
        var command = new UpdateProductPriceCommand(999, 100m);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Record_Command_With_Nested_Record_Should_Create_Product_With_Details()
    {
        // Arrange
        var handler = new CreateProductWithDetailsCommandHandler(_context);
        var details = new ProductDetails("Furniture", false);
        var command = new CreateProductWithDetailsCommand("Bookshelf", 149.99m, details);

        // Act
        var productId = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(productId > 0);

        var createdProduct = await _context.Products.FindAsync(productId);
        Assert.NotNull(createdProduct);
        Assert.Equal("Bookshelf", createdProduct.Name);
        Assert.Equal("Furniture", createdProduct.Category);
        Assert.False(createdProduct.IsActive);
    }

    #endregion

    #region Record Immutability Tests

    [Fact]
    public void Record_Query_With_Expression_Should_Create_New_Instance()
    {
        // Arrange
        var original = new GetProductsByCategoryQuery("Electronics", true);

        // Act
        var modified = original with { Category = "Furniture" };

        // Assert
        Assert.Equal("Electronics", original.Category);
        Assert.Equal("Furniture", modified.Category);
        Assert.True(modified.ActiveOnly); // ActiveOnly preserved
        Assert.NotSame(original, modified);
    }

    [Fact]
    public void Record_Command_With_Expression_Should_Create_New_Instance()
    {
        // Arrange
        var original = new CreateProductCommand("Original", 100m, "Category");

        // Act
        var modified = original with { Name = "Modified" };

        // Assert
        Assert.Equal("Original", original.Name);
        Assert.Equal("Modified", modified.Name);
        Assert.Equal(100m, modified.Price); // Price preserved
        Assert.Equal("Category", modified.Category); // Category preserved
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task Complete_Workflow_With_Records_Should_Work_End_To_End()
    {
        // Arrange
        var createHandler = new CreateProductCommandHandler(_context);
        var queryHandler = new GetProductByIdQueryHandler(_context);
        var updateHandler = new UpdateProductPriceCommandHandler(_context);

        // Act 1: Create product using record command
        var createCommand = new CreateProductCommand("Workflow Test Product", 59.99m, "Test");
        var productId = await createHandler.HandleAsync(createCommand, CancellationToken.None);

        // Act 2: Query product using record query
        var getQuery = new GetProductByIdQuery(productId);
        var product = await queryHandler.HandleAsync(getQuery, CancellationToken.None);

        // Assert product was created correctly
        Assert.NotNull(product);
        Assert.Equal("Workflow Test Product", product.Name);
        Assert.Equal(59.99m, product.Price);

        // Act 3: Update product using record command
        var updateCommand = new UpdateProductPriceCommand(productId, 69.99m);
        var updated = await updateHandler.HandleAsync(updateCommand, CancellationToken.None);

        // Assert update was successful
        Assert.True(updated);

        // Act 4: Verify update using record query
        var verifyQuery = new GetProductByIdQuery(productId);
        var updatedProduct = await queryHandler.HandleAsync(verifyQuery, CancellationToken.None);

        // Assert product was updated correctly
        Assert.NotNull(updatedProduct);
        Assert.Equal(69.99m, updatedProduct.Price);
    }

    #endregion
}
