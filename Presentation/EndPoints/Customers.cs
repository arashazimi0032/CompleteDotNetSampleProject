using Application.Customers.Commands.Create;
using Carter;
using MediatR;

namespace Presentation.EndPoints;

public class Customers : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("customer", async (CreateCustomerCommand command, ISender sender) =>
        {
            await sender.Send(command);
            
            return Results.Ok();
        });
    }
}