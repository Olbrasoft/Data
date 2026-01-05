namespace Olbrasoft.Data.Cqrs.Examples.Records;

/// <summary>
/// Example demonstrating immutable record-based commands with CQRS pattern.
/// Shows trade-offs between record immutability and command status tracking.
/// </summary>
public class RecordCommandExample
{
    /// <summary>
    /// Immutable record-based command.
    /// Simple and concise - perfect for commands that don't need status tracking.
    /// </summary>
    public record CreateUserCommand(string Name, string Email) : ICommand<int>;

    /// <summary>
    /// Record-based command with validation properties.
    /// Shows how to use records with init-only properties.
    /// </summary>
    public record UpdateUserCommand : ICommand<bool>
    {
        public int UserId { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }

        public UpdateUserCommand(int userId, string? name = null, string? email = null)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }
    }

    /// <summary>
    /// Entity class for database mapping.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// In-memory user repository for examples.
    /// In real application, this would be EF Core DbContext.
    /// </summary>
    public class UserRepository
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public User Add(User user)
        {
            user.Id = _nextId++;
            user.CreatedAt = DateTime.UtcNow;
            _users.Add(user);
            return user;
        }

        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public bool Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing == null) return false;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.UpdatedAt = DateTime.UtcNow;
            return true;
        }
    }

    /// <summary>
    /// Handler for immutable create command.
    /// Simple, straightforward implementation.
    /// </summary>
    public class CreateUserHandler : ICommandHandler<CreateUserCommand, int>
    {
        private readonly UserRepository _repository;

        public CreateUserHandler(UserRepository repository)
        {
            _repository = repository;
        }

        public Task<int> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            // In real EF Core scenario:
            // var user = new User
            // {
            //     Name = command.Name,
            //     Email = command.Email,
            //     CreatedAt = DateTime.UtcNow
            // };
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync(cancellationToken);
            // return user.Id;

            var user = new User
            {
                Name = command.Name,
                Email = command.Email
            };

            _repository.Add(user);

            return Task.FromResult(user.Id);
        }
    }

    /// <summary>
    /// Handler for update command.
    /// Demonstrates handling optional properties in record commands.
    /// </summary>
    public class UpdateUserHandler : ICommandHandler<UpdateUserCommand, bool>
    {
        private readonly UserRepository _repository;

        public UpdateUserHandler(UserRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _repository.GetById(command.UserId);
            if (user == null)
                return Task.FromResult(false);

            // Only update non-null properties
            if (command.Name != null)
                user.Name = command.Name;

            if (command.Email != null)
                user.Email = command.Email;

            var result = _repository.Update(user);
            return Task.FromResult(result);
        }
    }

    [Fact]
    public async Task CreateUserCommand_CreatesUser()
    {
        // Arrange
        var repository = new UserRepository();
        var command = new CreateUserCommand("John Doe", "john@example.com");
        var handler = new CreateUserHandler(repository);

        // Act
        var userId = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        userId.Should().BeGreaterThan(0);
        var user = repository.GetById(userId);
        user.Should().NotBeNull();
        user!.Name.Should().Be("John Doe");
        user.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task UpdateUserCommand_UpdatesUser()
    {
        // Arrange
        var repository = new UserRepository();
        var createCommand = new CreateUserCommand("John Doe", "john@example.com");
        var createHandler = new CreateUserHandler(repository);
        var userId = await createHandler.HandleAsync(createCommand, CancellationToken.None);

        var updateCommand = new UpdateUserCommand(userId, "Jane Doe", null);
        var updateHandler = new UpdateUserHandler(repository);

        // Act
        var result = await updateHandler.HandleAsync(updateCommand, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var user = repository.GetById(userId);
        user!.Name.Should().Be("Jane Doe");
        user.Email.Should().Be("john@example.com"); // Email unchanged
    }

    [Fact]
    public void CreateUserCommand_Immutability()
    {
        // Arrange
        var command = new CreateUserCommand("John", "john@example.com");

        // Act - Cannot modify properties (compile-time safety)
        // command.Name = "Jane"; // This won't compile!

        // Use 'with' expression to create modified copy
        var modifiedCommand = command with { Name = "Jane" };

        // Assert
        command.Name.Should().Be("John");
        modifiedCommand.Name.Should().Be("Jane");
    }

    [Fact]
    public void CreateUserCommand_Equality()
    {
        // Arrange
        var command1 = new CreateUserCommand("John", "john@example.com");
        var command2 = new CreateUserCommand("John", "john@example.com");
        var command3 = new CreateUserCommand("Jane", "jane@example.com");

        // Act & Assert - Records have value-based equality
        command1.Should().Be(command2);
        command1.Should().NotBe(command3);
    }

    [Fact]
    public void UpdateUserCommand_PartialUpdate()
    {
        // Arrange & Act
        var updateNameOnly = new UpdateUserCommand(1, "New Name", null);
        var updateEmailOnly = new UpdateUserCommand(1, null, "new@example.com");
        var updateBoth = new UpdateUserCommand(1, "New Name", "new@example.com");

        // Assert
        updateNameOnly.Name.Should().Be("New Name");
        updateNameOnly.Email.Should().BeNull();

        updateEmailOnly.Name.Should().BeNull();
        updateEmailOnly.Email.Should().Be("new@example.com");

        updateBoth.Name.Should().Be("New Name");
        updateBoth.Email.Should().Be("new@example.com");
    }

    /// <summary>
    /// NOTE: Record commands lose BaseCommand benefits:
    /// - No Status property
    /// - No StatusChanged event
    /// - No automatic status tracking
    ///
    /// Trade-off:
    /// ✓ Immutability (safer, more predictable)
    /// ✓ Value-based equality
    /// ✓ Concise syntax
    /// ✗ No built-in status tracking
    /// ✗ Cannot inherit from BaseCommand
    ///
    /// Use records for simple, stateless commands.
    /// Use BaseCommand classes for commands that need status tracking.
    /// </summary>
    [Fact]
    public void RecordCommand_TradeOffs_Documentation()
    {
        // This test documents the design decision

        // Record approach - immutable, simple
        var recordCommand = new CreateUserCommand("John", "john@example.com");

        // Class approach would be:
        // var classCommand = new CreateUserCommand(executor)
        // {
        //     Name = "John",
        //     Email = "john@example.com"
        // };
        // classCommand.Status = CommandStatus.Pending;
        // await classCommand.ExecuteAsync();
        // // Status automatically updated

        recordCommand.Should().NotBeNull();
    }
}
