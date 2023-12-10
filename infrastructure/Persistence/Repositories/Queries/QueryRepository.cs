using Microsoft.EntityFrameworkCore;
using Domain.Primitive;
using Domain.IRepositories.Queries;

namespace infrastructure.Persistence.Repositories.Queries;

public class QueryRepository<T, TId> : IQueryRepository<T, TId>
    where T : Entity<TId>
    where TId : notnull
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;

    public QueryRepository(ApplicationDbContext context)
    {
        _context = context;
        dbSet = _context.Set<T>();
    }

    public IQueryable<T> GetQueryable()
    {
        return dbSet;
    }

    public async Task<IQueryable<T>> GetQueryableAsync()
    {
        return dbSet;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(
                new object?[] { id, cancellationToken },
                cancellationToken: cancellationToken);
    }
}