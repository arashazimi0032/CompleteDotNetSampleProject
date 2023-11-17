using Domain.ApplicationUsers;
using Domain.Customers;
using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Orders;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Orders.Commands.Create;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        var lineItems = new HashSet<LineItem>();

        foreach (var productId in request.ProductId)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(productId, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(productId);
            }

            var lineItem = new LineItem
            {
                Id = Guid.NewGuid(), 
                OrderId = orderId, 
                ProductId = productId, 
                Price = product.Price,
            };

            lineItems.Add(lineItem);
        }

        var order = new Order
        {
            Id = orderId,
            CustomerId = request.CustomerId,
            LineItems = lineItems
        };

        await _unitOfWork.Order.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}