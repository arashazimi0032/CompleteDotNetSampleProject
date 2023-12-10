using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Commands.Delete;

internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Queries.Order.GetByIdAsync(OrderId.Create(request.OrderId), cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        _unitOfWork.Commands.Order.Remove(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}