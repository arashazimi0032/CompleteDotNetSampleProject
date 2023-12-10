using Domain.Orders;

namespace Application.Orders.Queries.Share;

public record OrderResponse(Guid Id, Guid CustomerId, IReadOnlySet<LineItem> LineItems);
