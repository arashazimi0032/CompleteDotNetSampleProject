using Domain.IRepositories.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class QueryUnitOfWork : IQueryUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    public IOrderQueryRepository Order { get; private set; }
    public ICustomerQueryRepository Customer { get; private set; }
    public IProductQueryRepository Product { get; private set; }
    public ILineItemQueryRepository LineItem { get; private set; }

    public QueryUnitOfWork(ApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        Order = new OrderQueryRepository(_context, _memoryCache);
        Customer = new CustomerQueryRepository(_context, _memoryCache);
        Product = new ProductQueryRepository(_context, _memoryCache);
        LineItem = new LineItemQueryRepository(_context, _memoryCache);
    }
}