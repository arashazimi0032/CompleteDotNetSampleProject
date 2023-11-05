using Application.Customers.Queries.Share;
using MediatR;

namespace Application.Customers.Queries.GetAll;

public record GetAllCustomersQuery() : IRequest<IEnumerable<CustomerResponse>>;
