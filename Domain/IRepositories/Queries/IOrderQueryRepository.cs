using Domain.IRepositories.Queries.Caches;
using Domain.Orders;
using Domain.Orders.ValueObjects;

namespace Domain.IRepositories.Queries;

public interface IOrderQueryRepository
    : IQueryRepository<Order, OrderId>,
        ICacheRepository<Order, OrderId>
{
    Task<Order?> GetByIdWithLineItemsAsync(OrderId id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order?>> GetAllWithLineItemsAsync(CancellationToken cancellationToken = default);
}
