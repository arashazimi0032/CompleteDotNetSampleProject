using MediatR;

namespace Application.Customers.Commands.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest;
