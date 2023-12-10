using Domain.Orders;

namespace Domain.IRepositories.Queries;

public interface IOrderQueryRepository : IQueryRepository<Order, OrderId>
{
    Task<Order?> GetByIdWithLineItemsAsync(OrderId id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order?>> GetAllWithLineItemsAsync(CancellationToken cancellationToken = default);
}
