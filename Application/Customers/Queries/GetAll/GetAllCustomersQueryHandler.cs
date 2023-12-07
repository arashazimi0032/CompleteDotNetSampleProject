using Application.Customers.Queries.Share;
using Domain.IRepositories;
using MediatR;

namespace Application.Customers.Queries.GetAll;

internal sealed class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CustomerResponse>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.Customer.GetAllAsync(cancellationToken);

        return customers
            .Select(c => new CustomerResponse(c.Id.Value, c.Name, c.Email))
            .ToList();
    }
}
