using Domain.Customers.ValueObjects;
using Domain.Orders;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Primitive.Events;
using Domain.Primitive.Models;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.Test.Orders;

public class OrderTests
{
    [Fact]
    public void CreateOrderWith1Parameter_Should_Succeed()
    {
        // Arrange 
        var customerId = CustomerId.CreateUnique();

        // Act
        var order = Order.Create(customerId);

        // Assert
        order.CustomerId.Should().Be(customerId);
        order.Id.Should().BeOfType<OrderId>();
        order.Id.Value.GetType().Should().Be(typeof(Guid));
        order.LineItems.Should().HaveCount(0);
        order.LineItems.Should().BeOfType<HashSet<LineItem>>();
        order.Should().BeAssignableTo<Entity<OrderId>>();
        order.DomainEvents.Should().HaveCount(1);
        order.DomainEvents.Should().BeAssignableTo<IList<IDomainEvent>>();
    }

    [Fact]
    public void CreateOrderWith2Parameter_Should_Succeed()
    {
        // Arrange 
        var orderId = OrderId.CreateUnique();
        var customerId = CustomerId.CreateUnique();

        // Act
        var order = Order.Create(orderId, customerId);

        // Assert
        order.CustomerId.Should().Be(customerId);
        order.Id.Should().BeSameAs(orderId);
        order.Id.Should().BeOfType<OrderId>();
        order.Id.Value.GetType().Should().Be(typeof(Guid));
        order.LineItems.Should().HaveCount(0);
        order.LineItems.Should().BeOfType<HashSet<LineItem>>();
        order.Should().BeAssignableTo<Entity<OrderId>>();
        order.DomainEvents.Should().HaveCount(1);
        order.DomainEvents.Should().BeAssignableTo<IList<IDomainEvent>>();
    }

    [Fact]
    public void AddLineItemToOrderWith2Parameter_Should_Succeed()
    {
        // Arrange 
        var orderId = OrderId.CreateUnique();
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(orderId, customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);

        // Act
        order.Add(productId, price);

        // Assert
        order.LineItems.Should().HaveCount(1);
        order.LineItems.First().Should().BeOfType<LineItem>();
        order.LineItems.First().ProductId.Should().Be(productId);
        order.LineItems.First().Price.Should().Be(price);
        order.LineItems.First().OrderId.Should().Be(orderId);
    }

    [Fact]
    public void AddLineItemToOrderWith3Parameter_Should_Succeed()
    {
        // Arrange 
        var orderId = OrderId.CreateUnique();
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(orderId, customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItemId = LineItemId.CreateUnique();

        // Act
        order.Add(lineItemId, productId, price);

        // Assert
        order.LineItems.Should().HaveCount(1);
        order.LineItems.First().Should().BeOfType<LineItem>();
        order.LineItems.First().Id.Should().Be(lineItemId);
        order.LineItems.First().ProductId.Should().Be(productId);
        order.LineItems.First().Price.Should().Be(price);
        order.LineItems.First().OrderId.Should().Be(orderId);
    }

    [Fact]
    public void UpdateOrder_Should_Succeed()
    {
        // Arrange 
        var orderId = OrderId.CreateUnique();
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(orderId, customerId);
        var productId1 = ProductId.CreateUnique();
        var price1 = new Money("USD", 100m);
        var lineItemId1 = LineItemId.CreateUnique();
        order.Add(lineItemId1, productId1, price1);

        var productId2 = ProductId.CreateUnique();
        var price2 = new Money("USD", 100m);
        var lineItemId2 = LineItemId.CreateUnique();
        List<LineItem> lineItemsNew = new() { LineItem.Create(lineItemId2, orderId, productId2, price2) };

        // Act
        order.Update(lineItemsNew);

        // Assert
        order.LineItems.Should().HaveCount(1);
        order.LineItems.Should().BeEquivalentTo(lineItemsNew);
    }
}