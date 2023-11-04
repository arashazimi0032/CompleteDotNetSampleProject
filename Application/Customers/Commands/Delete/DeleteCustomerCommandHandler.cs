using Domain.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.Customers.Commands.Delete;

internal sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customer.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            throw new CustomerNotFoundException(request.Id);
        }

        _unitOfWork.Customer.Remove(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}