using Domain.IRepositories.Commands;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence.Repositories.Commands;

public sealed class CommandUnitOfWork : ICommandUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IOrderCommandRepository Order { get; private set; }
    public ICustomerCommandRepository Customer { get; private set; }
    public IProductCommandRepository Product { get; private set; }
    public ILineItemCommandRepository LineItem { get; private set; }

    public CommandUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Order = new OrderCommandRepository(_context);
        Customer = new CustomerCommandRepository(_context);
        Product = new ProductCommandRepository(_context);
        LineItem = new LineItemCommandRepository(_context);
    }
}