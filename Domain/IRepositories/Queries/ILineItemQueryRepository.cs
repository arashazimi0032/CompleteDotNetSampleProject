using Domain.Orders;

namespace Domain.IRepositories.Queries;

public interface ILineItemQueryRepository : IQueryRepository<LineItem, LineItemId>
{
}