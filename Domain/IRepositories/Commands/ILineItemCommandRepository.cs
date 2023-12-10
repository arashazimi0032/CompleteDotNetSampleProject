using Domain.Orders;

namespace Domain.IRepositories.Commands;

public interface ILineItemCommandRepository : ICommandRepository<LineItem, LineItemId>
{
}