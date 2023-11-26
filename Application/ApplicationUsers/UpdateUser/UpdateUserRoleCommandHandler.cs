using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUsers.UpdateUser;

internal sealed class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UpdateUserRoleCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UserResponse> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.Id.ToString()) ?? throw new UserNotFoundException(command.Id);

        var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        var newRole = command.Request.Role;

        if (string.IsNullOrWhiteSpace(newRole) || !await _roleManager.RoleExistsAsync(newRole))
        {
            newRole = Role.Customer.ToString();
        }

        if (newRole.Equals(currentRole))
            return new UserResponse
            {
                Message = "The Role has not changed.",
                IsSuccess = true
            };

        if (currentRole != null) await _userManager.RemoveFromRoleAsync(user, currentRole);

        var assignRoleResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!assignRoleResult.Succeeded)
        {
            return new UserResponse
            {
                Message = "Could not assign new role to the user!",
                IsSuccess = false,
                Errors = assignRoleResult.Errors.Select(e => e.Description)
            };
        }

        return new UserResponse
        {
            Message = "Role updated successfully.",
            IsSuccess = true
        };
    }
}