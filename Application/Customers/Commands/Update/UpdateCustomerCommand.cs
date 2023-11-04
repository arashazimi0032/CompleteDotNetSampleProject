using MediatR;

namespace Application.Customers.Commands.Update;

public record UpdateCustomerCommand(Guid Id, string Name, string Email) : IRequest;
