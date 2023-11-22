using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Application.ApplicationUsers.ResetPassword;

internal sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new UserResponse
            {
                Message = "No user associated with email!", 
                IsSuccess = false
            };
        }

        if (!request.Password.Equals(request.ConfirmPassword))
        {
            return new UserResponse
            {
                Message = "Password doesn't match its confirmation!", 
                IsSuccess = false
            };
        }

        var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ResetPasswordAsync(user, normalToken, request.Password);

        if (!result.Succeeded)
        {
            return new UserResponse
            {
                Message = "Something went wrong", 
                IsSuccess = false
            };
        }

        return new UserResponse
        {
            Message = "Password has been reset successfully.", 
            IsSuccess = true
        };
    }
}