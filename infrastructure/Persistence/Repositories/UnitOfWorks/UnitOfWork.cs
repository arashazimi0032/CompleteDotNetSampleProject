using Domain.IRepositories.Commands;
using Domain.IRepositories.Queries;
using Domain.IRepositories.UnitOfWorks;
using infrastructure.Persistence.Repositories.Commands;
using infrastructure.Persistence.Repositories.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    public ICommandUnitOfWork Commands { get; private set; }
    public IQueryUnitOfWork Queries { get; private set; }

    public UnitOfWork(
        ApplicationDbContext context,
        IMemoryCache memoryCache,
        IDistributedCache distributedCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        Commands = new CommandUnitOfWork(_context);
        Queries = new QueryUnitOfWork(_context, memoryCache, distributedCache);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public DbContext GetDbContext()
    {
        return _context;
    }
}