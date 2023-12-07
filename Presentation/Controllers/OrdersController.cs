using System.Security.Claims;
using Application.Orders.Commands.Create;
using Application.Orders.Commands.Delete;
using Application.Orders.Commands.Update;
using Application.Orders.Queries.Get;
using Application.Orders.Queries.GetAll;
using Domain.ApplicationUsers;
using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderRequest request, 
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser is null)
                {
                    return NotFound("Current User Not Found. please login first!");
                }

                var command = new CreateOrderCommand(currentUser.CustomerId!.Value, request.ProductId);

                await _mediator.Send(command, cancellationToken);

                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            try
            {
                var deleteOrderCommand = new DeleteOrderCommand(id);

                await _mediator.Send(deleteOrderCommand, cancellationToken);

                return Ok();
            }
            catch (OrderNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            Guid id,
            UpdateOrderRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            try
            {
                var updateOrderCommand = new UpdateOrderCommand(id, request);

                await _mediator.Send(updateOrderCommand, cancellationToken);

                return Ok();
            }
            catch (OrderNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            try
            {
                var getOrderQuery = new GetOrderQuery(id);

                var response = await _mediator.Send(getOrderQuery, cancellationToken);
            
                return Ok(response);
            }
            catch (OrderNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            try
            {
                var getAllOrdersQuery = new GetAllOrdersQuery();
                var response = await _mediator.Send(getAllOrdersQuery, cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
