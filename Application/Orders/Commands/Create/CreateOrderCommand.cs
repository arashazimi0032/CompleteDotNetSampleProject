using Domain.Customers;
using Domain.Products;
using MediatR;

namespace Application.Orders.Commands.Create;

public record CreateOrderCommand(Guid CustomerId, List<Guid> ProductId) : IRequest;
