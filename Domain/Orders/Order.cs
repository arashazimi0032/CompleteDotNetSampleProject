using Domain.Customers.ValueObjects;
using Domain.Orders.Entities;
using Domain.Orders.Events;
using Domain.Orders.ValueObjects;
using Domain.Primitive.Models;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.Orders;

public class Order : AggregateRoot<OrderId>
{
    private Order()
    {

    }

    private Order(OrderId id, CustomerId customerId) : base(id)
    {
        CustomerId = customerId;
    }
    public CustomerId CustomerId { get; private set; }
    
    private readonly HashSet<LineItem> _lineItems = new();
    public IReadOnlySet<LineItem> LineItems => _lineItems.ToHashSet();

    public static Order Create(OrderId id, CustomerId customerId)
    {
        var order = new Order(id, customerId);

        order.Raise(new OrderCreatedDomainEvent(order));

        return order;
    }

    public static Order Create(CustomerId customerId)
    {
        var order = new Order(OrderId.CreateUnique(), customerId);
        
        order.Raise(new OrderCreatedDomainEvent(order));

        return order;
    }

    public void Update(IEnumerable<LineItem> lineItems)
    {
        _lineItems.Clear();

        foreach (var lineItem in lineItems)
        {
            Add(lineItem.Id, lineItem.ProductId, lineItem.Price);
        }
    }

    public void Add(ProductId productId, Money price)
    {
        var lineItem = LineItem.Create(
            Id, 
            productId,
            price);
        
        _lineItems.Add(lineItem);
    }

    public void Add(LineItemId lineItemId, ProductId productId, Money price)
    {
        var lineItem = LineItem.Create(
            lineItemId,
            Id, 
            productId,
            price);
        
        _lineItems.Add(lineItem);
    }
}