namespace Domain.IRepositories;

public interface IUnitOfWork
{
    IOrderRepository Order { get; }
    
    ICustomerRepository Customer { get; }
    
    IProductRepository Product { get; }

    ILineItemRepository LineItem { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}