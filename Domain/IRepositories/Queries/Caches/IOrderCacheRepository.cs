using Domain.Orders;
using Domain.Orders.ValueObjects;

namespace Domain.IRepositories.Queries.Caches;

public interface IOrderCacheRepository
{
    Task<Order?> GetByIdWithLineItemsMemoryCacheAsNoTrackAsync(OrderId id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithLineItemsMemoryCacheAsync(OrderId id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithLineItemsRedisCacheAsNoTrackAsync(OrderId id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithLineItemsRedisCacheAsync(OrderId id, CancellationToken cancellationToken = default);
}