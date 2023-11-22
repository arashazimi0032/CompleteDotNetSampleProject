using System.Text;
using Application.Abstractions.Email;
using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Application.ApplicationUsers.ForgetPassword;

internal sealed class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public ForgetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<UserResponse> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new UserResponse
            {
                Message = "No user associated with this email!", 
                IsSuccess = false
            };
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(token);
        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        var url =
            $"{_configuration["AppUrl"]}/api/authentication/ResetPasswordRequest?email={request.Email}&token={validToken}";

        await _emailService.SendEmailAsync(request.Email, "Reset Password",
            $"<h1>Follow the instructions to reset your password</h1>" +
            $"<p>To reset your password <a href='{url}'>Click here</a>.</p>");

        return new UserResponse
        {
            Message = "Reset password url has been sent to your email successfully!", 
            IsSuccess = true
        };
    }
}