using Application.Orders.Queries.Share;
using MediatR;

namespace Application.Orders.Queries.GetAll;

public record GetAllOrdersQuery() : IRequest<IEnumerable<OrderResponse>>;
