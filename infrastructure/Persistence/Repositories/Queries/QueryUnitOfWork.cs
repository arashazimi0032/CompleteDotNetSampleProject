using Domain.IRepositories.Queries;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class QueryUnitOfWork : IQueryUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    public IOrderQueryRepository Order { get; private set; }
    public ICustomerQueryRepository Customer { get; private set; }
    public IProductQueryRepository Product { get; private set; }
    public ILineItemQueryRepository LineItem { get; private set; }

    public QueryUnitOfWork(
        ApplicationDbContext context,
        IMemoryCache memoryCache, 
        IDistributedCache distributedCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        Order = new OrderQueryRepository(_context, _memoryCache, distributedCache);
        Customer = new CustomerQueryRepository(_context, _memoryCache, distributedCache);
        Product = new ProductQueryRepository(_context, _memoryCache, distributedCache);
        LineItem = new LineItemQueryRepository(_context, _memoryCache, distributedCache);
    }
}