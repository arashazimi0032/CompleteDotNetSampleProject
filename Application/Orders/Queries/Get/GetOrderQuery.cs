using Application.Orders.Queries.Share;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Queries.Get;

public record GetOrderQuery(Guid OrderId) : IRequest<OrderResponse>;
