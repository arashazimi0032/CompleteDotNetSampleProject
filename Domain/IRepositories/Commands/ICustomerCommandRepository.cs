using Domain.Customers;

namespace Domain.IRepositories.Commands;

public interface ICustomerCommandRepository : ICommandRepository<Customer, CustomerId>
{
}