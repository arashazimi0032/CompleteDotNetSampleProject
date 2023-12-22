using Domain.IRepositories.Queries;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class LineItemQueryRepository : QueryRepository<LineItem, LineItemId>, ILineItemQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;

    public LineItemQueryRepository(
        ApplicationDbContext context,
        IMemoryCache memoryCache, 
        IDistributedCache distributedCache) 
        : base(context, memoryCache, distributedCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
    }
}