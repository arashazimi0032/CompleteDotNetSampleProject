using Application.Abstractions.Authentication;
using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.ApplicationUsers.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProviderService _jwtProviderService;

    public LoginUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtProviderService jwtProviderService)
    {
        _userManager = userManager;
        _jwtProviderService = jwtProviderService;
    }

    public async Task<UserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new UserResponse
            {
                Message = "There is no user with that Email address!",
                IsSuccess = false
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return new UserResponse
            {
                Message = "Invalid Password!",
                IsSuccess = false
            };
        }

        var tokenResponse = await _jwtProviderService.GenerateToken(user);

        return new UserResponse
        {
            Message = tokenResponse.TokenAsString,
            IsSuccess = true,
            ExpireDate = tokenResponse.Token.ValidTo
        };
    }
}
