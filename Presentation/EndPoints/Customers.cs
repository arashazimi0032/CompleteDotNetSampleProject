using Application.Customers.Queries.Get;
using Application.Customers.Queries.GetAll;
using Carter;
using Domain.Exceptions;
using MediatR;

namespace Presentation.EndPoints;

public class Customers : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var customer = app.MapGroup("/api").WithTags("CustomerCommand");

        customer.MapGet("customers/{id:Guid}", async (Guid id, ISender sender) =>
        {
            try
            {
                var query = new GetCustomerQuery(id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }
            catch (CustomerNotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
        }).RequireAuthorization();

        customer.MapGet("customers", async (ISender sender) =>
        {
            var query = new GetAllCustomersQuery();

            var response = await sender.Send(query);

            return Results.Ok(response);
        }).RequireAuthorization("AdminPolicy");
    }
}