using Domain.IRepositories.Commands;
using Domain.IRepositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace Domain.IRepositories.UnitOfWorks;

public interface IUnitOfWork
{
    ICommandUnitOfWork Commands { get; }
    IQueryUnitOfWork Queries { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    DbContext GetDbContext();
}