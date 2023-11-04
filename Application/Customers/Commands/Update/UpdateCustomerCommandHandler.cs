using Domain.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.Customers.Commands.Update;

internal sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customer.GetByIdAsync(request.Id, cancellationToken);
        
        if (customer is null)
        {
            throw new CustomerNotFoundException(request.Id);
        }

        customer.Email = request.Email;
        customer.Name = request.Name;

        _unitOfWork.Customer.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}