using Application.Abstractions.Authentication;
using Domain.ApplicationUsers;
using Domain.Enums;
using infrastructure.Authentication.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;

namespace infrastructure.Authentication.Services;

public class JwtProviderService : IJwtProviderService
{
    private readonly JwtOptions _jwtOption;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtProviderService(IOptions<JwtOptions> jwtOption, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _jwtOption = jwtOption.Value;
    }

    public async Task<JwtResponseDto> GenerateToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("CustomerId", user.CustomerId.ToString() ?? string.Empty),
            new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? Role.Customer.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));

        var token = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtResponseDto
        {
            TokenAsString = tokenAsString,
            Token = token
        };
    }
}