using Domain.Customers;
using Domain.Customers.ValueObjects;
using Domain.IRepositories.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class CustomerQueryRepository : QueryRepository<Customer, CustomerId>, ICustomerQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public CustomerQueryRepository(ApplicationDbContext context, IMemoryCache memoryCache) 
        : base(context, memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }
}