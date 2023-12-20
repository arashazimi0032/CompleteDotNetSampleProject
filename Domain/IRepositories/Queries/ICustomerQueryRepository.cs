using Domain.Customers;
using Domain.Customers.ValueObjects;
using Domain.IRepositories.Queries.Caches;

namespace Domain.IRepositories.Queries;

public interface ICustomerQueryRepository
    : IQueryRepository<Customer, CustomerId>,
        ICacheRepository<Customer, CustomerId>
{
}