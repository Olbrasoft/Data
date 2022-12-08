using FluentAssertions;
using Olbrasoft.Data.Entities;

namespace Data.Entities.Abstractions.Tests;
public class EntityIdTests
{
    [Fact]
    public void Is_Abstract()
    {
        //Arrange
        var type = typeof(EntityId);

        //Assert
        type.Should().BeAbstract();
    }

    [Fact]
    public void TypeOf_EntityId_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(IHaveId).Assembly;

        // Act
        var result = typeof(EntityId).Assembly;

        // Assert
        result.Should().BeSameAs(expected);
    }



}
