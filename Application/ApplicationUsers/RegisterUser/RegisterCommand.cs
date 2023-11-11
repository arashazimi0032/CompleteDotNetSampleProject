using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.RegisterUser
{
    public class RegisterCommand : IRequest<UserResponse>
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string ConfirmPassword { get; set; }
    }
}
