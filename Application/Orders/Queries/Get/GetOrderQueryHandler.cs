using Application.Orders.Queries.Share;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
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
        var response = await (await _unitOfWork.Queries.Order.GetQueryableAsync())
            .Where(o => o.Id == OrderId.Create(request.OrderId))
            .Select(o => new OrderResponse(o.Id.Value, o.CustomerId!.Value, o.LineItems))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        return response;
    }
}