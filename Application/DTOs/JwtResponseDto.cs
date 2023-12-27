using System.IdentityModel.Tokens.Jwt;

namespace Application.DTOs;

public class JwtResponseDto
{
    public string TokenAsString { get; set; }
    public JwtSecurityToken Token { get; set; }
}