using System.Text;
using Application.Abstractions.Email;
using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.Customers;
using Domain.Enums;
using Domain.IRepositories.UnitOfWorks;
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
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IEmailService emailService,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _emailService = emailService;
        _roleManager = roleManager;
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

        var customer = Customer.Create(
            request.UserName,
            request.Email
        );

        var applicationUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            CustomerId = customer.Id,
        };

        await using (var transaction =
                     await _unitOfWork.GetDbContext().Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await _unitOfWork.Commands.Customer.AddAsync(customer, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var result = await _userManager.CreateAsync(applicationUser, request.Password);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new UserResponse()
                    {
                        Message = "User did not create!",
                        IsSuccess = false,
                        Errors = result.Errors.Select(e => e.Description)
                    };
                }

                IdentityResult addRoleResponse;
                if (string.IsNullOrWhiteSpace(request.Role) || !await _roleManager.RoleExistsAsync(request.Role))
                {
                    addRoleResponse = await _userManager.AddToRoleAsync(applicationUser, Role.Customer.ToString());
                }
                else
                {
                    addRoleResponse = await _userManager.AddToRoleAsync(applicationUser, request.Role);
                }

                if (!addRoleResponse.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new UserResponse
                    {
                        Message = "Could not assign this role to user!",
                        IsSuccess = false,
                        Errors = addRoleResponse.Errors.Select(e => e.Description)
                    };
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new UserResponse
                {
                    Message = e.Message,
                    IsSuccess = false
                };
            }
        }

        // send confirmation email
        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
        var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

        var url = $"{_configuration["AppUrl"]}/api/authentication/confirmEmail?userid={applicationUser.Id}&token={validEmailToken}";

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