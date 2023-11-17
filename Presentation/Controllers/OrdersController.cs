using System.Security.Claims;
using Application.Orders.Commands.Create;
using Application.Orders.Commands.Delete;
using Domain.ApplicationUsers;
using Domain.IRepositories;
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
        public async Task<IActionResult> Create([FromBody]CreateOrderRequest request, 
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser is null)
            {
                return NotFound("Current User Not Found. please login first!");
            }

            var command = new CreateOrderCommand(currentUser.CustomerId, request.ProductId);

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties are not valid!");
            }

            var deleteOrderCommand = new DeleteOrderCommand(id);

            await _mediator.Send(deleteOrderCommand, cancellationToken);

            return Ok();
        }
    }
}
