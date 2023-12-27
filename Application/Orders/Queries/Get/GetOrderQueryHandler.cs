using Application.Orders.Queries.Share;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries.Get;

internal sealed class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Create(request.OrderId);

        var order = await _unitOfWork.Queries.Order.GetByIdWithLineItemsMemoryCacheAsync(orderId, cancellationToken);

        if (order is null) throw new OrderNotFoundException(request.OrderId);

        var response = new OrderResponse(order.Id.Value, order.CustomerId.Value, order.LineItems);

        return response;
    }
}