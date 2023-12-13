using Domain.Orders;
using Domain.Orders.ValueObjects;

namespace Domain.IRepositories.Commands;

public interface IOrderCommandRepository : ICommandRepository<Order, OrderId>
{
}
