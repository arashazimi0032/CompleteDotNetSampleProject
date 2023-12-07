using Domain.Customers;
using Domain.Primitive;
using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public class Order : Entity
{
    private Order()
    {
    }

    public OrderId Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    
    private readonly HashSet<LineItem> _lineItems = new();
    public IReadOnlyList<LineItem> LineItems => _lineItems.ToList();

    public static Order Create(CustomerId customerId)
    {
        var order = new Order
        {
            Id = new OrderId(Guid.NewGuid()), 
            CustomerId = customerId,
        };

        return order;
    }

    public void Update(IEnumerable<LineItem> lineItems)
    {
        _lineItems.Clear();

        foreach (var lineItem in lineItems)
        {
            Add(lineItem.ProductId, lineItem.Price);
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
}