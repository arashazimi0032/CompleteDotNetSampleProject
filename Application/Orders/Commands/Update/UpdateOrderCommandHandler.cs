using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Orders.Commands.Update;

internal sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _distributedCache;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
    {
        _unitOfWork = unitOfWork;
        _distributedCache = distributedCache;
    }

    public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var key = $"Order-{OrderId.Create(command.OrderId)}";

        var order = await _unitOfWork.Queries.Order.GetByIdWithLineItemsAsync(OrderId.Create(command.OrderId), cancellationToken);
        //var order = await _unitOfWork.Queries.Order.GetByIdWithLineItemsMemoryCacheAsync(OrderId.Create(command.OrderId), cancellationToken);

        var lineItems = new List<LineItem>();

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        foreach (var productId in command.Request.ProductId)
        {
            var product = await _unitOfWork.Queries.Product.GetByIdAsync(ProductId.Create(productId), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(productId);

            var price = new Money(product.Price.Currency, product.Price.Amount);
            var lineItem = LineItem.Create(OrderId.Create(command.OrderId), ProductId.Create(productId), price);
            lineItems.Add(lineItem);
        }

        order.Update(lineItems);

        _unitOfWork.Commands.Order.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //// TODO: redis update must convert to a separate service
        //await _distributedCache.SetStringAsync(
        //    key,
        //    JsonConvert.SerializeObject(order),
        //    cancellationToken);
    }
}