using Domain.Customers;
using Domain.IRepositories;
using MediatR;

namespace Application.Customers.Commands.Create;

internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer()
        {
            Id = Guid.NewGuid(), 
            Name = request.Name,
            Email = request.Email
        };

        await _unitOfWork.Customer.AddAsync(customer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}