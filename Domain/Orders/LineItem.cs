using Domain.Primitive;
using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public class LineItem : Entity
{
    private LineItem()
    {
    }
    public static LineItem Create(OrderId orderId, ProductId productId, Money price)
    {
        return new LineItem
        {
            Id = new LineItemId(Guid.NewGuid()),
            OrderId = orderId,
            ProductId = productId,
            Price = price
        };
    }

    public LineItemId Id { get; private set; }
    public OrderId OrderId { get; private set; }
    public ProductId ProductId { get; private set; }
    public Money Price { get; private set; }
}