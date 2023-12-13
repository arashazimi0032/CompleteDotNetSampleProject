using Domain.Customers;
using Domain.Customers.ValueObjects;

namespace Domain.IRepositories.Commands;

public interface ICustomerCommandRepository : ICommandRepository<Customer, CustomerId>
{
}