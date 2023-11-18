using Application.Orders.Queries.Share;
using Domain.IRepositories;
using MediatR;

namespace Application.Orders.Queries.GetAll;

internal sealed class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await  _unitOfWork.Order.GetAllWithLineItemsAsync(cancellationToken);

        var response = orders
            .Select(o => new OrderResponse(o.Id, o.CustomerId, o.LineItems))
            .ToList();
        return response;
    }
}