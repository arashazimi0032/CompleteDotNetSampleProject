using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.ApplicationUsers.Share;
using Application.Authentication;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.ApplicationUsers.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public LoginUserCommandHandler(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
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

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("CustomerId", user.CustomerId.ToString() ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);


        return new UserResponse
        {
            Message = tokenAsString,
            IsSuccess = true,
            ExpireDate = token.ValidTo
        };
    }
}
