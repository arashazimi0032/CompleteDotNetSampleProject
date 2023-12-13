using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;

namespace Domain.IRepositories.Commands;

public interface ILineItemCommandRepository : ICommandRepository<LineItem, LineItemId>
{
}