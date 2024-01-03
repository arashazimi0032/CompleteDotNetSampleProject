using Application.ApplicationUsers.RegisterUser;
using Application.ApplicationUsers.Share;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;

namespace Presentation.Test.Controllers;

public class AuthenticationControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _mediatorMock = new();
        _controller = new AuthenticationController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnOkResultWithOneUserResponse_IfSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Arash003254",
            ConfirmPassword = "@Arash003254",
            Role = "Admin"
        };

        var userResponse = new UserResponse
        {
            Message = "User Created Successfully!",
            IsSuccess = true
        };

        _mediatorMock.Setup(x => x.Send(registerUserCommand, cancellationToken)).ReturnsAsync(userResponse);

        // Act
        var controllerResponse = await _controller.Register(registerUserCommand, cancellationToken);

        // Assert
        controllerResponse.Should().BeOfType<OkObjectResult>();
        var response = controllerResponse as OkObjectResult;
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Value.Should().Be(userResponse);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequestWithOneUserResponse_IfNotSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Arash003254",
            ConfirmPassword = "@Arash003254",
            Role = "Admin",
        };

        var userResponse = new UserResponse
        {
            Message = "User Created Successfully!",
            IsSuccess = false
        };

        _mediatorMock.Setup(x => x.Send(registerUserCommand, cancellationToken)).ReturnsAsync(userResponse);

        // Act
        var controllerResponse = await _controller.Register(registerUserCommand, cancellationToken);

        // Assert
        controllerResponse.Should().BeOfType<BadRequestObjectResult>();
        var response = controllerResponse as BadRequestObjectResult;
        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        response.Value.Should().Be(userResponse);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_IfThrowAnyException()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Arash003254",
            ConfirmPassword = "@Arash003254",
            Role = "Admin",
        };

        _mediatorMock.Setup(x => x.Send(registerUserCommand, cancellationToken))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var controllerResponse = await _controller.Register(registerUserCommand, cancellationToken);

        // Assert
        controllerResponse.Should().BeOfType<BadRequestObjectResult>();
        var response = controllerResponse as BadRequestObjectResult;
        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        response.Value.Should().Be("Something went wrong");
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_IfModelStateIsNotValid()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Arash003254",
            ConfirmPassword = "@Arash003254",
            Role = "Admin",
        };

        _controller.ModelState.AddModelError("Fails", "Something went wrong");

        // Act
        var controllerResponse = await _controller.Register(registerUserCommand, cancellationToken);

        // Assert
        controllerResponse.Should().BeOfType<BadRequestObjectResult>();
        var response = controllerResponse as BadRequestObjectResult;
        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        response.Value.Should().Be("Some properties are not valid!");
    }
}