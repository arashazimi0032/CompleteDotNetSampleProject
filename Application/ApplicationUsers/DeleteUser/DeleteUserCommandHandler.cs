using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUsers.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return new UserResponse
            {
                Message = "User not found!",
                IsSuccess = false
            };
        }

        var  response = await _userManager.DeleteAsync(user);
        if (!response.Succeeded)
        {
            return new UserResponse
            {
                Message = "Can not delete user.Something went wrong!",
                IsSuccess = false,
                Errors = response.Errors.Select(e => e.Description)
            };
        }

        return new UserResponse
        {
            Message = "User deleted successfully.",
            IsSuccess = true
        };
    }
}