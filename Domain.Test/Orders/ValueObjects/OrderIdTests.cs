using Domain.Orders.ValueObjects;

namespace Domain.Test.Orders.ValueObjects;

public class OrderIdTests
{
    [Fact]
    public void CreateUniqueOrderId_Should_Succeed()
    {
        // Arrange

        // Act
        var orderId = OrderId.CreateUnique();

        // Assert
        orderId.Should().BeOfType<OrderId>();
        orderId.Value.GetType().Should().Be(typeof(Guid));
    }
    [Fact]
    public void CreateOrderId_Should_Succeed()
    {
        // Arrange
        var orderIdValue = Guid.NewGuid();

        // Act
        var orderId = OrderId.Create(orderIdValue);

        // Assert
        orderId.Should().BeOfType<OrderId>();
        orderId.Value.GetType().Should().Be(typeof(Guid));
        orderId.Value.Should().Be(orderIdValue);
    }
}