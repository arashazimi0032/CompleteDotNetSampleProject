using Application.Customers.Queries.Share;
using Domain.Customers;
using Domain.Exceptions;
using Domain.IRepositories;
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
        var queryable = await _unitOfWork.Customer.GetQueryableAsync();

        var response = await queryable
            .Where(c => c.Id == new CustomerId(request.Id))
            .Select(c => new CustomerResponse(c.Id.Value, c.Name, c.Email))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new CustomerNotFoundException(request.Id);
        }

        return response;
    }
}
