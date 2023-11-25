using Domain.Customers;
using Domain.IRepositories;

namespace infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid? id, CancellationToken cancellationToken = default)
    {
        if (id is null)
        {
            return null;
        }
        return await _context.Customers.FindAsync(
            new object?[] { id, cancellationToken }, 
            cancellationToken: cancellationToken);
    }
}