using Application.Customers.Commands.Create;
using Application.Customers.Commands.Delete;
using Application.Customers.Commands.Update;
using Application.Customers.Queries.Get;
using Application.Customers.Queries.GetAll;
using Carter;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.EndPoints;

public class Customers : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("customers", async (CreateCustomerCommand command, ISender sender) =>
        {
            await sender.Send(command);
            
            return Results.Ok();
        }).RequireAuthorization();

        app.MapDelete("customers/{id:Guid}", async (Guid id, ISender sender) =>
        {
            try
            {
                var command = new DeleteCustomerCommand(id);

                await sender.Send(command);

                return Results.NoContent();
            }
            catch (CustomerNotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
        }).RequireAuthorization();

        app.MapPut("customers/{id:Guid}", async (Guid id, [FromBody] UpdateCustomerRequest request, ISender sender) =>
        {
            try
            {
                var command = new UpdateCustomerCommand(id, request.Name, request.Email);

                await sender.Send(command);

                return Results.NoContent();
            }
            catch (CustomerNotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
        }).RequireAuthorization();

        app.MapGet("customers/{id:Guid}", async (Guid id, ISender sender) =>
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

        app.MapGet("customers", async (ISender sender) =>
        {
            var query = new GetAllCustomersQuery();

            var response = await sender.Send(query);

            return Results.Ok(response);
        }).RequireAuthorization();
    }
}