using Domain.IRepositories.Commands;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;

namespace infrastructure.Persistence.Repositories.Commands;

public sealed class LineItemCommandRepository : CommandRepository<LineItem, LineItemId>, ILineItemCommandRepository
{
    private readonly ApplicationDbContext _context;

    public LineItemCommandRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}