using Application.Customers.Queries.Share;
using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Queries.Get;

internal sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Queries.Customer.GetByIdFromMemoryCacheAsync(CustomerId.Create(request.Id), cancellationToken);

        if (customer is null) throw new CustomerNotFoundException(request.Id);

        var response = new CustomerResponse(customer.Id.Value, customer.Name, customer.Email);

        return response;
    }
}
