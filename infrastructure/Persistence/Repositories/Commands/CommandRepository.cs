using Domain.IRepositories.Commands;
using Microsoft.EntityFrameworkCore;
using Domain.Primitive;

namespace infrastructure.Persistence.Repositories.Commands;

public class CommandRepository<T, TId> : ICommandRepository<T, TId>
    where T : Entity<TId>
    where TId : notnull
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;

    public CommandRepository(ApplicationDbContext context)
    {
        _context = context;
        dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
}