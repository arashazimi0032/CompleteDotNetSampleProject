using Domain.Customers;
using Domain.IRepositories;

namespace infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : Repository<Customer, CustomerId>, ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}