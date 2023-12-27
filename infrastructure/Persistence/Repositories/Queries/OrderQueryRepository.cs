using Domain.IRepositories.Queries;
using Domain.Orders;
using Domain.Orders.ValueObjects;
using infrastructure.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class OrderQueryRepository
    : QueryRepository<Order, OrderId>,
        IOrderQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;

    public OrderQueryRepository(
        ApplicationDbContext context, 
        IMemoryCache memoryCache, 
        IDistributedCache distributedCache) 
        : base(context, memoryCache, distributedCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
    }

    public async Task<Order?> GetByIdWithLineItemsAsNoTrackAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsNoTrackAsync())
            .Include(o => o.LineItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdWithLineItemsAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(o => o.LineItems)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order?>> GetAllWithLineItemsAsNoTrackAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsNoTrackAsync())
            .Include(o => o.LineItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order?>> GetAllWithLineItemsAsync(CancellationToken cancellationToken = default)
    {
        return await(await GetQueryableAsync())
            .Include(o => o.LineItems)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdWithLineItemsMemoryCacheAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        var key = $"Order-{id}";

        var order = await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                return GetByIdWithLineItemsAsNoTrackAsync(id, cancellationToken);
            });

        return order;
    }

    public async Task<Order?> GetByIdWithLineItemsRedisCacheAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        var key = $"Order-{id}";

        var orderCached = await _distributedCache.GetStringAsync(key, cancellationToken);

        Order? order;
        if (string.IsNullOrEmpty(orderCached))
        {
            order = await GetByIdWithLineItemsAsNoTrackAsync(id, cancellationToken);

            if (order is null) return order;

            await _distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(order),
                cancellationToken);

            return order;
        }

        order = JsonConvert.DeserializeObject<Order>(
            orderCached,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });

        return order;
    }
}