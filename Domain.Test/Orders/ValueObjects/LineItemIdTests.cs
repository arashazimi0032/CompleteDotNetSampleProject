using Domain.Orders.ValueObjects;

namespace Domain.Test.Orders.ValueObjects;

public class LineItemIdTests
{
    [Fact]
    public void CreateUniqueLineItemId_Should_Succeed()
    {
        // Arrange

        // Act
        var lineItemId = LineItemId.CreateUnique();

        // Assert
        lineItemId.Should().BeOfType<LineItemId>();
        lineItemId.Value.GetType().Should().Be(typeof(Guid));
    }
    [Fact]
    public void CreateLineItemId_Should_Succeed()
    {
        // Arrange
        var lineItemIdValue = Guid.NewGuid();

        // Act
        var lineItemId = LineItemId.Create(lineItemIdValue);

        // Assert
        lineItemId.Should().BeOfType<LineItemId>();
        lineItemId.Value.GetType().Should().Be(typeof(Guid));
        lineItemId.Value.Should().Be(lineItemIdValue);
    }
}