using MediatR;

namespace Application.Customers.Commands.Create;

public record CreateCustomerCommand(string Name, string Email) : IRequest;
