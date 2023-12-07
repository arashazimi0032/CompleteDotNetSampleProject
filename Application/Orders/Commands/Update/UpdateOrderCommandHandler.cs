using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Orders;
using Domain.Products;
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

        var lineItems = new List<LineItem>();

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        foreach (var productId in command.Request.ProductId)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(productId, cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(productId);

            var lineItem = LineItem.Create(new OrderId(command.OrderId), new ProductId(productId), product.Price);
            lineItems.Add(lineItem);
        }

        order.Update(lineItems);

        _unitOfWork.Order.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}