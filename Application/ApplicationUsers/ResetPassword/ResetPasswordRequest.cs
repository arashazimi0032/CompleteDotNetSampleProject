using System.ComponentModel.DataAnnotations;

namespace Application.ApplicationUsers.ResetPassword;

public class ResetPasswordRequest
{
    [Required]
    [MaxLength(150)]
    [MinLength(5)]
    public string Password { get; set; }

    [Required]
    [MaxLength(150)]
    [MinLength(5)]
    public string ConfirmPassword { get; set; }
}