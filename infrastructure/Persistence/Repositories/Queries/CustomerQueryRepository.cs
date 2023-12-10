using Domain.Customers;
using Domain.IRepositories.Queries;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class CustomerQueryRepository : QueryRepository<Customer, CustomerId>, ICustomerQueryRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerQueryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}