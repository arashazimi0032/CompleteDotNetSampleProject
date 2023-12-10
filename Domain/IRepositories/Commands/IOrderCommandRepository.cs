using Domain.Orders;

namespace Domain.IRepositories.Commands;

public interface IOrderCommandRepository : ICommandRepository<Order, OrderId>
{
}
