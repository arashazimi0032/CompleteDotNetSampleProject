using Domain.Orders.ValueObjects;
using Domain.Primitive.Models;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.Orders.Entities;

public class LineItem : Entity<LineItemId>
{
    private LineItem()
    {
        
    }
    private LineItem(LineItemId id) : base(id)
    {

    }
    private LineItem(LineItemId id, OrderId orderId, ProductId productId, Money price) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        Price = price;
    }
    public static LineItem Create(OrderId orderId, ProductId productId, Money price)
    {
        return new LineItem(LineItemId.CreateUnique(), orderId, productId, price);
    }
    public OrderId OrderId { get; private set; }
    public ProductId ProductId { get; private set; }
    public Money Price { get; private set; }
}