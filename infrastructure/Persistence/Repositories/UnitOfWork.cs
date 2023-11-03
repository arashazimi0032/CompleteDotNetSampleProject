using Domain.IRepositories;

namespace infrastructure.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IOrderRepository Order { get; private set; }
    public ICustomerRepository Customer { get; private set; }
    public IProductRepository Product { get; private set; }
    public ILineItemRepository LineItem { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Order = new OrderRepository(_context);
        Customer = new CustomerRepository(_context);
        Product = new ProductRepository(_context);
        LineItem = new LineItemRepository(_context);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}