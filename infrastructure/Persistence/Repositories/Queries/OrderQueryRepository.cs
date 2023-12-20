using Domain.IRepositories.Queries;
using Domain.Orders;
using Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class OrderQueryRepository : QueryRepository<Order, OrderId>, IOrderQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public OrderQueryRepository(ApplicationDbContext context, IMemoryCache memoryCache) 
        : base(context, memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Order?> GetByIdWithLineItemsAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(o => o.LineItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order?>> GetAllWithLineItemsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(o => o.LineItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}