using Domain.Customers;

namespace Domain.IRepositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetCustomerByIdAsync(Guid? id, CancellationToken cancellationToken = default);
}