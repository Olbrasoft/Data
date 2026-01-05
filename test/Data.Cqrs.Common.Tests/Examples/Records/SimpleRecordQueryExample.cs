namespace Olbrasoft.Data.Cqrs.Examples.Records;

/// <summary>
/// Example demonstrating simple record-based query with CQRS pattern.
/// Shows how to use records for queries and DTOs, providing immutability and concise syntax.
/// </summary>
public class SimpleRecordQueryExample
{
    /// <summary>
    /// Record-based query with primary constructor.
    /// Immutable by default - perfect for CQRS queries.
    /// </summary>
    public record GetUserByIdQuery(int UserId) : IQuery<UserDto>;

    /// <summary>
    /// Record-based DTO for query results.
    /// Value-based equality and concise syntax.
    /// </summary>
    public record UserDto(int Id, string Name, string Email, DateTime CreatedAt);

    /// <summary>
    /// Entity class for database mapping.
    /// Note: Entities are typically classes, not records, for EF Core compatibility.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Handler for record-based query.
    /// Demonstrates EF Core-like projection to record DTO.
    /// </summary>
    public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        private readonly List<User> _users;

        public GetUserByIdHandler()
        {
            // Simulated data source (in real app, this would be DbContext)
            _users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow.AddDays(-30) },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", CreatedAt = DateTime.UtcNow.AddDays(-15) }
            };
        }

        public Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            // In real EF Core scenario:
            // return await _context.Users
            //     .Where(u => u.Id == query.UserId)
            //     .Select(u => new UserDto(u.Id, u.Name, u.Email, u.CreatedAt))
            //     .FirstOrDefaultAsync(cancellationToken);

            var user = _users.FirstOrDefault(u => u.Id == query.UserId);

            if (user == null)
                throw new InvalidOperationException($"User with ID {query.UserId} not found");

            var dto = new UserDto(user.Id, user.Name, user.Email, user.CreatedAt);
            return Task.FromResult(dto);
        }
    }

    [Fact]
    public async Task GetUserByIdQuery_ReturnsUserDto()
    {
        // Arrange
        var query = new GetUserByIdQuery(UserId: 1);
        var handler = new GetUserByIdHandler();

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@example.com");
    }

    [Fact]
    public void GetUserByIdQuery_RecordEquality()
    {
        // Arrange
        var query1 = new GetUserByIdQuery(1);
        var query2 = new GetUserByIdQuery(1);
        var query3 = new GetUserByIdQuery(2);

        // Act & Assert - Records have value-based equality
        query1.Should().Be(query2); // Same value = equal
        query1.Should().NotBe(query3); // Different value = not equal
    }

    [Fact]
    public void GetUserByIdQuery_Immutability()
    {
        // Arrange
        var originalQuery = new GetUserByIdQuery(1);

        // Act - Use 'with' expression to create modified copy
        var modifiedQuery = originalQuery with { UserId = 2 };

        // Assert
        originalQuery.UserId.Should().Be(1); // Original unchanged
        modifiedQuery.UserId.Should().Be(2); // New instance with change
    }

    [Fact]
    public void UserDto_RecordEquality()
    {
        // Arrange
        var dto1 = new UserDto(1, "John", "john@example.com", new DateTime(2024, 1, 1));
        var dto2 = new UserDto(1, "John", "john@example.com", new DateTime(2024, 1, 1));

        // Act & Assert - Records compare by value
        dto1.Should().Be(dto2);
    }
}
