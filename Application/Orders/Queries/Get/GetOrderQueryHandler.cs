using Application.Orders.Queries.Share;
using Domain.Exceptions;
using Domain.IRepositories;
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
        //var order = await _unitOfWork.Order.GetByIdAsync(request.OrderId, cancellationToken);
        
        //if (order is null)
        //{
        //    throw new OrderNotFoundException(request.OrderId);
        //}

        //var response = new OrderResponse(order.Id, order.CustomerId, order.LineItems);

        //return response;

        var response = await (await _unitOfWork.Order.GetQueryableAsync())
            .Where(o => o.Id == request.OrderId)
            .Select(o => new OrderResponse(o.Id, o.CustomerId, o.LineItems))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        return response;
    }
}