namespace Olbrasoft.Data.Cqrs.Examples.Records;

/// <summary>
/// Example demonstrating record DTOs with EF Core-like projections and mapping.
/// Shows how to use records for complex data transfer objects with nested structures.
/// </summary>
public class RecordDtoMappingExample
{
    /// <summary>
    /// Query to get order summaries with nested data.
    /// </summary>
    public record GetOrderSummariesQuery(DateTime From, DateTime To) : IQuery<List<OrderSummaryDto>>;

    /// <summary>
    /// Complex DTO with nested records and computed properties.
    /// Perfect example of using records for read-only data transfer.
    /// </summary>
    public record OrderSummaryDto(
        int OrderId,
        string CustomerName,
        decimal TotalAmount,
        int ItemCount,
        DateTime CreatedAt,
        List<OrderItemDto> Items
    )
    {
        // Computed property - calculates average item price
        public decimal AverageItemPrice => ItemCount > 0 ? TotalAmount / ItemCount : 0;
    }

    /// <summary>
    /// Nested record for order item details.
    /// </summary>
    public record OrderItemDto(
        int ProductId,
        string ProductName,
        decimal Price,
        int Quantity
    )
    {
        // Computed property for line total
        public decimal LineTotal => Price * Quantity;
    }

    /// <summary>
    /// Entity classes for database mapping.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public List<OrderItem> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new();
    }

    /// <summary>
    /// In-memory repository simulating EF Core DbContext.
    /// </summary>
    public class OrderRepository
    {
        private readonly List<Order> _orders;

        public OrderRepository()
        {
            // Create sample data
            var customer1 = new Customer { Id = 1, Name = "John Doe" };
            var customer2 = new Customer { Id = 2, Name = "Jane Smith" };

            var product1 = new Product { Id = 1, Name = "Laptop" };
            var product2 = new Product { Id = 2, Name = "Mouse" };
            var product3 = new Product { Id = 3, Name = "Keyboard" };

            var order1 = new Order
            {
                Id = 1,
                CustomerId = customer1.Id,
                Customer = customer1,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1, ProductId = 1, Product = product1, Price = 1200.00m, Quantity = 1 },
                    new OrderItem { Id = 2, ProductId = 2, Product = product2, Price = 25.00m, Quantity = 2 }
                }
            };

            var order2 = new Order
            {
                Id = 2,
                CustomerId = customer2.Id,
                Customer = customer2,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 3, ProductId = 1, Product = product1, Price = 1200.00m, Quantity = 2 },
                    new OrderItem { Id = 4, ProductId = 3, Product = product3, Price = 75.00m, Quantity = 1 }
                }
            };

            _orders = new List<Order> { order1, order2 };
        }

        public IEnumerable<Order> GetOrders() => _orders;
    }

    /// <summary>
    /// Handler demonstrating EF Core-like projection to record DTOs.
    /// </summary>
    public class GetOrderSummariesHandler : IQueryHandler<GetOrderSummariesQuery, List<OrderSummaryDto>>
    {
        private readonly OrderRepository _repository;

        public GetOrderSummariesHandler(OrderRepository repository)
        {
            _repository = repository;
        }

        public Task<List<OrderSummaryDto>> HandleAsync(GetOrderSummariesQuery query, CancellationToken cancellationToken)
        {
            // In real EF Core scenario with records:
            // var summaries = await _context.Orders
            //     .Where(o => o.CreatedAt >= query.From && o.CreatedAt <= query.To)
            //     .Select(o => new OrderSummaryDto(
            //         o.Id,
            //         o.Customer.Name,
            //         o.Items.Sum(i => i.Price * i.Quantity),
            //         o.Items.Count,
            //         o.CreatedAt,
            //         o.Items.Select(i => new OrderItemDto(
            //             i.ProductId,
            //             i.Product.Name,
            //             i.Price,
            //             i.Quantity
            //         )).ToList()
            //     ))
            //     .ToListAsync(cancellationToken);

            var summaries = _repository.GetOrders()
                .Where(o => o.CreatedAt >= query.From && o.CreatedAt <= query.To)
                .Select(o => new OrderSummaryDto(
                    o.Id,
                    o.Customer.Name,
                    o.Items.Sum(i => i.Price * i.Quantity),
                    o.Items.Count,
                    o.CreatedAt,
                    o.Items.Select(i => new OrderItemDto(
                        i.ProductId,
                        i.Product.Name,
                        i.Price,
                        i.Quantity
                    )).ToList()
                ))
                .ToList();

            return Task.FromResult(summaries);
        }
    }

    [Fact]
    public async Task GetOrderSummaries_ReturnsRecordDtos()
    {
        // Arrange
        var repository = new OrderRepository();
        var query = new GetOrderSummariesQuery(
            From: DateTime.UtcNow.AddDays(-30),
            To: DateTime.UtcNow
        );
        var handler = new GetOrderSummariesHandler(repository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        var order1 = result[0];
        order1.OrderId.Should().Be(1);
        order1.CustomerName.Should().Be("John Doe");
        order1.TotalAmount.Should().Be(1250.00m); // 1200 + (25 * 2)
        order1.ItemCount.Should().Be(2);
        order1.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task OrderSummaryDto_ComputedProperties()
    {
        // Arrange
        var repository = new OrderRepository();
        var query = new GetOrderSummariesQuery(
            From: DateTime.UtcNow.AddDays(-30),
            To: DateTime.UtcNow
        );
        var handler = new GetOrderSummariesHandler(repository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        var order1 = result[0];
        order1.AverageItemPrice.Should().Be(625.00m); // 1250 / 2 items
    }

    [Fact]
    public async Task OrderItemDto_ComputedProperties()
    {
        // Arrange
        var repository = new OrderRepository();
        var query = new GetOrderSummariesQuery(
            From: DateTime.UtcNow.AddDays(-30),
            To: DateTime.UtcNow
        );
        var handler = new GetOrderSummariesHandler(repository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        var firstItem = result[0].Items[0];
        firstItem.LineTotal.Should().Be(1200.00m); // 1200 * 1

        var secondItem = result[0].Items[1];
        secondItem.LineTotal.Should().Be(50.00m); // 25 * 2
    }

    [Fact]
    public void OrderSummaryDto_RecordEquality()
    {
        // Arrange
        var items = new List<OrderItemDto>
        {
            new OrderItemDto(1, "Laptop", 1200.00m, 1)
        };

        var dto1 = new OrderSummaryDto(1, "John", 1200.00m, 1, DateTime.MinValue, items);
        var dto2 = new OrderSummaryDto(1, "John", 1200.00m, 1, DateTime.MinValue, items);

        // Act & Assert - Records use structural equality
        dto1.Should().Be(dto2);
    }

    [Fact]
    public void OrderSummaryDto_WithExpression_CreatesModifiedCopy()
    {
        // Arrange
        var items = new List<OrderItemDto>
        {
            new OrderItemDto(1, "Laptop", 1200.00m, 1)
        };

        var original = new OrderSummaryDto(1, "John", 1200.00m, 1, DateTime.MinValue, items);

        // Act - Create modified copy with different customer name
        var modified = original with { CustomerName = "Jane" };

        // Assert
        original.CustomerName.Should().Be("John");
        modified.CustomerName.Should().Be("Jane");
        modified.OrderId.Should().Be(original.OrderId); // Other properties copied
    }

    [Fact]
    public void NestedRecords_Immutability()
    {
        // Arrange
        var item1 = new OrderItemDto(1, "Laptop", 1200.00m, 1);

        // Act - Modify nested record using 'with'
        var item2 = item1 with { Quantity = 2 };

        // Assert
        item1.Quantity.Should().Be(1);
        item2.Quantity.Should().Be(2);
        item2.LineTotal.Should().Be(2400.00m); // Computed property updates
    }

    /// <summary>
    /// NOTE: Benefits of using records for DTOs:
    /// ✓ Immutable data transfer - safer in multi-threaded scenarios
    /// ✓ Value-based equality - great for caching, testing
    /// ✓ Concise syntax - less boilerplate
    /// ✓ Computed properties - clean, functional style
    /// ✓ EF Core projection support - works seamlessly
    /// ✓ JSON serialization - works out of the box
    /// </summary>
    [Fact]
    public void RecordDtos_Benefits_Documentation()
    {
        var dto = new OrderItemDto(1, "Laptop", 1200m, 1);
        dto.Should().NotBeNull();
        dto.LineTotal.Should().Be(1200m);
    }
}
