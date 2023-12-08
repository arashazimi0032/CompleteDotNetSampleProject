using Domain.Primitive;
using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public class LineItem : Entity<LineItemId>
{
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