using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Commands.Update;

internal sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Order.GetByIdWithLineItemsAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        foreach (var tuple in command.Request.LineItemId.Zip(
                     command.Request.ProductId, 
                     (lineItemId, productId) => (lineItemId, productId)))
        {
            var lineItem = order.LineItems.SingleOrDefault(li => li.Id == tuple.lineItemId);
            if (lineItem is null)
            {
                throw new LineItemNotFoundException(tuple.lineItemId);
            }

            lineItem.ProductId = tuple.productId;
        }

        _unitOfWork.Order.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}