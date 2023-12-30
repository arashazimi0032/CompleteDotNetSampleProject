using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Primitive.Events;
using Domain.Primitive.Models;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.Test.Orders.Entities;

public class LineItemTests
{
    [Fact]
    public void CreateLineItemWith3Parameters_Should_Succeed()
    {
        // Arrange
        var orderId = OrderId.CreateUnique();
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);

        // Act
        var lineItem = LineItem.Create(orderId, productId, price);

        // Assert
        lineItem.Should().BeOfType<LineItem>();
        lineItem.Id.Should().BeOfType<LineItemId>();
        lineItem.Id.Value.GetType().Should().Be(typeof(Guid));
        lineItem.OrderId.Should().Be(orderId);
        lineItem.Price.Should().Be(price);
        lineItem.Should().BeAssignableTo<Entity<LineItemId>>();
        lineItem.DomainEvents.Should().HaveCount(0);
        lineItem.DomainEvents.Should().BeAssignableTo<IList<IDomainEvent>>();
    }
    [Fact]
    public void CreateLineItemWith4Parameters_Should_Succeed()
    {
        // Arrange
        var guidLineItemId = Guid.NewGuid();
        var lineItemId = LineItemId.Create(guidLineItemId);
        var orderId = OrderId.CreateUnique();
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);

        // Act
        var lineItem = LineItem.Create(lineItemId, orderId, productId, price);

        // Assert
        lineItem.Id.Should().BeSameAs(lineItemId);
        lineItem.Should().BeOfType<LineItem>();
        lineItem.Id.Should().BeOfType<LineItemId>();
        lineItem.Id.Value.GetType().Should().Be(typeof(Guid));
        lineItem.OrderId.Should().Be(orderId);
        lineItem.Price.Should().Be(price);
        lineItem.Should().BeAssignableTo<Entity<LineItemId>>();
        lineItem.DomainEvents.Should().HaveCount(0);
        lineItem.DomainEvents.Should().BeAssignableTo<IList<IDomainEvent>>();
    }
}