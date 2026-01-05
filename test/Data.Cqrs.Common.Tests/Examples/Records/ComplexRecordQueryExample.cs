namespace Olbrasoft.Data.Cqrs.Examples.Records;

/// <summary>
/// Example demonstrating complex record-based query with nested records, filtering, and pagination.
/// Shows composition of records for complex query parameters and results.
/// </summary>
public class ComplexRecordQueryExample
{
    /// <summary>
    /// Complex query with nested record parameters.
    /// Demonstrates composition of multiple record types.
    /// </summary>
    public record SearchUsersQuery(
        string SearchTerm,
        UserFilter Filter,
        PagingOptions Paging
    ) : IQuery<PagedResult<UserDto>>;

    /// <summary>
    /// Record for filtering options.
    /// Nullable properties allow optional filters.
    /// </summary>
    public record UserFilter(bool? IsActive, DateTime? CreatedAfter);

    /// <summary>
    /// Record for paging parameters.
    /// Immutable configuration for pagination.
    /// </summary>
    public record PagingOptions(int Page, int PageSize)
    {
        // Computed properties work in records
        public int Skip => (Page - 1) * PageSize;
    }

    /// <summary>
    /// Generic paged result record.
    /// Reusable for any paged query result.
    /// </summary>
    public record PagedResult<T>(List<T> Items, int TotalCount, int Page, int PageSize)
    {
        // Computed property for total pages
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    /// <summary>
    /// User DTO record with additional properties.
    /// </summary>
    public record UserDto(int Id, string Name, string Email, bool IsActive, DateTime CreatedAt);

    /// <summary>
    /// Entity class for database mapping.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Handler for complex search query.
    /// Demonstrates filtering, searching, and pagination with records.
    /// </summary>
    public class SearchUsersHandler : IQueryHandler<SearchUsersQuery, PagedResult<UserDto>>
    {
        private readonly List<User> _users;

        public SearchUsersHandler()
        {
            // Simulated data source
            _users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-30) },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
                new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-60) },
                new User { Id = 4, Name = "Alice Williams", Email = "alice@example.com", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-5) }
            };
        }

        public Task<PagedResult<UserDto>> HandleAsync(SearchUsersQuery query, CancellationToken cancellationToken)
        {
            // In real EF Core scenario:
            // var dbQuery = _context.Users.AsQueryable();
            //
            // // Apply filters
            // if (query.Filter.IsActive.HasValue)
            //     dbQuery = dbQuery.Where(u => u.IsActive == query.Filter.IsActive.Value);
            //
            // if (query.Filter.CreatedAfter.HasValue)
            //     dbQuery = dbQuery.Where(u => u.CreatedAt >= query.Filter.CreatedAfter.Value);
            //
            // // Apply search
            // if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            //     dbQuery = dbQuery.Where(u => u.Name.Contains(query.SearchTerm) || u.Email.Contains(query.SearchTerm));
            //
            // // Get total count
            // var totalCount = await dbQuery.CountAsync(cancellationToken);
            //
            // // Apply pagination and projection
            // var items = await dbQuery
            //     .Skip(query.Paging.Skip)
            //     .Take(query.Paging.PageSize)
            //     .Select(u => new UserDto(u.Id, u.Name, u.Email, u.IsActive, u.CreatedAt))
            //     .ToListAsync(cancellationToken);

            var filteredUsers = _users.AsEnumerable();

            // Apply filters
            if (query.Filter.IsActive.HasValue)
                filteredUsers = filteredUsers.Where(u => u.IsActive == query.Filter.IsActive.Value);

            if (query.Filter.CreatedAfter.HasValue)
                filteredUsers = filteredUsers.Where(u => u.CreatedAt >= query.Filter.CreatedAfter.Value);

            // Apply search
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                filteredUsers = filteredUsers.Where(u =>
                    u.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));

            var totalCount = filteredUsers.Count();

            // Apply pagination
            var items = filteredUsers
                .Skip(query.Paging.Skip)
                .Take(query.Paging.PageSize)
                .Select(u => new UserDto(u.Id, u.Name, u.Email, u.IsActive, u.CreatedAt))
                .ToList();

            var result = new PagedResult<UserDto>(items, totalCount, query.Paging.Page, query.Paging.PageSize);
            return Task.FromResult(result);
        }
    }

    [Fact]
    public async Task SearchUsers_WithAllParameters_ReturnsPagedResults()
    {
        // Arrange
        var filter = new UserFilter(IsActive: true, CreatedAfter: DateTime.UtcNow.AddDays(-40));
        var paging = new PagingOptions(Page: 1, PageSize: 10);
        var query = new SearchUsersQuery(SearchTerm: "john", filter, paging);
        var handler = new SearchUsersHandler();

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items[0].Name.Should().Be("John Doe");
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task SearchUsers_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var filter = new UserFilter(IsActive: null, CreatedAfter: null);
        var paging = new PagingOptions(Page: 1, PageSize: 2);
        var query = new SearchUsersQuery(SearchTerm: "", filter, paging);
        var handler = new SearchUsersHandler();

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.TotalCount.Should().Be(4);
        result.Items.Should().HaveCount(2);
        result.TotalPages.Should().Be(2);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void PagingOptions_CalculatesSkipCorrectly()
    {
        // Arrange & Act
        var page1 = new PagingOptions(Page: 1, PageSize: 10);
        var page2 = new PagingOptions(Page: 2, PageSize: 10);
        var page3 = new PagingOptions(Page: 3, PageSize: 20);

        // Assert
        page1.Skip.Should().Be(0);
        page2.Skip.Should().Be(10);
        page3.Skip.Should().Be(40);
    }

    [Fact]
    public void SearchUsersQuery_WithExpression_CreatesModifiedCopy()
    {
        // Arrange
        var originalFilter = new UserFilter(IsActive: true, CreatedAfter: null);
        var originalPaging = new PagingOptions(Page: 1, PageSize: 10);
        var originalQuery = new SearchUsersQuery("test", originalFilter, originalPaging);

        // Act - Modify query using 'with' expression
        var modifiedQuery = originalQuery with
        {
            SearchTerm = "modified",
            Filter = originalFilter with { IsActive = false },
            Paging = originalPaging with { Page = 2 }
        };

        // Assert
        originalQuery.SearchTerm.Should().Be("test");
        originalQuery.Filter.IsActive.Should().BeTrue();
        originalQuery.Paging.Page.Should().Be(1);

        modifiedQuery.SearchTerm.Should().Be("modified");
        modifiedQuery.Filter.IsActive.Should().BeFalse();
        modifiedQuery.Paging.Page.Should().Be(2);
    }
}
