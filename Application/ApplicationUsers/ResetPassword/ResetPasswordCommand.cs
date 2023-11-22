using System.ComponentModel.DataAnnotations;
using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.ResetPassword;

public class ResetPasswordCommand : IRequest<UserResponse>
{
    public string Token { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(150)]
    [MinLength(5)]
    public string Password { get; set; }
    
    [Required]
    [MaxLength(150)]
    [MinLength(5)]
    public string ConfirmPassword { get; set; }
}