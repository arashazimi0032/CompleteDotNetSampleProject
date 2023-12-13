using Domain.Customers;
using Domain.Customers.ValueObjects;

namespace Domain.IRepositories.Queries;

public interface ICustomerQueryRepository : IQueryRepository<Customer, CustomerId>
{
}