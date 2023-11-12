using Application.ApplicationUsers.LoginUser;
using Application.ApplicationUsers.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterUserCommand userCommand, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!"); // status code 400
        }

        var response = await _mediator.Send(userCommand, cancellationToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        var userResponse = await _mediator.Send(command, cancellationToken);

        if (!userResponse.IsSuccess)
        {
            return BadRequest(userResponse);
        }

        return Ok(userResponse);
    }
}
