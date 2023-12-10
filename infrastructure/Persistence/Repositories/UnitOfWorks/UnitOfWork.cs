using Domain.IRepositories.Commands;
using Domain.IRepositories.Queries;
using Domain.IRepositories.UnitOfWorks;
using infrastructure.Persistence.Repositories.Commands;
using infrastructure.Persistence.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence.Repositories.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICommandUnitOfWork Commands { get; private set; }
    public IQueryUnitOfWork Queries { get; private set; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Commands = new CommandUnitOfWork(_context);
        Queries = new QueryUnitOfWork(_context);
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