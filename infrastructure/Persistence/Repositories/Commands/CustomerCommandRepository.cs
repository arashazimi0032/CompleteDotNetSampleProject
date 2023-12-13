using Domain.Customers;
using Domain.Customers.ValueObjects;
using Domain.IRepositories.Commands;

namespace infrastructure.Persistence.Repositories.Commands;

public sealed class CustomerCommandRepository : CommandRepository<Customer, CustomerId>, ICustomerCommandRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerCommandRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}