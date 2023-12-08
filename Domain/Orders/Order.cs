using Domain.Customers;
using Domain.Primitive;
using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public class Order : Entity<OrderId>
{
    private Order(OrderId id, CustomerId customerId) : base(id)
    {
        CustomerId = customerId;
    }
    public CustomerId CustomerId { get; private set; }
    
    private readonly HashSet<LineItem> _lineItems = new();
    public IReadOnlyList<LineItem> LineItems => _lineItems.ToList();

    public static Order Create(CustomerId customerId)
    {
        return new Order(OrderId.CreateUnique(), customerId);
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