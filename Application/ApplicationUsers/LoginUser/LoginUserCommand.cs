using System.ComponentModel.DataAnnotations;
using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.LoginUser;

public class LoginUserCommand : IRequest<UserResponse>
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; }


    [Required]
    [MaxLength(100)]
    [MinLength(5)]
    public string Password { get; set; }

}