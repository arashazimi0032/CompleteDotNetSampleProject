using Application.ApplicationUsers.Share;
using Application.DTOs;
using Domain.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationUsers.GetAllUsers;

internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, UserQueryResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);

        var usersList = new List<UserDto>();

        foreach (var user in users)
        {
            var userDto = new UserDto(user.UserName, user.Email);
            usersList.Add(userDto);
        }

        return new UserQueryResponse
        {
            Message = $"{usersList.Count} Users were found.",
            IsSuccess = true,
            Users = usersList
        };
    }
}