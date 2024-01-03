using Application.Abstractions.Email;
using Application.ApplicationUsers.RegisterUser;
using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using infrastructure.Persistence;
using infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Test.ApplicationUsers.RegisterUser;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<ApplicationDbContext> _context;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<ICustomerCommandRepository> _customerRepositoryMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _context = new(
                new DbContextOptions<ApplicationDbContext>(),
                new PublishDomainEventsInterceptor(new Mock<IPublisher>().Object),
                new UpdateAuditableEntitiesInterceptor());

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        var roleStore = new Mock<IRoleStore<IdentityRole>>();

        _userManagerMock = new(userStore.Object, null, null, null, null, null, null, null, null);
        _roleManagerMock = new(roleStore.Object, null, null, null, null);

        _unitOfWorkMock = new();
        _commandUnitOfWorkMock = new();
        _customerRepositoryMock = new();

        Mock<IConfiguration> configurationMock = new();
        Mock<IEmailService> mailServiceMock = new();

        _handler = new RegisterUserCommandHandler(
            _userManagerMock.Object,
            _unitOfWorkMock.Object,
            configurationMock.Object,
            mailServiceMock.Object,
            _roleManagerMock.Object);
    }

    [Fact]
    public async Task Handler_ShouldThrowNullReferenceException_WhenRequestIsNull()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = (RegisterUserCommand?)null;


        // Act
        var act = async () => await _handler.Handle(registerUserCommand, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task Handler_ShouldReturnFailureResult_WhenPasswordNotEqualToConfirmPassword()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Password1234",
            ConfirmPassword = "@Password12345",
            Role = "Admin"
        };

        // Act
        var handlerResponse = await _handler.Handle(registerUserCommand, cancellationToken);

        // Assert
        handlerResponse.Should().BeOfType<UserResponse>();
        handlerResponse.Message.Should().Be("Confirm password dos not match the password!");
        handlerResponse.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_ShouldReturnFailureResult_WhenUserManagerCreateIsNotSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Password1234",
            ConfirmPassword = "@Password1234",
            Role = "Admin"
        };

        var userResponse = new UserResponse()
        {
            Message = "User did not create!",
            IsSuccess = false,
            Errors = new []{ "Something went wrong" }
        };

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Customer).Returns(_customerRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetDbContext()).Returns(_context.Object);
        _unitOfWorkMock.Setup(x => x.GetDbContext().Database).Returns(new MockDatabaseFacade(_context.Object));
        _userManagerMock.Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(), 
                registerUserCommand.Password))
            .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError {Description = "Something went wrong" })));

        // Act
        var handlerResponse = await _handler.Handle(registerUserCommand, cancellationToken);

        // Assert
        handlerResponse.Should().BeEquivalentTo(userResponse);
    }

    [Fact]
    public async Task Handler_ShouldReturnFailureResult_WhenUserManagerAddToRoleIsNotSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Password1234",
            ConfirmPassword = "@Password1234",
            Role = "Admin"
        };

        var userResponse = new UserResponse()
        {
            Message = "Could not assign this role to user!",
            IsSuccess = false,
            Errors = new []{ "Something went wrong" }
        };

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Customer).Returns(_customerRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetDbContext()).Returns(_context.Object);
        _unitOfWorkMock.Setup(x => x.GetDbContext().Database).Returns(new MockDatabaseFacade(_context.Object));
        
        _userManagerMock.Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(), 
                registerUserCommand.Password))
            .Returns(Task.FromResult(IdentityResult.Success));
        
        _userManagerMock.Setup(x => x.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                registerUserCommand.Role))
            .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError{ Description = "Something went wrong" })));

        _roleManagerMock.Setup(x => x.RoleExistsAsync(registerUserCommand.Role))
            .Returns(Task.FromResult(true));

        // Act
        var handlerResponse = await _handler.Handle(registerUserCommand, cancellationToken);

        // Assert
        handlerResponse.Should().BeEquivalentTo(userResponse);
    }

    [Fact]
    public async Task Handler_ShouldSucceed_WhenAllIsSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var databaseFacadeMock = new MockDatabaseFacade(_context.Object);
        var registerUserCommand = new RegisterUserCommand
        {
            UserName = "UserName",
            Email = "email@gmail.com",
            Password = "@Password1234",
            ConfirmPassword = "@Password1234",
            Role = "Admin"
        };

        var userResponse = new UserResponse()
        {
            Message = "User Created Successfully!",
            IsSuccess = true
        };

        const string token = "This is a test token";

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Customer).Returns(_customerRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetDbContext()).Returns(_context.Object);
        _unitOfWorkMock.Setup(x => x.GetDbContext().Database).Returns(databaseFacadeMock);
        
        _userManagerMock.Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(), 
                registerUserCommand.Password))
            .Returns(Task.FromResult(IdentityResult.Success));
        
        _userManagerMock.Setup(x => x.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                registerUserCommand.Role))
            .Returns(Task.FromResult(IdentityResult.Success));

        _roleManagerMock.Setup(x => x.RoleExistsAsync(registerUserCommand.Role))
            .Returns(Task.FromResult(true));

        _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(token);

        // Act
        var handlerResponse = await _handler.Handle(registerUserCommand, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        handlerResponse.Should().BeEquivalentTo(userResponse);
    }
}