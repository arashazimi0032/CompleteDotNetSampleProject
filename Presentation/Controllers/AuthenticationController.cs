using Application.ApplicationUsers.ConfirmEmail;
using Application.ApplicationUsers.DeleteUser;
using Application.ApplicationUsers.ForgetPassword;
using Application.ApplicationUsers.GetAllUsers;
using Application.ApplicationUsers.GetUser;
using Application.ApplicationUsers.LoginUser;
using Application.ApplicationUsers.RegisterUser;
using Application.ApplicationUsers.ResetPassword;
using Application.ApplicationUsers.UpdateUser;
using Domain.Exceptions;
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

        try
        {
            var response = await _mediator.Send(userCommand, cancellationToken);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var userResponse = await _mediator.Send(command, cancellationToken);

            if (!userResponse.IsSuccess)
            {
                return BadRequest(userResponse);
            }

            return Ok(userResponse);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var confirmEmailCommand = new ConfirmEmailCommand(userId, token);

            var response = await _mediator.Send(confirmEmailCommand, cancellationToken);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("ForgetPassword")]
    public async Task<IActionResult> ForgetPassword(string email, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
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
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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

        try
        {
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
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }
        try
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetUser/{id}")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }
        try
        {
            var query = new GetUserQuery(id);

            var response = await _mediator.Send(query, cancellationToken);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }
        try
        {
            var query = new GetAllUsersQuery();

            var userQueryResponse = await _mediator.Send(query, cancellationToken);

            if (!userQueryResponse.IsSuccess)
            {
                return BadRequest(userQueryResponse);
            }

            return Ok(userQueryResponse);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UpdateUserDetails/{id}")]
    public async Task<IActionResult> UpdateUserDetails(
        Guid id, 
        [FromBody]UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return BadRequest("Some properties are not valid!");

        try
        {
            var command = new UpdateUserCommand(id, request);

            var userResponse = await _mediator.Send(command, cancellationToken);

            if (!userResponse.IsSuccess) return BadRequest(userResponse);

            return Ok(userResponse);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UpdateUserRole/{id}")]
    public async Task<IActionResult> UpdateUserRole(
        Guid id,
        [FromBody] UpdateUserRoleRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return BadRequest("Some properties are not valid!");

        try
        {
            var command = new UpdateUserRoleCommand(id, request);

            var userRoleResponse = await _mediator.Send(command,cancellationToken);

            if (!userRoleResponse.IsSuccess) return BadRequest(userRoleResponse);

            return Ok(userRoleResponse);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
