using Microsoft.EntityFrameworkCore;
using Domain.IRepositories.Queries;
using Domain.Primitive.Models;

namespace infrastructure.Persistence.Repositories.Queries;

public class QueryRepository<T, TId> : IQueryRepository<T, TId>
    where T : Entity<TId>
    where TId : ValueObject
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
        return dbSet.AsNoTracking();
    }

    public async Task<IQueryable<T>> GetQueryableAsync()
    {
        return dbSet.AsNoTracking();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await dbSet.FindAsync(
                new object?[] { id, cancellationToken },
                cancellationToken: cancellationToken);

        _context.Entry(entity!).State = EntityState.Detached;
        return entity;
    }
}