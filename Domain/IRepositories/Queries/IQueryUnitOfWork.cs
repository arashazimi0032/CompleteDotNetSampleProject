using Microsoft.EntityFrameworkCore;

namespace Domain.IRepositories.Queries;

public interface IQueryUnitOfWork
{
    IOrderQueryRepository Order { get; }

    ICustomerQueryRepository Customer { get; }

    IProductQueryRepository Product { get; }

    ILineItemQueryRepository LineItem { get; }
}