using System.Text;
using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.ApplicationUsers.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return new UserResponse
            {
                Message = "User not found!", 
                IsSuccess = false
            };
        }

        var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ConfirmEmailAsync(user, normalToken);

        if (!result.Succeeded)
        {
            return new UserResponse
            {
                Message = "Email did not confirm!", 
                IsSuccess = false, 
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new UserResponse
        {
            Message = "Email confirmed successfully", 
            IsSuccess = true,
        };
    }
}