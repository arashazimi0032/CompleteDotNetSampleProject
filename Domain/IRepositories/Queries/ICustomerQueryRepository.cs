using Domain.Customers;

namespace Domain.IRepositories.Queries;

public interface ICustomerQueryRepository : IQueryRepository<Customer, CustomerId>
{
}