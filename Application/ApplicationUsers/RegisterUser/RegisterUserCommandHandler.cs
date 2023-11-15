using System.Text;
using Application.Abstractions.Email;
using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.Customers;
using Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Application.ApplicationUsers.RegisterUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _emailService = emailService;
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

        // send confirmation email
        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
        var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

        string url = $"{_configuration["AppUrl"]}/api/authentication/confirmemail?userid={applicationUser.Id}&token={validEmailToken}";

        await _emailService.SendEmailAsync(request.Email,
            "Email Confirmation",
            $"<h1>Welcome to Complete .Net Sample Project</h1>" + 
            $"<p>Please confirm your email by <a href='{url}'>Clicking here.</a></p>");

        return new UserResponse()
        {
            Message = "User Created Successfully!",
            IsSuccess = true
        };
    }
}