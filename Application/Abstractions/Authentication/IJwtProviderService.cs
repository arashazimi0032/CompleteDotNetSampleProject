using Application.DTOs;
using Domain.ApplicationUsers;

namespace Application.Abstractions.Authentication;

public interface IJwtProviderService
{
    Task<JwtResponseDto> GenerateToken(ApplicationUser user);
}