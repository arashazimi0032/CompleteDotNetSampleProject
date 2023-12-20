using Domain.IRepositories.Queries;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class LineItemQueryRepository : QueryRepository<LineItem, LineItemId>, ILineItemQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public LineItemQueryRepository(ApplicationDbContext context, IMemoryCache memoryCache) 
        : base(context, memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }
}