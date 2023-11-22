using System.ComponentModel.DataAnnotations;
using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.ForgetPassword;

public class ForgetPasswordCommand : IRequest<UserResponse>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}