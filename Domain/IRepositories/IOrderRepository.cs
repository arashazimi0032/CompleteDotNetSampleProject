using Domain.Orders;

namespace Domain.IRepositories;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<Order?> GetByIdWithLineItemsAsync(OrderId id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order?>> GetAllWithLineItemsAsync(CancellationToken cancellationToken = default);
}
