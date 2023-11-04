using MediatR;

namespace Application.Customers.Queries.Get;

public record GetCustomerQuery(Guid Id) : IRequest<CustomerResponse>;
