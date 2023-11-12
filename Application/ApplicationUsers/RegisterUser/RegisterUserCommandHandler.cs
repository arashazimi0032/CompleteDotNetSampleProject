using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.Customers;
using Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUsers.RegisterUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new NullReferenceException("Register Model Is Null!");
        }

        if (request.Password != request.ConfirmPassword)
        {
            return new UserResponse()
            {
                Message = "Confirm password dos not match the password!",
                IsSuccess = false
            };
        }

        var customerId = Guid.NewGuid();

        var applicationUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            CustomerId = customerId
        };

        var customer = new Customer()
        {
            Id = customerId,
            Name = request.UserName,
            Email = request.Email
        };

        await _unitOfWork.Customer.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO : the code for creating customer and add it to database is repeated in CreateCustomerCommandHandler. It should be convert to a service

        var result = await _userManager.CreateAsync(applicationUser, request.Password);

        if (!result.Succeeded)
        {
            return new UserResponse()
            {
                Message = "User did not create!",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new UserResponse()
        {
            Message = "User Created Successfully!",
            IsSuccess = true
        };
    }
}