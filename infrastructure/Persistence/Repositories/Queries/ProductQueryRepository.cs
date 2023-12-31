﻿using Domain.IRepositories.Queries;
using Domain.Products;
using Domain.Products.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class ProductQueryRepository : QueryRepository<Product, ProductId>, IProductQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;

    public ProductQueryRepository(
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