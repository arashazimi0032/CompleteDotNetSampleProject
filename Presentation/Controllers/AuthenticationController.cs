using Application.ApplicationUsers.ConfirmEmail;
using Application.ApplicationUsers.DeleteUser;
using Application.ApplicationUsers.ForgetPassword;
using Application.ApplicationUsers.LoginUser;
using Application.ApplicationUsers.RegisterUser;
using Application.ApplicationUsers.ResetPassword;
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

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        var confirmEmailCommand = new ConfirmEmailCommand(userId, token);

        var response = await _mediator.Send(confirmEmailCommand, cancellationToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("ForgetPassword")]
    public async Task<IActionResult> ForgetPassword(string email, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        if (string.IsNullOrEmpty(email))
        {
            return NotFound("No Email entered!");
        }

        var command = new ForgetPasswordCommand
        {
            Email = email
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("ResetPasswordRequest")]
    public async Task<IActionResult> ResetPasswordRequest(
        string email,
        string token)
    {
        return Ok(new { email, token });
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(
        string email, 
        string token, 
        [FromBody]ResetPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        var command = new ResetPasswordCommand
        {
            ConfirmPassword = request.ConfirmPassword,
            Password = request.Password,
            Email = email,
            Token = token
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
