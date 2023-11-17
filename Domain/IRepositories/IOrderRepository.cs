using Domain.Orders;

namespace Domain.IRepositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithLineItemsAsync(Guid id, CancellationToken cancellationToken = default);
}
